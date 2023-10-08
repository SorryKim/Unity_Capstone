using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject m_Content;
    public InputField m_inputField;

    Text playerCount;
    PhotonView photonview;

    GameObject m_ContentText;

    string m_strUserName;
    void Start()
    {
        CreatePlayer();
        PhotonNetwork.IsMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings();
        m_ContentText = m_Content.transform.GetChild(0).gameObject;
        photonview = GetComponent<PhotonView>();
        //Invoke("CheckPlayerCount", 0.5f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && !m_inputField.isFocused)
        {
            m_inputField.ActivateInputField();
        }
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;

        int n_key = Random.Range(0, 100);

        m_strUserName = "user" + n_key;

        PhotonNetwork.LocalPlayer.NickName = m_strUserName;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
    }
    public void OnEndEditEvent()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string strMessage = m_strUserName + " : " + m_inputField.text;

            photonView.RPC("RPC_Chat", RpcTarget.All, strMessage);
            m_inputField.text = "";
        }
    }
    public void AddChatMessage(string msg)
    {
        GameObject goText = Instantiate(m_ContentText, m_Content.transform);

        goText.GetComponent<Text>().text = msg;
        m_Content.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public override void OnJoinedRoom()
    {
        AddChatMessage(PhotonNetwork.LocalPlayer.NickName + "¥‘ ¿‘¿Â!");
    }

    [PunRPC]
    void RPC_Chat(string msg)
    {
        AddChatMessage(msg);
    }

 
   
}
