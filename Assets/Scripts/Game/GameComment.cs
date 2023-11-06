using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameComment : MonoBehaviourPunCallbacks
{
    GameSystem gameSystem;
    GameManager gameManager;
    public static GameComment instance;

    // 발표 순서
    public List<int> commentSequence;
    public GameObject commentWaitPanel, commentPanel;
    public TMP_InputField commentInput;
    public TMP_Text[] comments;

    // 발표 여부
    private bool isCommentAllowed = false;
    private bool isEnd = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameSystem = GetComponent<GameSystem>();
        gameManager = GetComponent<GameManager>();
        
    }


    // 발표 순서를 정하는 메소드
    public void SetSequence()
    {
        Player[] playerArr = PhotonNetwork.PlayerList;

        // 누구부터 시작할지
        int startIdx = UnityEngine.Random.Range(0, playerArr.Length);

        // 시작한사람부터 차례대로
        for (int i = startIdx; i < gameSystem.players.Length; i++)
        {
            commentSequence.Add(i);
        }
        for (int i = 0; i < startIdx; i++)
        {
            commentSequence.Add(i);
        }
    }



    // 발표 시작
    public void StartComment()
    {
        if(PhotonNetwork.IsMasterClient)
            SetSequence();

        StartCoroutine(CommentRoutine());
    }

    IEnumerator CommentRoutine()
    {
        foreach(var idx in commentSequence)
        {
            Player player = PhotonNetwork.PlayerList[idx];
            if(PhotonNetwork.LocalPlayer == player) 
                startComment();
            else
                while(!isEnd)
                    commentWaitPanel.SetActive(true);
        }
            
        yield return new WaitForSeconds(2f); // 대기 시간 설정

    }

    void startComment()
    {
        photonView.RPC("CommentingTime", RpcTarget.All);
        Commenting();
        StartCoroutine(StartTimer());
        photonView.RPC("CommentingEndTime", RpcTarget.All);
    }

    public void CommentWaiting()
    {
        commentWaitPanel.SetActive(true);
    }

    // 누군가 코멘팅 중일 때 rpc를 통해 모두에게 코멘팅중임을 알림
    [PunRPC]
    public void CommentingTime()
    {
        isEnd = false;
    }

    // 코멘팅이 끝난 경우
    [PunRPC]
    public void CommentingEndTime()
    {
        isEnd = true;
    }


    // 발표 메소드
    void Commenting()
    {
        // 코멘트 인풋창 활성화
        commentPanel.SetActive(true);
        isCommentAllowed = true;
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(30);
        if (isCommentAllowed)
        {
            Send();
        }
    }

    #region 코멘트 작성
    public void Send()
    {
        photonView.RPC("SendComment", RpcTarget.All, (PhotonNetwork.LocalPlayer.NickName + " : " + commentInput.text));
    }

    [PunRPC]
    public void SendComment(string msg)
    {
        isCommentAllowed = false;
        int idx = 0;
        List<Player> temp = gameManager.players;
        for(int i = 0; i < temp.Count; i++)
        {
            if (temp[i] == PhotonNetwork.LocalPlayer)
            {
                idx = i;
                break;
            }
        }
        comments[idx].text = msg;
        commentPanel.SetActive(false);
    }
    #endregion
}