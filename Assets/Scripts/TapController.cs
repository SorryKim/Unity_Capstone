using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapController : MonoBehaviour
{

    private bool isTab;
    private bool isEsc;
    public GameObject tabUI, optionUI;

    // Start is called before the first frame update
    void Start()
    {
        isTab = false;
        isEsc = false;
    }
    // 설정
    public void OnClickSettingOn()
    {
        isEsc = !isEsc;
    }

   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) isTab = !isTab;
        if (Input.GetKeyDown(KeyCode.Escape)) isEsc = !isEsc;

        if (isTab)
            tabUI.SetActive(true);
        else
            tabUI.SetActive(false);

        if (isEsc)
            optionUI.SetActive(true);
        else
            optionUI.SetActive(false);


    }
}
