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
        bool[] check = new bool[gameSystem.players.Length];
        for (int i = 0; i < check.Length; i++)
            check[i] = false;

        // 누구부터 시작할지
        int startIdx = UnityEngine.Random.Range(0, gameSystem.players.Length);

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
        foreach (var num in commentSequence)
        {
            Player player = gameSystem.players[num];
            if(player == PhotonNetwork.LocalPlayer)
                Commenting();
        }
    }

    // 발표 메소드
    void Commenting()
    {
        // 발표자
        if (photonView.IsMine)
        {
            // 코멘트 인풋창 활성화
            commentPanel.SetActive(true);
            isCommentAllowed = true;
            
        }
        // 발표가 아닌 사람들
        else
        {
            // 코멘트 대기 패널 활성화
            commentWaitPanel.SetActive(true);
        }
    }

 

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(30);
        if (isCommentAllowed)
        {
            Send();
        }
    }

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

   
}