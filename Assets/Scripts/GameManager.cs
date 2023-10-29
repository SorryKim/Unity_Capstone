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

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
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

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
        RoomRenewal();
        chatInput.text = "";
        GameStart.GetComponent<Text>().text = "Ready";
        for (int i = 0; i < chatText.Length; i++) 
            chatText[i].text = "";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    
    public override void OnLeftRoom()
    {
     
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }

   

    void RoomRenewal()
    {
        roomInfo.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "명";
        var players = PhotonNetwork.CurrentRoom.Players;

        foreach (var temp in userList)
        {
            temp.text = "";
        }

        foreach (var player in players)
        {
            int idx = player.Key - 1;

            if (idx < 8)
            {
                if (player.Value.IsMasterClient)
                {
                    userList[idx].text = "<color=green>[방장]" + player.Value.NickName + "</color>";
                    GameStart.GetComponent<Text>().text = "Game Start";
                }


                else
                {
                    userList[idx].text = player.Value.NickName;
                    GameStart.GetComponent<Text>().text = "Ready";
                }

             }
            else
            {
                foreach(var temp in userList) { 
                    if(temp.text == "")
                    {
                        temp.text = player.Value.NickName;
                    }
                }
            }

            
        }
    }

   
    
    #endregion

    #region 채팅
    public void Send()
    {
        pv.RPC("ChatRPC", RpcTarget.All, (PhotonNetwork.NickName + " : " + chatInput.text));
        chatInput.text = "";
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
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
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < chatText.Length; i++) chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion
}
