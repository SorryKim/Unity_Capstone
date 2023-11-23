using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameLastVote : MonoBehaviourPunCallbacks
{
    public int yesCnt;
    public int noCnt;

    public Button yesBtn, noBtn;
    public GameObject trueLiarPanel, falseLiarPanel, lastVotePanel;

    private bool isLiar;
    

    void Start()
    {
        yesCnt = 0;
        noCnt = 0;
    }

    public void StartLastVote(Player candidate)
    {
        isLiar = (bool)candidate.CustomProperties["IsLiar"];


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
                // 투표부터 다시 시작
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
