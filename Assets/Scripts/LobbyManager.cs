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

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1"; // ���ӹ���

    int playerCnt = 4; // �÷��̾� �� ����
    string nickname = "", roomname = "";
    public GameObject startUI, nicknamePanel, createPanel, createRoomPanel, roomListPanel, manualPanel;
    public TMP_InputField nicknameInput, roomNameInput;
    public TMP_Text playerCntText;

    // �� ����� ������ ��ųʸ�
    public Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // ���� ǥ���� ������
    public GameObject roomPrefab;
    // Room �������� ���ϵ�� �� �θ�
    public Transform scrollContent;

    private void Awake()
    {
        // ������ �� �ε��ϸ�, ������ �ο� ��ũ
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���ӹ��� ����
        PhotonNetwork.GameVersion = gameVersion;

        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }


    private void Start()
    {
       
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ������ ����");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("02. �κ� ����");
    }

    // CreateRoomPanel���� next ��ư�� ������ ���
    public void ClickNextButtonCreateRoomPanel()
    {

        roomname = roomNameInput.text;
        RoomOptions roomOption = new RoomOptions();
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;
        roomOption.MaxPlayers = playerCnt;

        PhotonNetwork.CreateRoom(roomname, roomOption);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ����Ϸ�");
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
            // ���� ������ ���
            if (room.RemovedFromList)
            {
                
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            // �� ������ ���ŵ� ���
            else
            {
                // ���� ó�� ������ ���
                if (!roomDict.ContainsKey(room.Name))
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().roomInfo = room;
                    roomDict.Add(room.Name, _room);
                    Debug.Log("�� ����Ʈ");
                    Debug.Log("���̸�: " + room.Name);
                    Debug.Log("�� �ο�: " + room.PlayerCount);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().roomInfo = room; 
                }
            }
        }
    }

   

    // CreateRoomPanel���� back��ư�� ������ ���
    public void ClickBackButtonCreateRoomPanel()
    {

        createRoomPanel.SetActive(false);
        createPanel.SetActive(true);

    }

    // StartPanel���� start��ư�� ������ ���
    public void ClickStartButton()
    {
        startUI.SetActive(false);
        nicknamePanel.SetActive(true);
    }

    // NicknamePanel���� next ��ư�� ������ ���
    public void ClickNextButtonNickname()
    {
        nickname = nicknameInput.text;
        PhotonNetwork.NickName = nickname;
        PlayerPrefs.SetString("nickname", nickname);
        Debug.Log(nickname);
       
        nicknamePanel.SetActive(false);
        createPanel.SetActive(true);
    }

    // NicknamePanel���� back ��ư�� ������ ���
    public void ClickBackButtonNickname()
    {
        nicknamePanel.SetActive(false);
        startUI.SetActive(true);
    }

    // CreatePanel���� �游��� ��ư�� ������ ���
    public void ClickCreateButtonCreatePanel()
    {
        createPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    // CreatePanel���� ��ã���ư�� ������ ���
    public void ClickSearchButtonCreatePanel()
    {
        createPanel.SetActive(false);
        roomListPanel.SetActive(true);
    }

    // ManualPanel���� �ڷΰ��� ��ư�� ������ ���
    public void ClickManualButtonCreatePanel()
    {
        createPanel.SetActive(false);
        manualPanel.SetActive(true);
    }

    // CreatePanel���� �ڷΰ��� ��ư�� ������ ���
    public void ClickBackButtonCreatePanel()
    {
        createPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    // RoomListPanel���� �ڷΰ��� ��ư�� ������ ���
    public void ClickBackButtonRoomListPanel()
    {
        roomListPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    // ManualPanel���� �ڷΰ��� ��ư�� ������ ���
    public void ClickBackButtonManualPanel()
    {
        manualPanel.SetActive(false);
        createPanel.SetActive(true);
    }

    public void ClickAddNum()
    {
        if (playerCnt < 10)
        {
            playerCnt++;
            playerCntText.text = playerCnt.ToString();
        }
    }

    public void ClickMinusNum()
    {
        if (playerCnt >= 2)
        {
            playerCnt--;
            playerCntText.text = playerCnt.ToString();
        }
    }

    

}
