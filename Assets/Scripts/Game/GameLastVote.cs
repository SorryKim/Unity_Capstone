using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class GameLastVote : MonoBehaviourPunCallbacks
{
    public GameVote gameVote;
    public GameSystem gameSystem;
    public Button yesBtn, noBtn;
    public GameObject trueLiarPanel, falseLiarPanel, lastVotePanel, liarWinPanel, winnerPanel;

    public int yesCnt;
    public int noCnt;
    public Text trueText, falseText, liarWinText;
    public TMP_Text winnerName;

    private bool isLiar;
    private bool isEnd;
    private float lastVoteTime;
    public List<Player> voters;

    //최종투표 이미지 배열
    public Sprite[] player_images;
    public Sprite[] liar_images;

    //최종투표
    public Image player_image;
    public Image liar_image;

    //결과화면
    public RawImage re_liarimage;
    public RawImage re_playerimage;


    void Start()
    {
        gameSystem = GetComponent<GameSystem>();
        gameVote = GetComponent<GameVote>();
        yesCnt = 0;
        noCnt = 0;
        lastVoteTime = 10f;
    }

    public void StartLastVote(Player candidate)
    {
        int userIndex = (candidate.ActorNumber - 1) % 8; // 0~7, actornumber가 8이 되면 다시 0부터

        player_image.sprite = player_images[userIndex];
        liar_image.sprite = liar_images[userIndex];
        isEnd = false;
        isLiar = (bool)candidate.CustomProperties["IsLiar"];

        re_liarimage = gameSystem.ConvertSpriteToRawImage(gameSystem.liarImages[userIndex]);
        re_playerimage = gameSystem.ConvertSpriteToRawImage(gameSystem.playerImages[userIndex]);

        falseText.text = candidate.NickName + "님은 <color=red>라이어</color>가 아닙니다.";
        trueText.text = candidate.NickName + "님은 <color=red>라이어</color>가 맞습니다.";
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
        StartCoroutine(LastVote(candidate));
    }

    // 찬성 버튼을 눌렀을 때
    public void OnClickYesBtn()
    {
        photonView.RPC("YesBtnRPC", RpcTarget.All);
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
    }

    [PunRPC]
    void YesBtnRPC()
    {
        yesCnt++;
    }

    // 반대 버튼을 눌렀을 때
    public void OnClickNoBtn()
    {
        photonView.RPC("NoBtnRPC", RpcTarget.All);
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
    }

    [PunRPC]
    void NoBtnRPC()
    {
        noCnt++;
    }

    IEnumerator LastVote(Player candidate)
    {
        bool b = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsLive"];
        if (b)
            lastVotePanel.SetActive(true);

        yield return new WaitForSeconds(lastVoteTime);
        lastVotePanel.SetActive(false);

        // 최종투표 결과
        if (yesCnt >= noCnt)
        {
            bool isEnd = false;
            // 라이어를 맞춘 경우
            if (isLiar)
            {
                isEnd = true;
                trueLiarPanel.SetActive(true);
                if (PhotonNetwork.IsMasterClient)
                {
                    // 라이어가 아닌 사람들에게 1점 부과
                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (candidate == player)
                            continue;
                        player.AddScore(1);
                    }
                }
                yield return new WaitForSeconds(10f);
                trueLiarPanel.SetActive(false);
            }
            // 라이어가 아닌 경우
            else
            {
                falseLiarPanel.SetActive(true);

                // 기존 CustomProperties를 가져옴
                Hashtable customProperties = candidate.CustomProperties;

                // 탈락
                customProperties["IsLive"] = false;
                candidate.SetCustomProperties(customProperties);
                yield return new WaitForSeconds(10f);

                falseLiarPanel.SetActive(false);

                int temp = 0;
                Player liar = null;
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    bool isLive = (bool)player.CustomProperties["IsLive"];
                    bool isLiar = (bool)player.CustomProperties["IsLiar"];
                    if (isLive)
                        temp++;

                    if (isLiar)
                        liar = player;
                }

                // 살아남은 사람이 2명인 경우
                // 라이어의 승리
                if (temp == 2)
                {
                    // 라이어 승리패널
                    if (liar != null)
                    {
                        isEnd = true;
                        liarWinText.text = liar.NickName + "님이 <color=red>라이어</color>입니다!";
                        liarWinPanel.SetActive(true);
                        liar.AddScore(3);
                        yield return new WaitForSeconds(10f);
                        liarWinPanel.SetActive(false);
                    }
                    
                }
                else
                {
                    gameVote.StartVote();
                }
                

            }
            if(isEnd)
                ScoreCheck();
            else
                gameVote.StartVote();
        }
        // 최종 투표가 불발된 경우
        else
        {
            gameVote.StartVote();
        }
    }

    // 플레이어의 점수를 확인하여 
    void ScoreCheck()
    {
        bool isEnd = false;
        int cnt = 0;
        Room room = PhotonNetwork.CurrentRoom;
        int maxScore = (int)room.CustomProperties["MaxScore"];
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int score = player.GetScore();
            if (score >= maxScore)
            {
                winnerName.text = player.NickName + "님이 최종승리하셨습니다~!";
                winnerPanel.SetActive(true);
                isEnd = true;
            }
        }

        // 최종승리자가 없는경우 게임 재시작
        if (!isEnd)
            gameSystem.GameStart();
    }
}