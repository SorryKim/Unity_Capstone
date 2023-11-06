using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    private Text roomInfoText;
    private RoomInfo _roomInfo;


    public RoomInfo roomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            GetComponent<Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
       
    }

    private void Awake()
    {
        roomInfoText = GetComponentInChildren<Text>();
        
    }

    private void Start()
    {
        roomInfoText.text = $"{_roomInfo.Name } ( {_roomInfo.PlayerCount} / {_roomInfo.MaxPlayers} )";
    }
    private void Update()
    {
       
    }

    public void OnEnterRoom(string roomName)
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 8;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}
