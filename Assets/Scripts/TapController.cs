using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{

    private bool isTab = false;
    public GameObject tabUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) isTab = !isTab;


        if (isTab)
            tabUI.SetActive(true);
        else
            tabUI.SetActive(false); 
        
    }
}
