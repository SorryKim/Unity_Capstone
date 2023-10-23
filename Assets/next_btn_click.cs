using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class next_btn_click : MonoBehaviour
{
    public AudioSource nextclick;

    public void ClickSound()
    {
        nextclick.Play();
    }
}
