using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool isConnect = false;
    public string nickname = "";

    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        CreatePlayer();

        Debug.Log("방이름 :" + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("최대인원수" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString());
    }

    private void Update()
    {
        
    }

    void CreatePlayer()
    {
        GameObject temp = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
    }
}
