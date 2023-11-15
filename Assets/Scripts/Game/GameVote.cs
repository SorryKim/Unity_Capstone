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
    public GameObject[] voteList;
    public GameObject votePanel;
    public Button[] voteButtons;
    public bool isVoteStart;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isVoteStart = false;
        
    }



    public void StartVote()
    {
        votePanel.SetActive(true);
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            string nickname = players[i].NickName;
            if (nickname != PhotonNetwork.LocalPlayer.NickName)
            {
                voteList[i].SetActive(true);
                voteList[i].GetComponentInChildren<TMP_Text>().text = players[i].NickName;
                Transform buttonTransform = voteList[i].transform.Find("Button");
                int idx = i;
                buttonTransform.GetComponent<Button>().onClick.AddListener(() => OnVoteClick(players[idx].NickName));
            }
        }

        isVoteStart = true;

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
                int cnt = (int)player.CustomProperties["VoteCount"];
                Transform votersTransform = voteObject.transform.Find("voters");

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
