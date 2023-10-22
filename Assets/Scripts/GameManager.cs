using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public bool isConnect = false;
    public string nickname = "";

    private PhotonView pv;

    private void Awake()
    {
       
    }

    private void Start()
    {

        Debug.Log("방이름 :" + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("최대인원수" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString());
        if(PhotonNetwork.CurrentRoom.PlayerCount <=1 )
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);

    }

  

   
    public override void OnJoinedRoom()
    {
      
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
    }




    private void Update()
    {

    }


    void CreatePlayer()
    {
        GameObject temp = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
    }
}
