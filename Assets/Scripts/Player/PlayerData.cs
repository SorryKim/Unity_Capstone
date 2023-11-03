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
