using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class NewCamera : Adjust
{
    public float originalScreenY = 0f;
    public float originalScreenX = 0f;
    private float screenXVelocity = 0f;
    private float screenYVelocity = 0f;
    protected Player player;
    public CameraManager cameraManager;
    public GameObject blackholePrefab;
    public CinemachineCamera playerCamera;
    public CinemachineCamera grenadeCamera;
    public CinemachineCamera blackholeCamera;
    public CinemachineCamera thunderCamera;
    public CinemachineCamera grenadeExplodeFxCamera;
    public CinemachinePositionComposer positionComposer;
    [Header("camera info")]
    public NoiseSettings noiseProfile; // 设置一个 Noise Profile 的引用
    public CinemachineBasicMultiChannelPerlin noise;
    public float zoomSpeed = 10f;
    public float smoothTime = .01f; // 平滑过渡的时间
    public float temporaryScreenX; // 用于临时储存平滑过渡值
    
    public float targetScreenX;
    public float blackholeScreenY = 0.34f;
    
    public float currentVelocity;
    public float blackholeOrthoSize = 4f;
    public float defaultScreenSmoothTime = 0.1f;
    public float grenadeOrthoSize = 1f;
    public float playerOrthoSize = 2.28f;
    public float thunderOrthoSize = 2.28f;
    public float grenadeExplodeOrthoSize = 1f;
    public float targetOrthoSize;
    public CinemachineCamera activeCam;
    protected  void Awake()
    {
        
        Debug.Log("new camera awake");
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineCamera>();
        grenadeCamera = GameObject.FindGameObjectWithTag("GrenadeCamera").GetComponent<CinemachineCamera>();
        
        blackholeCamera = GameObject.FindGameObjectWithTag("BlackholeCamera").GetComponent<CinemachineCamera>();
        thunderCamera = GameObject.FindGameObjectWithTag("ThunderCamera").GetComponent<CinemachineCamera>();
        grenadeExplodeFxCamera = GameObject.FindGameObjectWithTag("GrenadeExplodeFxCamera")
            .GetComponent<CinemachineCamera>();
        cameraManager = CameraManager.instance;

    }

    protected void Start()
    {

        // Use null-conditional operators for safer access to singletons
        player = PlayerManager.instance?.player;


        if (player == null)
        {
            Debug.LogError(
                "Player object is null in Adjust.Start(). Ensure PlayerManager is initialized and assigns the player reference before Adjust starts, or check Script Execution Order.");
            // Depending on your game logic, you might want to disable this component or return early
            // enabled = false;
            // return;
        }

        if (newCamera == null)
        {
            Debug.LogError(
                "NewCamera reference from CameraManager is null in Adjust.Start(). Ensure CameraManager is initialized and assigns newCamera before Adjust starts, or check Script Execution Order.");
            // Depending on your game logic, you might want to disable this component or return early
            // enabled = false;
            // return;
        }
        else
        {
            Debug.Log("Adjust.Start(): newCamera reference obtained.");
             activeCam = cameraManager.GetCurrentActiveCamera();

            if (activeCam != null)
            {
                Debug.Log($"Adjust.Start(): Attempting to get PositionComposer from active camera: {activeCam.name}");
                targetOrthoSize = activeCam.Lens.OrthographicSize;
                // Use TryGetComponent for safety
                positionComposer = activeCam.GetComponent<CinemachinePositionComposer>();
                    if(positionComposer != null)
                {
                    // --- Success! ---
                    if(activeCam.name == "PlayerCamera")
                    { 
                        originalScreenX = positionComposer.Composition.ScreenPosition.x;
                        originalScreenY = positionComposer.Composition.ScreenPosition.y;
                        Debug.Log("originscreenXY"+originalScreenX+""+""+originalScreenY+"");
                    }
                    else
                    {
                        Debug.Log("not playercamera");
                    }
                    // Only assign original values AFTER confirming positionComposer is valid
                }
                else
                {
                    // --- Failure ---
                    // Composer component was not found on the active camera
                    positionComposer = null; // Ensure it's null if TryGetComponent fails
                    Debug.LogError(
                        $"Adjust.Start(): CinemachinePositionComposer component NOT found on the active camera '{activeCam.name}'! Cannot initialize original screen positions.");
                }


            }
        }
    }

    protected virtual void Update()
    {
        SmoothZoom(activeCam, targetOrthoSize);
    }
    public void ResetZoom()
            {
                // You need to get the CinemachinePositionComposer

                positionComposer = CameraManager.instance.GetCurrentActiveCamera()
                    .GetComponent<CinemachinePositionComposer>();
                if(positionComposer!= null)
                {
                    
                        Debug.LogWarning("reset zoom"+newCamera.name);
                        // positionComposer.Composition.ScreenPosition.x = Mathf.SmoothDamp(
                        //     positionComposer.Composition.ScreenPosition.x,
                        //     targetScreenX,
                        //     ref currentVelocity,
                        //     defaultScreenSmoothTime);
                        // positionComposer.Composition.ScreenPosition.y = Mathf.SmoothDamp(
                        //     positionComposer.Composition.ScreenPosition.y,
                        //     originalScreenY,
                        //     ref currentVelocity,
                        //     defaultScreenSmoothTime);
                            positionComposer.Composition.ScreenPosition.x = originalScreenX;
                            positionComposer.Composition.ScreenPosition.y = originalScreenY;
                            SmoothZoom(CameraManager.instance.GetCurrentActiveCamera(),
                                CameraManager.instance.GetCurrentActiveCamera().Lens.OrthographicSize);
                        

                        Debug.Log("reset zoom");
                    }
                else
                {
                    Debug.LogWarning("position composer is null");
                }

                }
                


            
        }