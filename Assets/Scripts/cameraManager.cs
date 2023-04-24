using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] allCameras;
    public CinemachineVirtualCamera movementCamera;
    public CinemachineVirtualCamera fallingCamera;
    public CinemachineVirtualCamera ViewUpCamera;
    public CinemachineVirtualCamera ViewDownCamera;

    public CinemachineVirtualCamera startCamera;
    private CinemachineVirtualCamera currentCamera;


    private void Start()
    {
        currentCamera = startCamera;

        //changes priority to make it the currentCamera
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i] == currentCamera)
            {
                allCameras[i].Priority = 20;
            }
            else
            {
                allCameras[i].Priority = 10;
            }
        }
    }

    //new camera = camera that we wanna switch to
    public void SwitchCamera(CinemachineVirtualCamera newCamera)
    {
        currentCamera = newCamera;

        currentCamera.Priority = 20;
        for (int i = 0; i < allCameras.Length; i++)
        {
            if (allCameras[i] != currentCamera)
            {
                allCameras[i].Priority = 10;
            }
        }
    }

    private void Update()
    {
    }
}
