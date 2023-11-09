using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;

public class GameComment : MonoBehaviourPunCallbacks
{
    GameSystem gameSystem;
    GameManager gameManager;
    public static GameComment instance;

    // 발표 순서
    public GameObject commentWaitPanel, commentPanel;
    public TMP_InputField commentInput;
    public TMP_Text[] comments;

    // 발표 여부
    private bool isCommentAllowed = false;
    private bool isEnd = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameSystem = GetComponent<GameSystem>();
        gameManager = GetComponent<GameManager>();
    }

    #region 코멘트 발표

    // 발표 시작
    public void StartComment()
    {
        
    }

    IEnumerator CommentingTimer()
    {
        for (int i = 0; i < gameSystem.commentSequence.Length; i++)
        {

            int idx = gameSystem.commentSequence[i];
            gameSystem.players[idx].IsCommenting = true;

            if (PhotonNetwork.LocalPlayer.IsCommenting)
            {
                photonView.RPC("StartCommenting", RpcTarget.All, idx);
                yield return new WaitForSeconds(30f); // 30초 동안 코멘트 허용
                photonView.RPC("EndCommenting", RpcTarget.All);
            }
            else
            {
                commentWaitPanel.SetActive(true);
            }

            gameSystem.players[idx].IsCommenting = false;
            yield return new WaitForSeconds(1f);

        }
    }

    [PunRPC]
    public void StartCommenting(int idx)
    {
        commentPanel.SetActive(true);
        Send(idx);
    }

    [PunRPC]
    public void EndCommenting()
    {
        commentPanel.SetActive(false);
        commentWaitPanel.SetActive(false);
    }



    #endregion

    #region 코멘트 작성
    public void Send(int idx)
    {
        photonView.RPC("SendComment", RpcTarget.All, (PhotonNetwork.LocalPlayer.NickName + " : " + commentInput.text), idx);
    }

    [PunRPC]
    public void SendComment(string msg, int idx)
    {
        comments[idx].text = msg;
    }
    #endregion
}