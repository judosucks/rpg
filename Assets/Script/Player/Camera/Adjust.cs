
using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
// using Unity.VisualScripting; // Remove if not used
using UnityEngine;
using UnityEngine.Assertions; // Good for checks

public class Adjust : MonoBehaviour
{
    public NewCamera newCamera;

    // Private fields for smoothing
    protected float orthoSizeVelocity = 0f;
// Keep separate velocities for X and Y screen adjustments if needed often,
// otherwise, creating them locally in the method is fine.
    // private float screenXVelocity;
    // private float screenYVelocity;
    private void Awake()
    {
        
    }

    private void Start()
    {
        newCamera = CameraManager.instance?.newCamera;
    }

    // --- Core Logic ---

    private CinemachineCamera GetActiveCamera()
    {
        if (CameraManager.instance == null)
        {
            Debug.LogError("CameraManager.instance is null! Cannot get active camera.");
            return null;
        }
        return CameraManager.instance.GetCurrentActiveCamera();
    }

    private void SetCameraPriority(CinemachineCamera mainCam, CinemachineCamera secondaryCam)
    {
        // Add null checks for safety
        if (mainCam == null)
        {
            Debug.LogError("SetCameraPriority called with a null mainCam!");
            return;
        }

        // Debug.Log($"Setting priority: Main={mainCam.name}(20), Secondary={(secondaryCam != null ? secondaryCam.name : "None")}(10)");

        if (mainCam == secondaryCam)
        {
            // This might be okay if you just want to ensure the main one is high priority
            // Debug.LogWarning("SetCameraPriority: mainCam and secondaryCam are the same.");
             mainCam.Priority = 20; // Ensure it's high
            return;
        }

        mainCam.Priority = 20;
        if (secondaryCam != null)
        {
            secondaryCam.Priority = 10;
        }
    }

    public virtual void SmoothZoom(CinemachineCamera cam, float targetOrthoSize)
    {
        if (cam == null) return; // Safety check

        // Lens settings are structs, so get it, modify it, set it back
        LensSettings lens = cam.Lens;
        lens.OrthographicSize = Mathf.SmoothDamp(
            lens.OrthographicSize,
            targetOrthoSize,
            ref orthoSizeVelocity, // Use the class member velocity
            newCamera.zoomSpeed);
        cam.Lens = lens; // Assign the modified struct back
    }

    // --- Follow Methods ---

    private void FollowTarget(CinemachineCamera targetCamera, float targetOrthoSize, string debugName)
    {
        CinemachineCamera currentCam = GetActiveCamera();
        if (currentCam == null || targetCamera == null) return; // Exit if manager or target is invalid

        // Debug.Log($"Follow {debugName}. Current: {currentCam.name}, Target: {targetCamera.name}");
        if (currentCam != targetCamera)
        {
            SetCameraPriority(targetCamera, currentCam);
            SmoothZoom(targetCamera, targetOrthoSize);
        }
        else
        {
            // If already on the target camera, still apply zoom smoothly
             SmoothZoom(targetCamera, targetOrthoSize);
        }
    }

    public void FollowGrenade() => FollowTarget(newCamera.grenadeCamera, newCamera.grenadeOrthoSize, "Grenade");
    public virtual void FollowPlayer() => FollowTarget(newCamera.playerCamera, newCamera.playerOrthoSize, "Player");
    public virtual void FollowThunder() => FollowTarget(newCamera.thunderCamera, newCamera.thunderOrthoSize, "Thunder");
    public virtual void FollowBlackhole() => FollowTarget(newCamera.blackholeCamera, newCamera.blackholeOrthoSize, "Blackhole");
    public virtual void FollowGrenadeExplosion() => FollowTarget(newCamera.grenadeExplodeFxCamera, newCamera.grenadeExplodeOrthoSize, "GrenadeExplosionFX");


    // --- Screen Adjustment Methods ---

    public virtual void SetScreenX(float targetScreenX, float? smoothTime = null)
    {
        CinemachineCamera currentCam = GetActiveCamera();
        if (currentCam == null) return;
        SetScreenValue(currentCam, targetScreenX, smoothTime ?? newCamera.defaultScreenSmoothTime, true);
    }

     public virtual void SetScreenY(float targetScreenY, float? smoothTime = null)
    {
        CinemachineCamera currentCam = GetActiveCamera();
        if (currentCam == null) return;
        SetScreenValue(currentCam, targetScreenY, smoothTime ?? newCamera.defaultScreenSmoothTime, false);
    }
    private void SetScreenValue(CinemachineCamera cam, float targetValue, float smoothTime, bool isX)
    {
        if (cam.TryGetComponent<CinemachinePositionComposer>(out var composer))
        {
            // Get the current composition struct
            var composition = composer.Composition; // Use 'var' or the actual struct type if known (often internal)

            float currentVal = isX ? composition.ScreenPosition.x : composition.ScreenPosition.y;
            float velocity = 0f; // Use a local velocity for SmoothDamp here

            // Modify the struct's properties
            if (isX)
            {
                composition.ScreenPosition.x = Mathf.SmoothDamp(currentVal, targetValue, ref velocity, smoothTime);
            }
            else
            {
                composition.ScreenPosition.y = Mathf.SmoothDamp(currentVal, targetValue, ref velocity, smoothTime);
            }

            // Assign the modified struct back to the composer
            composer.Composition = composition;
        }
        else
        {
            Debug.LogWarning($"CinemachinePositionComposer not found on camera '{cam.name}'! Cannot set screen {(isX ? 'X' : 'Y')}.");
        }
    }

    public void AdjustCameraScreenX(float targetScreenX, float smoothTime)
    {
        if (newCamera != null)
        {
            SetScreenX(newCamera.temporaryScreenX,newCamera.smoothTime);
        }
    }

    // private void SetScreenValue(CinemachineCamera cam, float targetValue, float smoothTime, bool isX)
    // {
    //     if (cam.TryGetComponent<CinemachinePositionComposer>(out var composer))
    //     {
    //         // Composition settings are structs, handle similarly to LensSettings
    //         CompositionSettings composition = composer.Composition;
    //         float currentVal = isX ? composition.ScreenPosition.x : composition.ScreenPosition.y;
    //         float velocity = 0f; // Use a local velocity for SmoothDamp here
    //
    //         if (isX)
    //         {
    //             composition.ScreenPosition.x = Mathf.SmoothDamp(currentVal, targetValue, ref velocity, smoothTime);
    //         }
    //         else
    //         {
    //             composition.ScreenPosition.y = Mathf.SmoothDamp(currentVal, targetValue, ref velocity, smoothTime);
    //         }
    //         composer.Composition = composition; // Assign the modified struct back
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"CinemachinePositionComposer not found on camera '{cam.name}'! Cannot set screen {(isX ? 'X' : 'Y')}.");
    //     }
    // }


    // --- Shake Method ---

    public void ShakeCamera(float intensity, float duration)
    {
        CinemachineCamera currentCamera = GetActiveCamera();
        if (currentCamera == null)
        {
            Debug.LogError("No active camera found for ShakeCamera!");
            return;
        }

        // Try to get existing noise component
        CinemachineBasicMultiChannelPerlin noise = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        // Add noise component if it doesn't exist
        if (noise == null)
        {
            noise = currentCamera.AddComponent<CinemachineBasicMultiChannelPerlin>();
             Debug.Log($"Added CinemachineBasicMultiChannelPerlin to {currentCamera.name}");
        }

        if (newCamera.noiseProfile == null)
        {
             Debug.LogError("Noise Profile is not assigned in the Inspector for Adjust script!");
             return;
        }

        // Debug.Log($"Shaking camera {currentCamera.name} with intensity {intensity} for {duration}s");
        noise.NoiseProfile = newCamera.noiseProfile;
        noise.AmplitudeGain = intensity;        noise.FrequencyGain = 1f; // Or make this configurable

        // Stop any previous shake coroutine for this noise component to avoid conflicts
        StopAllCoroutines(); // Be careful if Adjust runs other coroutines
        StartCoroutine(StopShake(noise, duration));
    }

    private IEnumerator StopShake(CinemachineBasicMultiChannelPerlin noise, float duration)
    {
        
        yield return new WaitForSeconds(duration);

        CinemachineCamera potentiallyNewCurrentCamera = GetActiveCamera();

        // Check if the noise component still exists and if its VirtualCamera matches the potentially new current camera
        if (noise != null && potentiallyNewCurrentCamera != null && noise.VirtualCamera == potentiallyNewCurrentCamera) // <-- Use VirtualCamera here
        {
            // Debug.Log($"Stopping shake on {noise.VirtualCamera.name}"); // <-- Use VirtualCamera here too if logging name
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
        }
        else
        {
            // Debug.LogWarning("StopShake: Noise component no longer valid or camera changed.");
        }
    }


    
    
}
