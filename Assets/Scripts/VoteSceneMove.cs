using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoteSceneMove : MonoBehaviour
{
    public void VoteSceneCtrl()
    {
        SceneManager.LoadScene("Vote");
        Debug.Log("Vote Scene");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
