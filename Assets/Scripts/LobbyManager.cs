using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1"; // 게임버젼

    public Text connectionInfoText; // 네트워크 정보 텍스트
    public Button joinButton; // 참여 버튼

    // 게임 실행과 동시에 마스터 서버 접속 시도
    public void Start()
    {
        // 접속에 필요한 게임 버젼 설정
        PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        // 접속 버튼 비활성화
        joinButton.interactable = false;
        connectionInfoText.text = "Master Server에 접속 시도중...";
    }

    // 마스터 서버 접속 성공 시 실행
    public override void OnConnectedToMaster()
    {
        // 룸 접속 버튼 활성화
        joinButton.interactable = true;

        // 성공 텍스트 안내
        connectionInfoText.text = "Online: Master Server와 연결됨";
    }

    // 실패할 경우 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 룸 접속 버튼 비활성화
        joinButton.interactable = false;
        // 접속 시도중 텍스트 안내
        connectionInfoText.text = "Offline: Master Sever와 접속 시도중....";

        // 마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 게임 룸 접속
    public void Connect()
    {
        // 중복 접속 방지, 버튼 비활성화
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "룸에 접속 시도";
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            
            // 마스터 서버에 접속시도
            connectionInfoText.text = "Offline: Master Sever와 접속 시도중....";
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // 랜덤 룸 참가 실패한 경우 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 상태 표시
        connectionInfoText.text = "No room. 새로운 방 생성";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "방 참가 성공!";
        PhotonNetwork.LoadLevel("Main");
    }
}
