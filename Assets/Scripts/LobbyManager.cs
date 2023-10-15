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

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1"; // 게임버젼

    int playerCnt = 4; // 플레이어 수 설정
    string nickname = "", roomname = "";
    public GameObject startUI, nicknamePanel, createPanel, createRoomPanel;
    public TMP_InputField nicknameInput, roomNameInput;
    public TMP_Text playerCntText;


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

    // CreatePanel에서 방만들기 버튼을 누르는 경우
    public void ClickBackButtonCreatePanel()
    {
        createPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }

    public void ClickAddNum()
    {
        playerCnt++;
        playerCntText.text = playerCnt.ToString();
    }

    public void ClickMinusNum()
    {
        playerCnt--;
        playerCntText.text = playerCnt.ToString();
    }

    // CreateRoomPanel에서 next 버튼을 누르는 경우
    public void ClickNextButtonCreateRoomPanel()
    {
        roomname = roomNameInput.text;
        Debug.Log(roomname);
        SceneManager.LoadScene("Main");
    }

    // CreateRoomPanel에서 back버튼을 누르는 경우
    public void ClickBackButtonCreateRoomPanel()
    {

        createRoomPanel.SetActive(false);
        createPanel.SetActive(true);

    }

}
