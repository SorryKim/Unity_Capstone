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

    private string gameVersion = "1"; // ���ӹ���

    int playerCnt = 4; // �÷��̾� �� ����
    string nickname = "", roomname = "";
    public GameObject startUI, nicknamePanel, createPanel, createRoomPanel;
    public TMP_InputField nicknameInput, roomNameInput;
    public TMP_Text playerCntText;


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

    // CreatePanel���� �游��� ��ư�� ������ ���
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

    // CreateRoomPanel���� next ��ư�� ������ ���
    public void ClickNextButtonCreateRoomPanel()
    {
        roomname = roomNameInput.text;
        Debug.Log(roomname);
        SceneManager.LoadScene("Main");
    }

    // CreateRoomPanel���� back��ư�� ������ ���
    public void ClickBackButtonCreateRoomPanel()
    {

        createRoomPanel.SetActive(false);
        createPanel.SetActive(true);

    }

}
