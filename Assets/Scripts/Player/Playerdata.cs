using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
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
    public RuntimeAnimatorController ghostAnimCon;
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

            if (isLive && pv.IsMine)
            {
                ActionRPC("Character", PhotonNetwork.LocalPlayer.ActorNumber);
            }
            else if (!isLive && pv.IsMine)
            {
                ActionRPC("GhostCharacter", null);
            }
        }
    }

    [PunRPC]
    void Character(int actorNumber)
    {
        int userIndex = (actorNumber - 1) % 8; //0~7, actornumber가 8이 되면 다시 0부터

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

    [PunRPC]
    void GhostCharacter()
    {
        anim.runtimeAnimatorController = ghostAnimCon;
    }
}
