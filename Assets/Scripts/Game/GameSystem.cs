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
using System.IO.Pipes;
using System;

public class GameSystem : MonoBehaviourPunCallbacks
{
    public static GameSystem instance;
    public GameComment gameComment;
    public GameManager gameManager;
    public GameObject themePanel, waitPanel, liarPanel, noLiarPanel, userListPanel, chatPanel, checkPanel, loadingPanel;
    public Button startBtn;
    public Text word;
    public Player[] players;
    public TMP_Text roleCheckText;
    public int commentStartIdx;
    public int liarIdx;


    private bool isLiar = false;
# region 게임 정답 관련 변수
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

#endregion

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
        gameComment = GetComponent<GameComment>();
        gameManager = GetComponent<GameManager>();
        isLiar = false;
    }


    #region 방장이 게임시작 버튼 누르면

    // 게임 시작버튼 누를 경우
    public void OnPressedStart()
    {
        // 모든플레이어 스타트
        photonView.RPC("GameStart", RpcTarget.All);
    }

    [PunRPC]
    public void GameStart()
    {
        // 로딩창 띄움
        userListPanel.SetActive(false);
        chatPanel.SetActive(false);
        loadingPanel.SetActive(true);
        
        // 기본 설정 시작
        StartCoroutine(StartSetting());

        // 설정이 끝난 후 단어선택 창 띄움
        if (PhotonNetwork.IsMasterClient)
            themePanel.SetActive(true);
        else
            waitPanel.SetActive(true);
    }

    // 기본설정
    IEnumerator StartSetting()
    {

        // 방장일 경우만 진행
        if (PhotonNetwork.IsMasterClient)
        {
            players = PhotonNetwork.PlayerList;

            // 코멘트 첫 시작 순서
            commentStartIdx = UnityEngine.Random.Range(0, players.Length);

            // 라이어 인덱스
            liarIdx = UnityEngine.Random.Range(0, players.Length);

           
            for (int i = 0; i < players.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
                if(i == liarIdx)
                    customProperties.Add("IsLiar", true); // 변경할 속성 추가
                else
                    customProperties.Add("IsLiar", false); // 변경할 속성 추가

                // SetCustomProperties 메서드를 사용하여 커스텀 속성을 설정
                players[i].SetCustomProperties(customProperties);
            }

          
            

            // 위에서 설정된 정보를 모두와 동기화
            photonView.RPC("SendSetting", RpcTarget.All, players, commentStartIdx, liarIdx);
        }
        
        // 5초 대기
        yield return new WaitForSeconds(5f);
        loadingPanel.SetActive(false);
    }

    // 변수 동기화
    [PunRPC]
    public void SendSetting(Player[] list, int commentStartIdx, int liarIdx)
    {
        players = list;
        this.commentStartIdx = commentStartIdx;
        this.liarIdx = liarIdx;
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
                int randomIdx = UnityEngine.Random.Range(0, theme.word.Count);
                temp = theme.word[randomIdx];
            }
        }
        return temp;
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
        StartCoroutine(ExecuteAfterDelay());
    }

    // 제시어 확인 텍스트
    public void SetCheckUI(bool isLiar)
    {
        if (isLiar) roleCheckText.text = "당신은 라이어";
        else roleCheckText.text = "제시어: " + answer;
    }

    // 10초동안 확인하는 코루틴
    IEnumerator ExecuteAfterDelay()
    {
        bool isLiar = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsLiar"))
        {
            isLiar = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsLiar"];
        }

        if (isLiar)
        {
            liarPanel.SetActive(true);
            noLiarPanel.SetActive(false);
        }
        else
        {
            liarPanel.SetActive(false);
            noLiarPanel.SetActive(true);
        }

        foreach(var player in PhotonNetwork.PlayerList)
        {
            Debug.Log("닉네임: " + player.NickName + " 라이어여부: " + player.CustomProperties["IsLiar"].ToString());
        }
        yield return new WaitForSeconds(10);

        liarPanel.SetActive(false);
        noLiarPanel.SetActive(false);
        checkPanel.SetActive(true);
        userListPanel.SetActive(true);
        chatPanel.SetActive(true);
        SetCheckUI(isLiar);

        // 코멘트시작!
        gameComment.StartComment();
    }
    #endregion

}
