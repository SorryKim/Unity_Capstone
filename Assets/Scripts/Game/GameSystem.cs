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
using System.Security.Cryptography.X509Certificates;

public class GameSystem : MonoBehaviourPunCallbacks
{
    public static GameSystem instance;
    public GameComment gameComment;
    public GameManager gameManager;
    public GameObject themePanel, waitPanel, liarPanel, noLiarPanel, userListPanel, chatPanel, checkPanel, loadingPanel, settingPanel;
    public Button startBtn, settingBtn;
    public Text word;
    public TMP_Text roleCheckText, themeText1, themeText2;
    public int commentStartIdx;
    public int liarIdx;
    public RawImage playerimage;
    public Sprite[] playerImages;
    public RawImage liarimage;
    public Sprite[] liarImages;

    public Player[] players;




    #region 게임 정답 관련 변수
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

        Room room = PhotonNetwork.CurrentRoom;

        Debug.Log("최대점수: " + room.CustomProperties["MaxScore"]);
        
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
        gameManager.RoomRenewal();
        // 로딩창 띄움
        userListPanel.SetActive(false);
        chatPanel.SetActive(false);
        loadingPanel.SetActive(true);
        settingBtn.gameObject.SetActive(false);
        checkPanel.SetActive(false);
        // 기본 설정 시작
        StartCoroutine(StartSetting());

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 모든 플레이어에 대해 이동 명령 전송
        foreach (GameObject player in players)
        {
            // 이동하고자 하는 위치 설정 (예: (0, 0, 0))
            Vector3 targetPosition = new Vector3(0, 0, 0);

            // 플레이어의 Transform 컴포넌트를 가져오기
            Transform playerTransform = player.transform;

            // 플레이어의 위치를 설정된 목표 위치로 이동
            playerTransform.position = targetPosition;
        }


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
                customProperties.Add("VoteCount", 0); // 투표에서 사용할 변수
                customProperties.Add("IsVote", false); // 투표에서 사용할 변수
                customProperties.Add("IsLive", true); // 생존 여부
                if (i == liarIdx)
                    customProperties.Add("IsLiar", true); // 변경할 속성 추가         
                else
                    customProperties.Add("IsLiar", false); // 변경할 속성 추가
              
                // SetCustomProperties 메서드를 사용하여 커스텀 속성을 설정
                players[i].SetCustomProperties(customProperties);
            }

        }     
        // 5초 대기
        yield return new WaitForSeconds(5f);
        loadingPanel.SetActive(false);
    }


    

    // 주제어를 선택한 경우
    public void OnClickWord()
    {
        // 현재 클릭된 버튼
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        selectedTheme = clickObject.GetComponentInChildren<TMP_Text>().text.ToString();
        string ans = parseJson();
        // 정답단어를 모두에게 전달
        photonView.RPC("SetAnswer", RpcTarget.All, ans, selectedTheme);
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
    void SetAnswer(string answer, string selectedTheme)
    {
        this.answer = answer;
        this.selectedTheme = selectedTheme;
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
    public void SetCheckUI()
    {
        Player player = PhotonNetwork.LocalPlayer;
        bool isLiar = (bool)player.CustomProperties["IsLiar"];
        bool isLive = (bool)player.CustomProperties["IsLive"];

        if (!isLive) {
            roleCheckText.text = "당신은 관전 상태입니다...";
        }
        else {
            if (isLiar) roleCheckText.text = "당신은 라이어";
            else roleCheckText.text = "제시어: " + answer;
        }
        
    }


    // 10초동안 확인하는 코루틴
    IEnumerator ExecuteAfterDelay()
    {
        int userIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 8; 
        bool isLiar = false;
        word.text = answer;
        themeText1.text = "주제: " + selectedTheme;
        themeText2.text = "주제: " + selectedTheme;
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsLiar"))
        {
            isLiar = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsLiar"];
        }

        yield return new WaitForSeconds(2f);
        if (isLiar)
        {
            liarPanel.SetActive(true);
            noLiarPanel.SetActive(false);
            liarimage.texture = SpriteToTexture(liarImages[userIndex]);
        }
        else
        {
            liarPanel.SetActive(false);
            noLiarPanel.SetActive(true);
            playerimage.texture = SpriteToTexture(playerImages[userIndex]);
        }

        yield return new WaitForSeconds(10);

        liarPanel.SetActive(false);
        noLiarPanel.SetActive(false);
        checkPanel.SetActive(true);
        userListPanel.SetActive(true);
        chatPanel.SetActive(true);
        settingBtn.gameObject.SetActive(true);
        SetCheckUI();

        // 코멘트시작!
        gameComment.CommentStart();
    }

   

    #endregion

    public Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y,
            (int)sprite.rect.width, (int)sprite.rect.height));
        texture.Apply();
        return texture;
    }
    // Sprite를 RawImage로 변환하여 반환하는 함수
    public RawImage ConvertSpriteToRawImage(Sprite sprite)
    {
        // RawImage에 적용할 Texture2D 생성
        Texture2D rawTexture = SpriteToTexture(sprite);

        // RawImage 생성 및 Texture2D 적용
        RawImage rawImage = new GameObject("RawImage").AddComponent<RawImage>();
        rawImage.texture = rawTexture;

        return rawImage;
    }

   
}
