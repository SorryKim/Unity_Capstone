using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using TMPro;

public class GameSystem : MonoBehaviourPunCallbacks
{
    public static GameSystem instance;
    public GameObject themePanel, waitPanel, liarPanel, noLiarPanel;
    public Button startBtn;
    public Text wordText;
    public List<Player> players;
    public List<Button> buttons = new List<Button>();
    // 게임 정답
    public string answer;


    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(players != null)
            players = GameManager.instance.players;
    }

    private void Start()
    {
        //players = GameManager.instance.players;
        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.gameObject.SetActive(true);
        }
    }
    

    void SelectLiar()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            int randomIdx = Random.Range(0, players.Count);
            players[randomIdx].IsLiar = true;
        }

    }

    // 게임 시작버튼 누를 경우
    public void OnPressedStart()
    {

        SelectLiar();
        // 모든플레이어들 게임시작
        photonView.RPC("GameStart", RpcTarget.All);
        
    }

    // 주제어 선택메소드
    // 구현부탁드립니다 채림씨
    public string selectWord()
    {
        
        return "사과";
    }

    public void OnClickWord()
    {

        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        string selectedTheme = clickObject.GetComponentInChildren<TMP_Text>().text.ToString();
        //Debug.Log("선택된 주제: " + selectedTheme);
        photonView.RPC("SelectComplete", RpcTarget.All);
        photonView.RPC("IdentifyWord", RpcTarget.All);

    }
    
    [PunRPC]
    public void GameStart()
    {
        // 방장인 경우 주제선택 페이지 뜸
        if (PhotonNetwork.IsMasterClient)
        {
            themePanel.SetActive(true);
            answer = selectWord();
        }
        // 방장이 아닌 경우
        else
        {
            waitPanel.SetActive(true);
        }
    }

    // 방장이 단어선택을 마친 경우
    [PunRPC]
    public void SelectComplete()
    {
        themePanel.SetActive(false);
        waitPanel.SetActive(false);
    }

    // 자신의 역할 및 단어 확인
    [PunRPC]
    public void IdentifyWord()
    {
        bool isLiar = PhotonNetwork.LocalPlayer.IsLiar;
        wordText.text = answer;
        if (PhotonNetwork.LocalPlayer.IsLiar) liarPanel.SetActive(true);
        else noLiarPanel.SetActive(true);

        // 10초후의 자동으로 꺼짐
        StartCoroutine(ExecuteAfterDelay());

    }

    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(10);

        liarPanel.SetActive(false);
        noLiarPanel.SetActive(false);
        
    }

   

}
