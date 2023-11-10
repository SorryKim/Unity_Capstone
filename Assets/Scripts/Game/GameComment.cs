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
    public static GameComment instance;

    private int currentPlayerIndex = 0; // 현재 순서에 있는 플레이어의 인덱스
    private bool isMyTurn; // 현재 플레이어의 차례 여부
    private bool inputReceived = false; // 코멘트 입력여부

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
    }

    private void Update()
    {
        if (isMyTurn && Input.GetKeyDown(KeyCode.Return))
        {
            int myPlayerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
            Send(myPlayerIndex);
            EndTurn();
        }
    }
    #region 순서대로 코멘트

    public void StartComment()
    {
        currentPlayerIndex = gameSystem.commentStartIdx;
        UpdateTurnUI();
    }

    void UpdateTurnUI()
    {
        int myPlayerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        if(myPlayerIndex == currentPlayerIndex)
        {
            isMyTurn = true;
            commentPanel.SetActive(true);
            commentWaitPanel.SetActive(false);
            StartCoroutine(WaitForInput());
        }
        else
        {
            isMyTurn = false;
            commentPanel.SetActive(false);
            commentWaitPanel.SetActive(true);
        }
    }

    public void EndTurn()
    {
        int myPlayerIndex = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        if (isMyTurn)
        {

            // TODO: 코멘트 입력 처리 로직 추가
            photonView.RPC("Send", RpcTarget.AllBuffered,myPlayerIndex);
            // 다음 플레이어로 차례 이동
            currentPlayerIndex = (currentPlayerIndex + 1) % PhotonNetwork.PlayerList.Length;

            // 다음 플레이어에게 차례가 넘어갔음을 알림
            photonView.RPC("SendUpdateTurnUI", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SendUpdateTurnUI()
    {
        UpdateTurnUI();
    }

    IEnumerator WaitForInput()
    {
        float timer = 0f;
        float maxWaitTime = 30f;

        while (!inputReceived && timer < maxWaitTime)
        {
            // 입력이 없으면 대기
            yield return null;

            // 경과 시간 증가
            timer += Time.deltaTime;
        }

        // 입력이 없거나 대기 시간이 초과하면 처리
        commentPanel.SetActive(false);
    }

    #endregion

    #region 코멘트 작성
    public void Send(int idx)
    {
        photonView.RPC("SendComment", RpcTarget.All, (PhotonNetwork.LocalPlayer.NickName + " : " + commentInput.text), idx);
    }

    [PunRPC]
    public void SendComment(string msg, int idx)
    {
        comments[idx].text = msg;
    }
    #endregion
}