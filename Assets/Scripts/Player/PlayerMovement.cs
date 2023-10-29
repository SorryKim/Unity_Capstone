using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //�ӵ� ����
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;
    public RuntimeAnimatorController[] animCon;

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

        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];

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

        }
        
    }

    [PunRPC]
    void FlipXRPC(float axis)
    {
        spriter.flipX = axis == 1;
    }


   
}
