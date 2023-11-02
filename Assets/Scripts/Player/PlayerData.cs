using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PlayerData : MonoBehaviour
{

    // 플레이어 데이터 사용할 변수
    private int score, colorNum;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            int temp = PhotonNetwork.CurrentRoom.PlayerCount;
            PhotonNetwork.LocalPlayer.SetPlayerNumber(temp);
            playerColorNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            anim.runtimeAnimatorController = animCon[playerColorNum];
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void SyncColor()
    {

    }
}
