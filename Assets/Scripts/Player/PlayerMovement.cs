using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //속도 관리

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // Awake : 시작할때 한번만 실행
    // Update : 하나의 프레임마다 한번씩 호출되는 생명주기 함수
    // FixedUpdate : 물리연산 프레임마다 호출
    // LateIpdate : 프레임이 종료되기 전 실행되는 생명주기 함수

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
        //inputVec.normalized : 벡터 값의 크기가 1이 되도록 좌표가 수정된 값
        //Time.fixedDeltaTime : 물리 프레임 하나가 소비한 시간
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
