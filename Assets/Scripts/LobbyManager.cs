using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1"; // ���ӹ���

    public Text connectionInfoText; // ��Ʈ��ũ ���� �ؽ�Ʈ
    public Button joinButton; // ���� ��ư

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    public void Start()
    {
        // ���ӿ� �ʿ��� ���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        // ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        connectionInfoText.text = "Master Server�� ���� �õ���...";
    }

    // ������ ���� ���� ���� �� ����
    public override void OnConnectedToMaster()
    {
        // �� ���� ��ư Ȱ��ȭ
        joinButton.interactable = true;

        // ���� �ؽ�Ʈ �ȳ�
        connectionInfoText.text = "Online: Master Server�� �����";
    }

    // ������ ��� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        // �� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = false;
        // ���� �õ��� �ؽ�Ʈ �ȳ�
        connectionInfoText.text = "Offline: Master Sever�� ���� �õ���....";

        // ������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� �� ����
    public void Connect()
    {
        // �ߺ� ���� ����, ��ư ��Ȱ��ȭ
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "�뿡 ���� �õ�";
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            
            // ������ ������ ���ӽõ�
            connectionInfoText.text = "Offline: Master Sever�� ���� �õ���....";
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // ���� �� ���� ������ ��� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���� ǥ��
        connectionInfoText.text = "No room. ���ο� �� ����";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "�� ���� ����!";
        PhotonNetwork.LoadLevel("Main");
    }
}
