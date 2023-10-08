using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Text msgList;
    public InputField inputMsg;

    
    void Start()
    {
        CreatePlayer();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    public void OnSendChatMsg()
    {
        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, inputMsg.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
    }

    [PunRPC]
    void ReceiveMsg(string msg)
    {
        msgList.text += "\n" + msg;
    }

    void Update()
    {
        
    }
}
