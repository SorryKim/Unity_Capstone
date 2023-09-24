using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    
    void Start()
    {

        // 자신이 로컬 플레이어인 경우
        if (photonView.IsMine)
        {

            // 해당 씬에 존재하는 시네머신 가상 카메라 찾기
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

            // 가상 카메라의 추적 대상을 자신의 위치로 변경
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
