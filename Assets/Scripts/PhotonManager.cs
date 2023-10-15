using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    private readonly string gameVersion = "1.0";
    private string userId = "kim";

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
        Debug.Log("0. ���� �Ŵ��� ����");
        PhotonNetwork.NickName = userId;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("1. ���� ���� ����");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("2. ���� �� ���� ����");

        // �� ����
        RoomOptions roomOption = new RoomOptions();
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;
        roomOption.MaxPlayers = 10;
        
        // �� ���� �� �ڵ� ����
        PhotonNetwork.CreateRoom("room1", roomOption);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("3. �� ���� �Ϸ�");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("4. �� ���� �Ϸ�");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }
}
