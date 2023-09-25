using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //�ӵ� ����

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // Awake : �����Ҷ� �ѹ��� ����
    // Update : �ϳ��� �����Ӹ��� �ѹ��� ȣ��Ǵ� �����ֱ� �Լ�
    // FixedUpdate : �������� �����Ӹ��� ȣ��
    // LateIpdate : �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
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
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
