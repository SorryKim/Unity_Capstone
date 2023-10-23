using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    
    void Start()
    {

        // �ڽ��� ���� �÷��̾��� ���
        if (photonView.IsMine)
        {

            // �ش� ���� �����ϴ� �ó׸ӽ� ���� ī�޶� ã��
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();

            // ���� ī�޶��� ���� ����� �ڽ��� ��ġ�� ����
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
