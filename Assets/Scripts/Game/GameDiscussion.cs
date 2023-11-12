using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDiscussion : MonoBehaviour
{
    public Transform targetLocation; // 이동시킬 대상 장소
    public static GameDiscussion instance;
    private PhotonView pv;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }


    public void StartDiscussion()
    {
        MoveAllPlayersToTargetLocation();
    }


    void MoveAllPlayersToTargetLocation()
    {
        Debug.Log("토론장소로 이동");
        // 모든 플레이어를 가져옴
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 각 플레이어에 대해 이동 명령 전송
        foreach (GameObject player in players)
        {
            player.transform.position = targetLocation.position;
        }
    }

   
}
