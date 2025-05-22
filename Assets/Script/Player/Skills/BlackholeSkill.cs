using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BlackholeSkill : Skill
{
    [SerializeField] private UISkillTreeSlot blackholeUnlockButton;
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    public bool unlockBlackhole { get; private set; }
    public float blackholeDuration;
    FireOrbitController fireOrbitController;
    BlackholeSkillController currentBlackhole;

    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
        {
            unlockBlackhole = true;
        }
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForSkillTreeSlotInitialization());
    }

    private IEnumerator WaitForSkillTreeSlotInitialization()
    {
        while (!isSkillTreeSlotInitialized)
        {
            yield return null; // Wait for one frame
        }
        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
        CheckUnlocked();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        
        GameObject newBlackhole = Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);
        
        currentBlackhole = newBlackhole.GetComponent <BlackholeSkillController>();
        
        fireOrbitController = newBlackhole.GetComponent<FireOrbitController>();
        
        currentBlackhole.SetupBlackhole(maxSize,growSpeed,shrinkSpeed,amountOfAttacks,cloneCooldown,blackholeDuration);
        // 啟動火焰公轉效果（只需初始化一次）
        fireOrbitController.Initialize(currentBlackhole.transform);
        // 啟用火焰
        fireOrbitController.SetFireObjectsActive(true);
           
        
    }

    public bool BlackholeSkillCompleted()
    {
        if (!currentBlackhole||currentBlackhole == null)
        {
            
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            
            currentBlackhole = null;
            return true;
        }
        
        return false;
    }

    protected override void CheckUnlocked()
    {
        Debug.LogWarning("check unlocked blackhole");
        UnlockBlackhole();
    }
}
