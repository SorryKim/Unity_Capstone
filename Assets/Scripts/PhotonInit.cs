using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public enum ActivePanel
    {
        LOGIN = 0,
        ROOMS = 1
    }

    public ActivePanel activePanel = ActivePanel.LOGIN;

    public string gameVersion = "1.0";
    public string userId = "kim";
    public byte maxPlayer = 20;

    public InputField txtUserId;
    public InputField txtRoomName;

    public GameObject[] panels;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        // 유저아이디 작성안하면 랜덤 적용
        txtUserId.text = PlayerPrefs.GetString("USER_ID", "USER_" + Random.Range(1, 100));
        txtRoomName.text = PlayerPrefs.GetString("ROOM_NAME", "ROOM_" + Random.Range(1, 100));
    }

    #region SELF_CALLBACK_FUNCTIONS
    public void OnLogin()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.NickName = txtUserId.text;

        PhotonNetwork.ConnectUsingSettings();

        PlayerPrefs.SetString("User_ID", PhotonNetwork.NickName);
        ChangePanel(ActivePanel.ROOMS);
    }

    public void OnCreateRoomClick()
    {
        PhotonNetwork.CreateRoom(txtRoomName.text, new RoomOptions{ MaxPlayers = this.maxPlayer });
    }

    public void OnJoinRandomRoomClick()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    private void ChangePanel(ActivePanel panel)
    {
        foreach(GameObject _panel in panels)
        {
            Debug.Log(panels);
            _panel.SetActive(false);
        }
        panels[(int)panel].SetActive(true);
    }

    #region PHOTON_CALLBACK_FUNCTIONS
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayer });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("Main");
    }
    #endregion
}
