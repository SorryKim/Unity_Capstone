using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dino_color : MonoBehaviour
{
    private PhotonView pv;
    public RawImage[] images;
    public RawImage mydino;
    // Start is called before the first frame update
    void Start()
    {
        int actornumber = PhotonNetwork.LocalPlayer.ActorNumber;
        My_dino(actornumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void My_dino(int actorNumber)
    {
        int userIndex = (actorNumber - 1) % 8; //0~7, actornumber가 8이 되면 다시 0부터

        mydino = images[userIndex];
    }
}
