using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1"; // 게임버젼

    int playerCnt = 4; // 플레이어 수 설정
    int score = 1; // 점수 설정
    
    string nickname = "", roomname = "";
    public GameObject startUI, nicknamePanel, createPanel, createRoomPanel, roomListPanel, manualPanel;
    public TMP_InputField nicknameInput, roomNameInput;
    public TMP_Text playerCntText, scoreText;



    // 룸 목록을 저장할 딕셔너리
    public Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // 룸을 표시할 프리팹
    public GameObject roomPrefab;
    // Room 프리팹이 차일드로 될 부모
    public Transform scrollContent;

    private void Awake()
    {
        // 방장이 씬 로딩하면, 나머지 인원 싱크
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임버젼 지정
        PhotonNetwork.GameVersion = gameVersion;

        

    }


    private void Start()
    {
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("포톤 서버에 접속");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("02. 로비에 접속");
    }

    // CreateRoomPanel에서 next 버튼을 누르는 경우
    public void ClickNextButtonCreateRoomPanel()
    {
        roomname = roomNameInput.text;
        int maxScore = int.Parse(scoreText.text);
        RoomOptions roomOption = new RoomOptions();
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;
        roomOption.MaxPlayers = playerCnt;
        roomOption.CustomRoomProperties = new Hashtable();
        roomOption.CustomRoomProperties.Add("MaxScore", maxScore);
        PhotonNetwork.CreateRoom(roomname, roomOption);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장완료");
        PhotonNetwork.LoadLevel("Main");

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 5;

        PhotonNetwork.CreateRoom("room1", ro);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (var room in roomList)
        {
            // 방이 삭제된 경우
            if (room.RemovedFromList)
            {

                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            // 룸 정보가 갱신된 경우
            else
            {
                // 룸이 처음 생성된 경우
                if (!roomDict.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().roomInfo = room;
                    roomDict.Add(room.Name, _room);

                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().roomInfo = room;
                }
            }
        }
    }



    // CreateRoomPanel에서 back버튼을 누르는 경우
    public void ClickBackButtonCreateRoomPanel()
    {

        createRoomPanel.SetActive(false);
        createPanel.SetActive(true);

    }

    // StartPanel에서 start버튼을 누르는 경우
    public void ClickStartButton()
    {
        
        startUI.SetActive(false);
        nicknamePanel.SetActive(true);
    }

    // NicknamePanel에서 next 버튼을 누르는 경우
    public void ClickNextButtonNickname()
    {
        nickname = nicknameInput.text;
        PhotonNetwork.NickName = nickname;
        PlayerPrefs.SetString("nickname", nickname);
        Debug.Log(nickname);

        nicknamePanel.SetActive(false);
        createPanel.SetActive(true);
    }

    // NicknamePanel에서 back 버튼을 누르는 경우
    public void ClickBackButtonNickname()
    {
        nicknamePanel.SetActive(false);
        startUI.SetActive(true);
    }

    // CreatePanel에서 방만들기 버튼을 누르는 경우
    public void ClickCreateButtonCreatePanel()
    {
        createPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    // CreatePanel에서 방찾기버튼을 누르는 경우
    public void ClickSearchButtonCreatePanel()
    {
        createPanel.SetActive(false);
        roomListPanel.SetActive(true);
    }

    // CreatePanel에서 뒤로가기 버튼을 누르는 경우
    public void ClickBackButtonCreatePanel()
    {
        createPanel.SetActive(false);
        nicknamePanel.SetActive(true);
    }

    // RoomListPanel에서 뒤로가기 버튼을 누르는 경우
    public void ClickBackButtonRoomListPanel()
    {
        roomListPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void ClickManualButtonCreatePanel()
    {
        createPanel.SetActive(false);
        manualPanel.SetActive(true);
    }

    public void CLickBackBUttonManualPanel()
    {
        manualPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void ClickAddNum()
    {
        if (playerCnt < 8)
        {
            playerCnt++;
            playerCntText.text = playerCnt.ToString();
        }
    }

    public void ClickMinusNum()
    {
        if (playerCnt > 4)
        {
            playerCnt--;
            playerCntText.text = playerCnt.ToString();
        }
    }

    public void ClickAddScore()
    {
        if (score < 5)
        {
            score++;
            scoreText.text = score.ToString();
        }
    }

    public void ClickMinusScore()
    {
        if (score > 1)
        {
            score--;
            scoreText.text = score.ToString();
        }
    }

    

}