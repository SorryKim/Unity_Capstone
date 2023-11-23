using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using UnityEngine.UI;

public class GameLast : MonoBehaviourPunCallbacks
{
    public GameLastVote gameLastVote;
    public GameObject lastCommentPanel;
    public GameObject lastCommentWaitPanel;
    public TMP_InputField lastCommentInput;
    public Text lastComment;

    public Player candidate;
    // Start is called before the first frame update
    void Start()
    {
        gameLastVote = GetComponent<GameLastVote>();
    }

    void Update()
    {
        
    }

    public void StartLast(Player candidate)
    {
        this.candidate = candidate;
        Player player = PhotonNetwork.LocalPlayer;
        lastCommentWaitPanel.transform.Find("Title").GetComponent<TMP_Text>().text = player.NickName + "님의 최후의 변론...";
        // 최종후보인 경우
        if (player.Equals(candidate))
        {
            lastCommentPanel.SetActive(true);
        }
        // 나머지 플레이어들
        else
        {
            lastCommentWaitPanel.SetActive(true);
        }
    }



    #region 최후의변론 전송
    public void SendLastComment()
    {
        photonView.RPC("SendLastCommentRPC", RpcTarget.All, lastCommentInput.text);
    }

    [PunRPC]
    public void SendLastCommentRPC(string msg)
    {
        lastComment.text = msg;
        gameLastVote.StartLastVote(candidate);
    }
    #endregion
}
