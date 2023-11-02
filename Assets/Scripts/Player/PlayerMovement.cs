using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;
using JetBrains.Annotations;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //�ӵ� ����
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;
    private bool isVote;
    public RuntimeAnimatorController[] animCon;
    public Transform teleportTarget;

    // Awake : �����Ҷ� �ѹ��� ����
    // Update : �ϳ��� �����Ӹ��� �ѹ��� ȣ��Ǵ� �����ֱ� �Լ�
    // FixedUpdate : �������� �����Ӹ��� ȣ��
    // LateIpdate : �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();    }
    //void OnEnable()
    //{
    //    anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    //}

    private void Start()
    {
        int playerId = Random.Range(0, animCon.Length);
        anim.runtimeAnimatorController = animCon[playerId];
        isVote = false;
        DontDestroyOnLoad(this.gameObject);
        // �г��� ����
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;

        // ī�޶� ����
        if (pv.IsMine)
        {
            var cm = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            cm.Follow = transform;
            cm.LookAt = transform;
        }

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

            //inputVec.normalized : ���� ���� ũ�Ⱑ 1�� �ǵ��� ��ǥ�� ������ ��
            //Time.fixedDeltaTime : ���� ������ �ϳ��� �Һ��� �ð�
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
