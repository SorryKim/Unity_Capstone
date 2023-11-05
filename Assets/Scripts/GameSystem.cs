using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using LitJson;
using Newtonsoft.Json;




public class GameSystem : MonoBehaviourPunCallbacks
{
    public static GameSystem instance;
    public GameManager gameManager;
    public GameObject themePanel, waitPanel, liarPanel, noLiarPanel, userListPanel, chatPanel;
    public Button startBtn;
    public Text word;
    public Player[] players;

    // 게임 정답 관련 변수
    public string answer;
    public string selectedTheme;
    public TextAsset jsonData;
    [System.Serializable]
    public class WordData
    {
        public string name;
        public List<string> word;
    }
    [System.Serializable]
    public class ThemeData
    {
        public List<WordData> theme;
    }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
  
    }

    private void Start()
    {
        //players = GameManager.instance.players;
        if (PhotonNetwork.IsMasterClient)
        {
            startBtn.gameObject.SetActive(true);
        }
        gameManager = GetComponent<GameManager>();
        
    }


    #region 방장이 게임시작 버튼 누르면

    // 게임 시작버튼 누를 경우
    public void OnPressedStart()
    {
        
        // 모든플레이어들 게임시작
        photonView.RPC("GameStart", RpcTarget.All);
        
    }


    // 라이어 설정
    void SelectLiar()
    {
        players = PhotonNetwork.PlayerList;
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            int randomIdx = Random.Range(0, players.Length);
            players[randomIdx].IsLiar = true;
        }

    }

    [PunRPC]
    public void GameStart()
    {
        // 라이어 설정 후
        SelectLiar();

        userListPanel.SetActive(false);
        chatPanel.SetActive(false);

        // 방장인 경우 주제선택 페이지 뜸
        if (PhotonNetwork.IsMasterClient)
        {
            themePanel.SetActive(true);
        }
        // 방장이 아닌 경우
        else
        {
            waitPanel.SetActive(true);
        }
    }


    // 해당 주제에 맞는 단어를 JSON에서 가져오기
    public string parseJson()
    {
        ThemeData themeData = JsonConvert.DeserializeObject<ThemeData>(jsonData.text);
        string temp = "";
        // 파싱한 데이터 사용 예시
        foreach (WordData theme in themeData.theme)
        {
            if (theme.name == selectedTheme)
            {
                int randomIdx = Random.Range(0, theme.word.Count);
                temp = theme.word[randomIdx];
            }

        }
        return temp;

    }

    // 주제어를 선택한 경우
    public void OnClickWord()
    {

        // 현재 클릭된 버튼
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        selectedTheme = clickObject.GetComponentInChildren<TMP_Text>().text.ToString();
        string s = parseJson();
        // 정답단어를 모두에게 전달
        photonView.RPC("SetAnswer", RpcTarget.AllBuffered, s);
        // 주제패널 or 주제대기 패널 비활성화
        photonView.RPC("SelectComplete", RpcTarget.All);
        // 단어 확인 패널 10초동안 확인
        photonView.RPC("IdentifyWord", RpcTarget.All);
       
       
    }

    // 정답단어를 모두에게 전달
    [PunRPC]
    void SetAnswer(string str)
    {
        this.answer = str;
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
        word.text = answer;
        if (isLiar) liarPanel.SetActive(true);
        else noLiarPanel.SetActive(true);

        // 10초후의 자동으로 꺼짐
        StartCoroutine(ExecuteAfterDelay());

    }

    // 10초동안 확인하는 코루틴
    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(10);

        liarPanel.SetActive(false);
        noLiarPanel.SetActive(false);

        GameComment.instance.StartComment();
    }

    

    #endregion

}
