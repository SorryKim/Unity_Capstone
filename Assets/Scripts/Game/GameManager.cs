using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.Demo.Cockpit;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Windows;
using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using Random = System.Random;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public GameSystem gameSystem;
    public bool isConnect = false;
    public string nickname = "";

    public InputField chatInput;
    public Text roomInfo;
    public Text[] chatText, userList;
    public Text GameStart;
    

    private PhotonView pv;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        gameSystem = GetComponent<GameSystem>();
    }

    void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);

        RoomRenewal();
    }

    private void Update()
    { 
            

    }


    #region 방

    // 플레이어가 들어 왔을 때
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
        
        RoomRenewal();
        chatInput.text = "";
        for (int i = 0; i < chatText.Length; i++) 
            chatText[i].text = "";

       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=red>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=red>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    
    public override void OnLeftRoom()
    {
     
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }

   

    public void RoomRenewal()
    {
        Photon.Realtime.Player currentPlayer = PhotonNetwork.LocalPlayer;

        if (currentPlayer.IsMasterClient)
        {
            GameStart.text = "GameStart";
        }
        else
        {
            GameStart.text = "Ready";
        }
        roomInfo.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "명";


        foreach (var temp in userList)
        {
            temp.text = "";
        }
        int idx = 0;

        Player[] playerArr = PhotonNetwork.PlayerList;
        foreach (var player in playerArr)
        {
            if (player.IsMasterClient)
                userList[idx].text = "<color=green>[방장]" + player.NickName + "</color>" + " / " + player.GetScore().ToString() + "점";
            else
                userList[idx].text = player.NickName + " / " + player.GetScore().ToString() + "점"; ;
            idx++;
        }
    }

   public void checkId()
    {
        var list = PhotonNetwork.CurrentRoom.Players;

        foreach(var player in list)
        {
            Debug.Log("번호: " + player.Key.ToString() + " 닉네임: " + player.Value.NickName);
        }
    }
    
    #endregion

    #region 채팅
    public void Send()
    {
        pv.RPC("ChatRPC", RpcTarget.All, (PhotonNetwork.NickName + " : " + chatInput.text));
        chatInput.text = "";
    }

    [PunRPC] 
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            }
        if (!isInput) 
        {
            for (int i = 1; i < chatText.Length; i++) chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion
}
