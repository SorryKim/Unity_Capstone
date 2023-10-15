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

        // 방장이 씬 로딩하면, 나머지 인원 싱크
        PhotonNetwork.AutomaticallySyncScene = true;

        // 게임버젼 지정
        PhotonNetwork.GameVersion = gameVersion;

        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        Debug.Log("0. 포톤 매니저 시작");
        PhotonNetwork.NickName = userId;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("1. 포톤 서버 접속");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("2. 랜덤 룸 접속 실패");

        // 방 설정
        RoomOptions roomOption = new RoomOptions();
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;
        roomOption.MaxPlayers = 10;
        
        // 룸 생성 후 자동 입장
        PhotonNetwork.CreateRoom("room1", roomOption);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("3. 방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("4. 방 입장 완료");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }
}
