using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using System;
using UnityEngine.XR;

public class GameComment : MonoBehaviourPunCallbacks
{
    GameSystem gameSystem;
    GameManager gameManager;
    GameDiscussion gameDiscussion;
    public static GameComment instance;

    private bool enterPressed = false;

    public float commentDuration = 30f; // comment() 호출을 유지하는 시간

    private int currentPlayerIndex; // 현재 순서에 있는 플레이어의 인덱스
    private int startIndex;

    // 발표 순서
    public GameObject commentWaitPanel, commentPanel;
    public TMP_InputField commentInput;
    public TMP_Text[] comments;
   
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameSystem = GetComponent<GameSystem>();
        gameManager = GetComponent<GameManager>();
        gameDiscussion = GetComponent<GameDiscussion>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enterPressed = true;
        }

    }
    #region 순서대로 코멘트

    public void CommentStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 특정 플레이어부터 코멘팅을 시작하는 동작 수행
            startIndex = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);
            StartCoroutine(CommentingRoutine());
        }
    }

    private IEnumerator CommentingRoutine()
    {
        Player[] players = PhotonNetwork.PlayerList;
        currentPlayerIndex = startIndex;

        while(currentPlayerIndex < players.Length)
        {
            Player nowPlayer = players[currentPlayerIndex];
            photonView.RPC("StartCommenting", nowPlayer, currentPlayerIndex);

            foreach (var player in players)
            {
                if (nowPlayer.Equals(player))
                    continue;
                photonView.RPC("ShowWaitPanel", player);
            }

            float timer = 0f;
            while(timer < commentDuration && !enterPressed)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if(enterPressed || timer >= 10f)
            {
                enterPressed = false;
                photonView.RPC("EndCommenting", RpcTarget.All, currentPlayerIndex);
                photonView.RPC("HideAllPanels", RpcTarget.All);
                currentPlayerIndex++;
            }
        }

        currentPlayerIndex = 0;

        while(currentPlayerIndex < startIndex)
        {
            Player nowPlayer = players[currentPlayerIndex];
            photonView.RPC("StartCommenting", nowPlayer, currentPlayerIndex);

            foreach (var player in players)
            {
                if (nowPlayer.Equals(player))
                    continue;
                photonView.RPC("ShowWaitPanel", player);
            }

            float timer = 0f;
            while (timer < 10f && !enterPressed)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            if (enterPressed || timer >= 10f)
            {
                enterPressed = false;
                photonView.RPC("EndCommenting", RpcTarget.All, currentPlayerIndex);
                photonView.RPC("HideAllPanels", RpcTarget.All);
                currentPlayerIndex++;
            }
        }


        // 모든 플레이어의 코멘팅이 종료되면 원하는 작업 수행
        photonView.RPC("RPCDiscussion", RpcTarget.All);
    }

    [PunRPC]
    private void StartCommenting(int playerIndex)
    {
        // 특정 플레이어의 코멘팅을 시작하는 동작 수행
        Debug.Log("Player " + playerIndex + " Start Commenting");
        commentPanel.SetActive(true);
    }

    [PunRPC]
    private void EndCommenting(int playerIndex)
    {
        // 특정 플레이어의 코멘팅을 종료하는 동작 수행
        Debug.Log("Player " + playerIndex + " End Commenting");
        commentPanel.SetActive(false);
        commentWaitPanel.SetActive(false);
    }

    [PunRPC]
    private void HideAllPanels()
    {
        // 모든 패널을 숨김
        commentWaitPanel.SetActive(false);
        commentPanel.SetActive(false);
        // 추가로 숨기고 싶은 다른 패널이 있다면 여기에 추가
    }

    [PunRPC]
    private void ShowWaitPanel()
    {
        // 모든 플레이어에게 대기 패널을 보여주는 동작 수행
        commentPanel.SetActive(false);
        commentWaitPanel.SetActive(true);
    }

    [PunRPC]
    private void RPCDiscussion()
    {
        gameDiscussion.StartDiscussion();
    }


    #endregion

    #region 코멘트 작성
    public void CommentSend()
    {
        int idx = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        photonView.RPC("SendComment", RpcTarget.All, (PhotonNetwork.LocalPlayer.NickName + " : " + commentInput.text), idx);
    }

    [PunRPC]
    public void SendComment(string msg, int idx)
    {
        comments[idx].text = msg;
    }
    #endregion
}