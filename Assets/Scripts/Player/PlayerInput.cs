using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 해당 로컬 플레이어가 아닌 경우 입력 받지 않음
        if (!photonView.IsMine)
        {
            return;
        }


    }
}
