using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float voteTime;

    public RawImage[] images;

    public Sprite[] sprites;


 
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
        isVoteStart = false;
        StartCoroutine(VoteSetting()); 
    }

    #region Vote 루틴
    IEnumerator VoteSetting()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer; // 다른 변수 이름을 사용하여 혼란을 방지합니다.
                                                        // 기존 CustomProperties를 가져옴
        Hashtable customProperties = localPlayer.CustomProperties;

        // 업데이트된 값을 다시 CustomProperties에 저장
        customProperties["VoteCount"] = 0;
        customProperties["IsVote"] = false;
        localPlayer.SetCustomProperties(customProperties);

        yield return new WaitForSeconds(3f);
        StartCoroutine(VoteRoutine());
    }


    private IEnumerator VoteRoutine() {

        yield return new WaitForSeconds(1f);

        isVoteStart = true;
        // VotePanel 활성화
        votePanel.SetActive(true);
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            bool b = (bool)players[i].CustomProperties["IsLive"];
            if (b) {
                string nickname = players[i].NickName;
                // 다른 사람들의 투표공간
                if (nickname != PhotonNetwork.LocalPlayer.NickName)
                {
                    voteList[i].SetActive(true);
                    voteList[i].GetComponentInChildren<TMP_Text>().text = nickname;
                    voteList[i].transform.Find("Button").gameObject.SetActive(true);

                    Texture2D playerTexture = GetPlayerTextureByActorNumber(players[i].ActorNumber);
                    voteList[i].GetComponentInChildren<RawImage>().texture = playerTexture;
                    Transform buttonTransform = voteList[i].transform.Find("Button");
                    buttonTransform.GetComponent<Button>().onClick.AddListener(() => OnVoteClick(nickname));
                }
                // 자신의 공간일 경우 버튼칸은 비활성화인 상태로 생성
                else
                {
                    voteList[i].SetActive(true);
                    voteList[i].GetComponentInChildren<TMP_Text>().text = nickname;
                    voteList[i].transform.Find("Button").gameObject.SetActive(false);

                    Texture2D userTexture = GetPlayerTextureByActorNumber(PhotonNetwork.LocalPlayer.ActorNumber);
                    voteList[i].GetComponentInChildren<RawImage>().texture = userTexture;
                }
            }
        }

        // 30초 대기
        yield return new WaitForSeconds(voteTime);

        // VotePanel 비활성화
        votePanel.SetActive(false);
        EndVote();
    }



    public void OnVoteClick(string buttonNickName)
    {
        foreach (Button btn in voteButtons)
            btn.gameObject.SetActive(false);

        photonView.RPC("UpdateVoteCount", RpcTarget.All, buttonNickName);
    }

    [PunRPC]
    void UpdateVoteCount(string playerName)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (playerName == player.NickName)
            {
                // 기존 CustomProperties를 가져옴
                Hashtable customProperties = player.CustomProperties;

                // "VoteCount" 키에 해당하는 값을 가져오고, 없으면 0으로 초기화
                int voteCount = (int)customProperties["VoteCount"];

                // VoteCount를 1 증가시킴
                voteCount++;

                // 업데이트된 값을 다시 CustomProperties에 저장
                customProperties["VoteCount"] = voteCount;
                player.SetCustomProperties(customProperties);
            }
        }

        Player temp = PhotonNetwork.LocalPlayer;
        Hashtable customProperty = temp.CustomProperties;
        // 업데이트된 값을 다시 CustomProperties에 저장
        customProperty["IsVote"] = true;
        temp.SetCustomProperties(customProperty);
        UpdateVoteList();
    }


    void EndVote()
    {
        votePanel.SetActive(false);
        Player[] players = PhotonNetwork.PlayerList;
        List<Player> list = new List<Player> ();
        if (PhotonNetwork.IsMasterClient)
        {
            int maxV = 0;

            foreach(Player player in players)
            {
                int nowCnt = (int)player.CustomProperties["VoteCount"];
                //투표최댓값인 경우
                if(nowCnt > maxV)
                {
                    maxV = nowCnt;
                    list.Clear();
                    list.Add(player);
                }else if(nowCnt == maxV)
                {
                    list.Add(player);
                }
            }

            if(list.Count >= 2)
            {
                photonView.RPC("ReVoteRPC", RpcTarget.All);
            }else if(list.Count == 1)
            {
                int temp = (int)list[0].CustomProperties["VoteCount"];
                if(temp >= players.Length / 2)
                {
                    photonView.RPC("GoLast", RpcTarget.All, list[0]);
                }
                else
                {
                    photonView.RPC("ReVoteRPC", RpcTarget.All);
                }
            }
            else
            {
                photonView.RPC("ReVoteRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void ReVoteRPC()
    {
        StartCoroutine(ReVote());
    }
    IEnumerator ReVote()
    {
        reVotePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        reVotePanel.SetActive(false);
        // 투표 재시작
        StartVote();
    }

    [PunRPC]
    void GoLast(Player player)
    {
        gameLast.StartLast(player);
    }

    void UpdateVoteList()
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
                Debug.Log(player.NickName + "의 투표수: " + cnt);
                // 투표한 경우 투표표시
                if (isVote)
                    voteObject.transform.Find("Isvote").gameObject.SetActive(true);
                else
                    voteObject.transform.Find("Isvote").gameObject.SetActive(false);

                if (votersTransform != null)
                {
                    // 업데이트 전에 "voters" 아래의 모든 자식 오브젝트를 재설정
                    for (int j = 0; j < votersTransform.childCount; j++)
                    {
                        votersTransform.GetChild(j).gameObject.SetActive(false);
                    }

                    // 현재 투표 수에 따라 업데이트
                    for (int j = 0; j < cnt; j++)
                    {
                        votersTransform.GetChild(j).gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogError("Player1 아래에 voters를 찾을 수 없습니다!");
                }

            }

        }
    }

    #endregion
    void Update()
    {
        
    }

    #region 투표 이미지
    Texture2D GetPlayerTextureByActorNumber(int actorNumber)
    {
        int userIndex = (actorNumber - 1) % 8;
        Sprite playerSprite = sprites[userIndex];

        return SpriteToTexture(playerSprite);
    }
    Texture2D SpriteToTexture(Sprite sprite)
    {
        // Sprite를 Texture2D로 변환하는 코드
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
                          (int)sprite.rect.width, (int)sprite.rect.height));
        texture.Apply();
        return texture;
    }
    #endregion

}
