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
    public float speed; //속도 관리
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;

    // Awake : 시작할때 한번만 실행
    // Update : 하나의 프레임마다 한번씩 호출되는 생명주기 함수
    // FixedUpdate : 물리연산 프레임마다 호출
    // LateIpdate : 프레임이 종료되기 전 실행되는 생명주기 함수

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {

        // 닉네임 설정
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;

        // 카메라 설정
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

            //inputVec.normalized : 벡터 값의 크기가 1이 되도록 좌표가 수정된 값
            //Time.fixedDeltaTime : 물리 프레임 하나가 소비한 시간
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
