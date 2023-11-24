using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameLastVote : MonoBehaviourPunCallbacks
{
    public GameVote gameVote;

    public Button yesBtn, noBtn;
    public GameObject trueLiarPanel, falseLiarPanel, lastVotePanel;

    public int yesCnt;
    public int noCnt;
    public Text trueText, falseText;

    private bool isLiar;
    

    void Start()
    {
        gameVote = GetComponent<GameVote>();
        yesCnt = 0;
        noCnt = 0;
    }

    public void StartLastVote(Player candidate)
    {
        isLiar = (bool)candidate.CustomProperties["IsLiar"];
        lastVotePanel.SetActive(true);
        falseText.text = candidate.NickName + "님은 <color=red>라이어</color>가 아닙니다.";
        trueText.text = candidate.NickName + "님은 <color=red>라이어</color>가 맞습니다.";
    }

    // Update is called once per frame
    void Update()
    {
        int cnt = PhotonNetwork.PlayerList.Length;
        if ((yesCnt + noCnt) == cnt)
        {
            if(yesCnt >= (cnt / 2)){
                if (isLiar)
                {
                    lastVotePanel.SetActive(false);
                    trueLiarPanel.SetActive(true);
                }
                else
                {
                    lastVotePanel.SetActive(false);
                    falseLiarPanel.SetActive(true);
                }
            }
            else
            {
                gameVote.StartVote();
            }
        }
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
}
