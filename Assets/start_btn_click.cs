using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_btn_click : MonoBehaviour
{
    public AudioSource startclick;

    public void ClickSound()
    {
        startclick.Play();
    }
}
