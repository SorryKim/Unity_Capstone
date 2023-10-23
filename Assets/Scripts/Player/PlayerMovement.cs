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
    public float speed; //ï¿½Óµï¿½ ï¿½ï¿½ï¿½ï¿½
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;

    // Awake : ï¿½ï¿½ï¿½ï¿½ï¿½Ò¶ï¿½ ï¿½Ñ¹ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    // Update : ï¿½Ï³ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ó¸ï¿½ï¿½ï¿½ ï¿½Ñ¹ï¿½ï¿½ï¿½ È£ï¿½ï¿½Ç´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ö±ï¿½ ï¿½Ô¼ï¿½
    // FixedUpdate : ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ó¸ï¿½ï¿½ï¿½ È£ï¿½ï¿½
    // LateIpdate : ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ç±ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ç´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ö±ï¿½ ï¿½Ô¼ï¿½

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {

        // ´Ð³×ÀÓ ¼³Á¤
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;

        // Ä«¸Þ¶ó ¼³Á¤
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

            //inputVec.normalized : ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Å©ï¿½â°¡ 1ï¿½ï¿½ ï¿½Çµï¿½ï¿½ï¿½ ï¿½ï¿½Ç¥ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½
            //Time.fixedDeltaTime : ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ï³ï¿½ï¿½ï¿½ ï¿½Òºï¿½ï¿½ï¿½ ï¿½Ã°ï¿½
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
