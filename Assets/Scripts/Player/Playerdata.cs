using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Playerdata : MonoBehaviour
{
    private PhotonView pv;
    Animator anim;
    public RuntimeAnimatorController[] animCon;
    public TMP_Text nickname;
    private bool isLive;

    void ActionRPC(string functionName, object value)
    {
        pv.RPC(functionName, RpcTarget.AllBufferedViaServer, value);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        isLive = true;
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        if (pv.IsMine)
        {
            ActionRPC("Character", PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsLive", out object isLiveValue))
        {
            isLive = (bool)isLiveValue;

            // 해당 플레이어의 PhotonView를 통해 현재 플레이어인지 확인 후 설정
            if (pv.IsMine)
            {
                anim.SetBool("Dead", !isLive);
            }
        }
    }

    [PunRPC]
    void Character(int actorNumber)
    {
        int userIndex = (actorNumber - 1) % 8; // 0~7, actornumber가 8이 되면 다시 0부터

        if (userIndex >= 0 && userIndex < animCon.Length)
        {
            // 캐릭터 할당
            RuntimeAnimatorController selectedController = animCon[userIndex];
            anim.runtimeAnimatorController = selectedController;
        }
        else
        {
            Debug.LogError("Invalid user index or character index.");
        }
    }
}
