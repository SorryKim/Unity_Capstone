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
    public GameObject themePanel, waitPanel, liarPanel, noLiarPanel;
    public Button startBtn;
    public Text word;
    public List<Player> players = new List<Player>();
    public List<Button> buttons = new List<Button>();
    // 게임 정답
    public string answer;
    public string selectedTheme;
    //채리미가 한거
    [SerializeField] Text nameText;
    [SerializeField] Text wordText;
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

    [System.Serializable]
    public class RootData
    {
        public ThemeData[] data;
    }

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

        parseJson();

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


    public void OnClickWord()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        selectedTheme = clickObject.GetComponentInChildren<TMP_Text>().text.ToString();
        answer = parseJson();
        photonView.RPC("SelectComplete", RpcTarget.All);
        photonView.RPC("IdentifyWord", RpcTarget.All);
        // 채리미
       
    }

   

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
    
    [PunRPC]
    public void GameStart()
    {
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
