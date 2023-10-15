using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //�ӵ� ����
    public TMP_Text nickname;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private PhotonView pv;

    // Awake : �����Ҷ� �ѹ��� ����
    // Update : �ϳ��� �����Ӹ��� �ѹ��� ȣ��Ǵ� �����ֱ� �Լ�
    // FixedUpdate : �������� �����Ӹ��� ȣ��
    // LateIpdate : �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        nickname.text = PlayerPrefs.GetString("nickname");
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        //inputVec.normalized : ���� ���� ũ�Ⱑ 1�� �ǵ��� ��ǥ�� ������ ��
        //Time.fixedDeltaTime : ���� ������ �ϳ��� �Һ��� �ð�
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        anim.SetFloat("speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x > 0;
        }
    }
}
