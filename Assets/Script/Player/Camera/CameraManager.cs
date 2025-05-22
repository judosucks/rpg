using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public NewCamera newCamera;
    private CinemachineBrain brain;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        if (newCamera == null)
        {
            Debug.Log("newcamera is null");
            newCamera = GameObject.FindWithTag("Camera").GetComponent<NewCamera>();
            Debug.Log("newcamera"+newCamera);
        }else if(newCamera != null)
        {
            Debug.Log("newcamera is not null");
        }

        brain = newCamera.GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            Debug.LogError("CinemachineBrain is null");
        }
    }

    private void Start()
    {
        EnsureSingleAudioListener();
    }

    private void EnsureSingleAudioListener()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        if (listeners.Length > 1)
        {
            Debug.LogWarning("muitiple audilisternersd found destroying");
            for (int i = 0; i < listeners.Length; i++)
            {
                Destroy(listeners[i].gameObject);
            }
        }
    }
    // public void AdjustGrenadeCameraScreenX(float targetScreenX, float smoothTime)
    // {
    //     if (newCamera != null)
    //     {
    //         newCamera.SetScreenX(newCamera.grenadeCamera, targetScreenX, smoothTime);
    //     }
    // }

    // public void AdjustPlayerCameraScreenX(float targetScreenX, float smoothTime)
    // {
    //     if (newCamera != null)
    //     {
    //         
    //         newCamera.SetScreenX(newCamera.playerCamera, targetScreenX, smoothTime);
    //     }
    // }

    public CinemachineCamera GetCurrentActiveCamera()
    {
        if (brain != null && brain.ActiveVirtualCamera is CinemachineCamera activeVirtualCamera)
        {
            Debug.Log("active camera found");
            return activeVirtualCamera;
        }
        Debug.LogWarning("No active camera found"+brain);
        return null; // no active camera
    }
    // Method to shake the camera
   
    
}
