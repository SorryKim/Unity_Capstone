using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Animations;
using Cinemachine;
using JetBrains.Annotations;




public class PlayerMovement : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public Vector2 inputVec;
    public float speed;
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;
    private bool isVote;
    public RuntimeAnimatorController[] animCon;
    public Transform teleportTarget;


    // 플레이어의 색깔번호
    //[SerializeField] int colorNum;
    //public int ColorNum { get => colorNum; set => ActionRPC(nameof(SetColorNum), value); }
    //[PunRPC] void SetColorNum(int value) => colorNum = value;

    void ActionRPC(string functionName, object value)
    {
        pv.RPC(functionName, RpcTarget.AllBufferedViaServer, value);
    }

    //public void InvokeProperties()
    //{
    //    ColorNum = ColorNum;
    //}

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>(); 
    }

    [PunRPC]
    void Character(int actorNumber)
    {

        int userIndex = actorNumber % 8 -1; //0~7, actornumber가 8이 되면 다시 0부터

        if (userIndex >= 0 && userIndex < animCon.Length)
        {
            // 캐릭터 할당
            RuntimeAnimatorController selectedController = animCon[userIndex];
            anim.runtimeAnimatorController = selectedController;
        }
        else
        {
            Debug.LogError("Invalid user index or character index.");
        }
    }


    //void OnEnable()
    //{
    //    anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    //}

    private void Start()
    {
        
        isVote = false;
        DontDestroyOnLoad(this.gameObject);
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        //ColorNum = PhotonNetwork.CurrentRoom.PlayerCount - 1;

        if (pv.IsMine)
        {
            var cm = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            cm.Follow = transform;
            cm.LookAt = transform;
            //캐릭터할당요청 & 서버에 전달
            pv.RPC("Character", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }

        

    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
    }

    // 순간이동 실행
    public void Teleport()
    {
        // 현재 플레이어가 로컬 플레이어인지 확인
        if (!isVote && Input.GetKeyDown(KeyCode.Escape))
        {
            // 캐릭터의 위치와 회전을 순간이동할 위치로 설정
            Debug.Log("순간이동");
            transform.position = new Vector3(-25,0,0);
           

            // 위치 및 회전 정보를 다른 플레이어에게 동기화
            pv.RPC("SyncTeleport", RpcTarget.Others, new Vector3(-25, 0, 0));
            isVote = !isVote;
        }
        else if (isVote && Input.GetKeyDown(KeyCode.Escape))
        {
            // 캐릭터의 위치와 회전을 순간이동할 위치로 설정
            Debug.Log("순간이동");
            transform.position = new Vector3(0, 0, 0);


            // 위치 및 회전 정보를 다른 플레이어에게 동기화
            pv.RPC("SyncTeleport", RpcTarget.Others, new Vector3(0, 0, 0));
            isVote = !isVote;
        }
    }

    // RPC를 통해 순간이동 정보를 다른 플레이어에게 전달
    [PunRPC]
    void SyncTeleport(Vector3 position)
    {
        transform.position = position;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            inputVec.x = Input.GetAxisRaw("Horizontal");
            inputVec.y = Input.GetAxisRaw("Vertical");

            rigid.velocity = new Vector2(3 * inputVec.x, 3 * inputVec.y);

            if (inputVec.x != 0)
            {
                pv.RPC("FlipXRPC", RpcTarget.AllBuffered, inputVec.x);
            }

            if (rigid.velocity != Vector2.zero)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }

            Teleport();

        }
        
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        spriter.flipX = axis == 1;
    }


   
}
