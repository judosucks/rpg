
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using Yushan.Enums;

public class GrenadeSkill : Skill
{
    public GrenadeType grenadeType = GrenadeType.Frag;
    private Mouse mouse;
    [SerializeField] private UISkillTreeSlot grenadeSkillUnlockButton;
    
    [Header("skill info")] [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField] private float grenadeGravity;
    [SerializeField] private Vector2 launchForce;

    private Vector2 finalDirection;

    [Header("Aim dots")] [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;
    [SerializeField] private Transform handPoint;

    [Header("grende info")] [SerializeField]
    private GameObject spawnedGrenade; // 用于记录已生成的手榴弹

    [SerializeField] private float handPointOffsetY; //spwan grenade offset y
    public float explosionTimer => ExplosionTimer; //read-only
    [SerializeField] private float ExplosionTimer; //editable in inspector
    [SerializeField] private UISkillTreeSlot timeStopUnlockButton;
    [SerializeField] private UISkillTreeSlot volnurableUnlockButton;
    [SerializeField] private UISkillTreeSlot fragGrenadeUnlockButton;
    [SerializeField] private UISkillTreeSlot flashGrenadeUnlockButton;
    [SerializeField] private UISkillTreeSlot smokeGrenadeUnlockButton;
    [SerializeField] private UISkillTreeSlot incendiaryGrenadeUnlockButton;
    public bool grenadeUnlocked { get; private set; }
    public bool timeStopUnlocked { get; private set; }
    public bool volnurableUnlocked { get; private set; }

    private void Awake()
    {
       
    }

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
        
        mouse = Mouse.current;
        StartCoroutine(WaitForSkillTreeSlotInitialization());
    }

    private IEnumerator WaitForSkillTreeSlotInitialization()
    {
        while (!isSkillTreeSlotInitialized)
        {
            yield return null; // Wait for one frame
        }

        grenadeSkillUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockGrende);
        // timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        // volnurableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVolnurable);
        // fragGrenadeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockFragGrenade);
        // flashGrenadeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockFlashGrenade);
        // smokeGrenadeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSmokeGrenade);
        // incendiaryGrenadeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockIncendiaryGrenade);
        CheckUnlocked();
    }
    protected override void CheckUnlocked()
    {
        UnlockGrende();
        // UnlockTimeStop();
        // UnlockVolnurable();
        // UnlockFragGrenade();
        // UnlockFlashGrenade();
        // UnlockSmokeGrenade();
        // UnlockIncendiaryGrenade();
    }
    protected override void Update()
    {
        base.Update();
        if (mouse.rightButton.isPressed && playerData.mouseButttonIsInUse)
        {
            Debug.Log("right from grenade skill");

            // Aiming logic
            if (playerData.isAiming && !playerData.grenadeCanceled)
            {
                Debug.Log("player is aiming with grenade not canceled");

                // Cancel grenade if left button is pressed
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    // Block further input and cancel grenade state
                    ResetGrenadeState();
                    Debug.Log("left button pressed, canceling grenade");

                    // Cancel grenade logic
                    player.CancelThrowGrenade();

                    DestroyGrenadeSpwawn(); // Destroy the spawned grenade

                    player.anim.SetBool("AimGrenade", false);
                    player.anim.SetTrigger("AimAbort"); // Transition to abort state

                    return;
                }

                SpawnGrenade();
            }
        }

// Right button released logic
        if (playerData.isAimCheckDecided && mouse.rightButton.wasReleasedThisFrame)
        {
            Debug.Log("right button released");
            ResetGrenadeState();
            DestroyGrenadeSpwawn();

            // Cleanup if grenade was canceled and right button was released
            if (playerData.grenadeCanceled && !playerData.isAiming)
            {
                // Ensure animator transitions cleanly
                player.anim.SetBool("AimGrenade", false);
                player.anim.SetTrigger("AimAbort"); // Transition to abort state

                playerData.isAiming = false;
                playerData.isAimCheckDecided = false;

                return;
            }

            // Throw grenade logic
            player.anim.SetTrigger("ThrowGrenade");
            playerData.isAiming = false;
            playerData.isAimCheckDecided = false;

            // Calculate final throw direction and launch grenade
            finalDirection = new Vector2(
                AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y
            );
            return;
        }

        if (playerData.isAiming)
        {
            
            UpdateDotsPosition();
        }
        else
        {
           
            DotsActive(false);
        }

        if (spawnedGrenade != null)
        {
            spawnedGrenade.transform.position = handPoint.position;
        }

    }
#region Unlock

private void UnlockGrende()
{
    if (grenadeSkillUnlockButton.unlocked)
    {
        grenadeUnlocked = true;
    }
}
     private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
        {
            timeStopUnlocked = true;
        }
    }

    private void UnlockVolnurable()
    {
        if (volnurableUnlockButton.unlocked)
        {
            volnurableUnlocked = true;
        }
    }

    private void UnlockFlashGrenade()
    {
        if (flashGrenadeUnlockButton.unlocked)
        {
            grenadeType = GrenadeType.Flash;
        }
    }
    private void UnlockSmokeGrenade()
    {
        if (smokeGrenadeUnlockButton.unlocked)
        {
            grenadeType = GrenadeType.Smoke;
        }
    }

    private void UnlockIncendiaryGrenade()
    {
        if (incendiaryGrenadeUnlockButton.unlocked)
        {
            grenadeType = GrenadeType.Incendiary;
        }
    }
     private void UnlockFragGrenade()
    {
        if (fragGrenadeUnlockButton.unlocked)
        {
            grenadeType = GrenadeType.Frag;
            grenadeUnlocked = true;
        }
    }
    #endregion
   

private void UpdateDotsPosition()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Debug.LogWarning("update dots position");
            Vector2 handPoint2D = handPoint.position;
            dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }
    public void ResetGrenadeState()
    {
        playerData.mouseButttonIsInUse = false;
        playerData.rightButtonLocked = false;
    }
    private void SpawnGrenade()
    {
        if (spawnedGrenade != null)
        {
            return;
        }
        spawnedGrenade = Instantiate(grenadePrefab,new Vector2(handPoint.position.x,handPointOffsetY),grenadePrefab.transform.rotation);
        spawnedGrenade.transform.SetParent(handPoint);
    }

    private void DestroyGrenadeSpwawn()
    {
        if (spawnedGrenade != null)
        {
            spawnedGrenade.transform.parent = null;
            Destroy(spawnedGrenade);
        }
    }
   
    public void CreateGrenade()
    {
        if (player.grenade != null)
        {
            return;
        }

        Debug.Log($"[CreateGrenade] Creating a {grenadeType} grenade...");
        
        GameObject newGrenade = Instantiate(grenadePrefab, handPoint.position, handPoint.rotation);
        // 确保实例化的手榴弹处于激活状态
        if (!newGrenade.activeSelf)
        {
            newGrenade.SetActive(true);
        }
        GrenadeSkillController newGrenadeScript = newGrenade.GetComponent<GrenadeSkillController>();
        
        if (newGrenade == null)
        {
            Debug.LogError("failed to create grenade");
        }
        
        Debug.Log($"grenade created: {newGrenade.name}");

        switch (grenadeType)
        {
            case GrenadeType.Frag:
                Debug.Log("[CreateGrenade] Applying Frag Grenade setup");
                newGrenadeScript.SetupFragGrenade(true);
                break;

            case GrenadeType.Flash:
                Debug.Log("[CreateGrenade] Applying Flash Grenade setup");
                newGrenadeScript.SetupFlashGrenade(true);
                break;

            default:
                Debug.LogError("[CreateGrenade] Unknown grenade type. No setup applied.");
                break;
        }
        newGrenadeScript.SetupGrenade(finalDirection, grenadeGravity,player);
        player.AssignNewGrenade(newGrenade);
        DotsActive(false);
        
    }

    private void SetupGravity()
    {
        Debug.Log("SetupGravity");
    }

    private void GenerateDots()
    { 
        
        dots = new GameObject[numberOfDots];
        
        for (int i = 0; i < numberOfDots; i++)
        {
            
           dots[i] = Instantiate(dotPrefab, handPoint.position, Quaternion.identity, dotsParent);
           dots[i].SetActive(false);
        }
    }
   

    public void DotsActive(bool _isActive)
    {
       
        for (int i = 0; i < dots.Length; i++)
        {
            
            dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
       Vector3 position = (Vector2)handPoint.position + //initial position
                           AimDirection().normalized * launchForce * t    //initial velocity*t
                           + 0.5f * Physics2D.gravity * grenadeGravity * (t * t); // 1/2(gravity*t^2)
       
        return position;
    }
    

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();
       
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        if (Camera.main == null)
        {
            Debug.Log("Camera.main is null");
        }
        Vector2 direction = mouseWorldPosition - playerPosition;

        return direction;
    }
    
}