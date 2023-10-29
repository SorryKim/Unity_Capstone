using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviourPunCallbacks
{
    private bool isGameStart;
    private PhotonView pv;
    public GameObject themePanel, identityPanel, gameStartBtn;
    public Text answerText; 

    void Start()
    {
        isGameStart = false;
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string selectTheme() {
        themePanel.SetActive(true);
        // TODO: 문제 선택 메소드 구현
        
        Invoke("temp", 3f);

        return "사과";
    }

    void temp()
    {
        themePanel.SetActive(false);
    }


    public void OnPressedStartBtn()
    {
        pv.RPC("GameStart", RpcTarget.All);
        gameStartBtn.SetActive(false);
    }

    // 게임 시작 메소드
    [PunRPC] 
    void GameStart()
    {

        isGameStart = true;
        string answer = selectTheme();
        answerText.text = answer;

        identityPanel.SetActive(true);
    }
}
