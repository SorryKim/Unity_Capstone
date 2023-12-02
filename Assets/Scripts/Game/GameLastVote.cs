using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameLastVote : MonoBehaviourPunCallbacks
{
    public GameVote gameVote;
    public GameSystem gameSystem;
    public Button yesBtn, noBtn;
    public GameObject trueLiarPanel, falseLiarPanel, lastVotePanel;

    public int yesCnt;
    public int noCnt;
    public Text trueText, falseText;

    private bool isLiar;
    private bool isEnd;
    private float lastVoteTime;
    public List<Player> voters;
    

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
        isEnd = false;
        isLiar = (bool)candidate.CustomProperties["IsLiar"];
        falseText.text = candidate.NickName + "님은 <color=red>라이어</color>가 아닙니다.";
        trueText.text = candidate.NickName + "님은 <color=red>라이어</color>가 맞습니다.";
        yesBtn.gameObject.SetActive(true);
        noBtn.gameObject.SetActive(true);
        StartCoroutine(LastVote(candidate));    
    }

    // 찬성 버튼을 눌렀을 때
    public void OnClickYesBtn()
    {
        photonView.RPC("YesBtnRPC",RpcTarget.All);
        yesBtn.gameObject.SetActive(false);
        noBtn.gameObject.SetActive(false);
    }

    [PunRPC]
    void YesBtnRPC()
    {
        yesCnt++;
    }

    // 반대 버튼을 눌렀을 때
    public void OnClickNoBtn() {
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
        if (yesCnt >= noCnt)
        {
            if (isLiar)
            {
                trueLiarPanel.SetActive(true);
                if (PhotonNetwork.IsMasterClient)
                {
                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if (candidate == player)
                            continue;
                        player.AddScore(1);
                    }
                }
                yield return new WaitForSeconds(10f);
                trueLiarPanel.SetActive(false);
                gameSystem.GameStart();
            }
            else
            {
                falseLiarPanel.SetActive(true);

                // 기존 CustomProperties를 가져옴
                Hashtable customProperties = candidate.CustomProperties;

                // 업데이트된 값을 다시 CustomProperties에 저장
                customProperties["IsLive"] = false;
                candidate.SetCustomProperties(customProperties);
                yield return new WaitForSeconds(10f);

                falseLiarPanel.SetActive(false);
                gameVote.StartVote();

            }
            
        }
        else
        {
            gameVote.StartVote();
        }
    }
}
