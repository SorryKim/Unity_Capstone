using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameVote : MonoBehaviourPunCallbacks
{
    public static GameVote instance;
    public GameLast gameLast;
    public GameObject[] voteList;
    public GameObject votePanel, reVotePanel;
    public Button[] voteButtons;
    public bool isVoteStart;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isVoteStart = false;
        gameLast = GetComponent<GameLast>();
    }


    public void StartVote()
    {
        isVoteStart = true;
        Player player = PhotonNetwork.LocalPlayer;

        // 기존 CustomProperties를 가져옴
        Hashtable customProperties = player.CustomProperties;
        
        // 업데이트된 값을 다시 CustomProperties에 저장
        customProperties["VoteCount"] = 0;
        customProperties["IsVote"] = false;
        player.SetCustomProperties(customProperties);
        
        StartCoroutine(VoteRoutine());
    }

    private IEnumerator VoteRoutine() {

        // VotePanel 활성화
        votePanel.SetActive(true);
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            string nickname = players[i].NickName;
            // 다른 사람들의 투표공간
            if (nickname != PhotonNetwork.LocalPlayer.NickName)
            {
                voteList[i].SetActive(true);
                voteList[i].GetComponentInChildren<TMP_Text>().text = players[i].NickName;
                voteList[i].transform.Find("Button").gameObject.SetActive(true);
                Transform buttonTransform = voteList[i].transform.Find("Button");
                int idx = i;
                buttonTransform.GetComponent<Button>().onClick.AddListener(() => OnVoteClick(players[idx].NickName));
            }
            // 자신의 공간일 경우 버튼칸은 비활성화인 상태로 생성
            else
            {
                
                voteList[i].SetActive(true);
                voteList[i].GetComponentInChildren<TMP_Text>().text = players[i].NickName;
                voteList[i].transform.Find("Button").gameObject.SetActive(false);
            }
        }

        // 30초 대기
        yield return new WaitForSeconds(30f);

        // VotePanel 비활성화
        votePanel.SetActive(false);
        EndVote();
    }

    public void OnVoteClick(string buttonNickName)
    {
        foreach(Player player in PhotonNetwork.PlayerList) {
            if(buttonNickName == player.NickName)
            {
                // 기존 CustomProperties를 가져옴
                Hashtable customProperties = player.CustomProperties;

                // "VoteCount" 키에 해당하는 값을 가져오고, 없으면 0으로 초기화
                int voteCount = customProperties.ContainsKey("VoteCount") ? (int)customProperties["VoteCount"] : 0;

                // VoteCount를 1 증가시킴
                voteCount++;

                // 업데이트된 값을 다시 CustomProperties에 저장
                customProperties["VoteCount"] = voteCount;
                player.SetCustomProperties(customProperties);
            }
        }

        foreach(Button btn in voteButtons)
        {
            btn.gameObject.SetActive(false);
        }

        Player temp = PhotonNetwork.LocalPlayer;
        Hashtable customProperty = temp.CustomProperties;
        // 업데이트된 값을 다시 CustomProperties에 저장
        customProperty["IsVote"] = true;
        temp.SetCustomProperties(customProperty);

    }

    void EndVote()
    {
        int max = 0;
        Player[] players = PhotonNetwork.PlayerList;
        List<Player> list = new List<Player>();
      
        foreach (Player player in players)
        {
            int temp = (int)player.CustomProperties["VoteCount"];
            
            // 최다표를 받은 경우
            if(temp > max)
            {
                list.Clear();
                list.Add(player);
            }
            // 동률인 경우
            else if(temp == max)
            {
                list.Add(player);
            }
        }

        gameLast.StartLast(list[0]);

        //// 과반수도 아니고 동률인 경우
        //if(max < players.Length || list.Count >=2)
        //{
        //    StartCoroutine(ReVote());     
        //}
        //else
        //{
        //    //최종 반론 시작
        //    gameLast.StartLast(list[0]);
        //}
    }

    IEnumerator ReVote()
    {
        reVotePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        reVotePanel.SetActive(false);
        // 투표 재시작
        StartVote();

    }
    void Update()
    {
        if (isVoteStart)
        {
            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                GameObject voteObject = voteList[i];
                // 투표당한 수
                int cnt = (int)player.CustomProperties["VoteCount"];
                // 투표완료 여부
                bool isVote = (bool)player.CustomProperties["IsVote"];

                Transform votersTransform = voteObject.transform.Find("voters");
                
                // 투표한 경우 투표표시
                if (isVote)
                    voteObject.transform.Find("Isvote").gameObject.SetActive(true);

                if (votersTransform != null)
                {
                    for(int j = 0; j < cnt; j++)
                    {
                        votersTransform.GetChild(j).gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("voters not found under Player1!");
                }

            }
        }
    }


}
