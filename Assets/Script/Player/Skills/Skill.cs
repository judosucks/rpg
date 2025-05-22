using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;
    protected Player player;
    
    protected SkillManager skillManager;
    protected bool isSkillInitialized= false;
    public static bool isSkillTreeSlotInitialized = false;
    [SerializeField]protected PlayerData playerData;

    private void Awake()
    {
        skillManager = SkillManager.instance;
        
    }

    protected virtual void Start()
    {
        StartCoroutine(WaitForPlayerManagerInitialization());
        
        if (skillManager == null)
        {
            skillManager = SkillManager.instance;
            if (skillManager == null)
            {
                Debug.LogError("SkillManager not found");
                return;
            }
        }
        
        StartCoroutine(WaitForSkillTreeSlotInitialization());
        // CheckUnlocked();
        
    }
    private IEnumerator WaitForSkillTreeSlotInitialization()
    {
        skillManager = SkillManager.instance;
        if(skillManager == null)
        {
            Debug.LogError("SkillManager not found");
            yield break;
        }
        while (!UISkillTreeSlot.IsInitialized)
        {
            yield return null; // Wait for one frame
        }
        
        isSkillTreeSlotInitialized = true;

        Debug.LogWarning("Skill: UISkillTreeSlot initialization complete, proceeding...");
        // CheckUnlocked();
        
            if (skillManager == null)
            {
                skillManager = SkillManager.instance;
                if (skillManager == null)
                {
                    Debug.LogError("SkillManager not found");

                }
            }
        
            
        
        
    }
    private IEnumerator WaitForPlayerManagerInitialization()
    {
        while (PlayerManager.instance == null)
        {
            yield return null; // Wait for one frame
        }
        
        player = PlayerManager.instance.player;
        if (player == null)
        {
            Debug.LogError("Player not found");
            yield break;
        }
        else
        {
            Debug.LogWarning("player found");
        }
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
        
    }
    protected virtual void FixedUpdate()
    {
        
    }
    protected virtual void CheckUnlocked()
    {
        Debug.Log("check unlocked from skill");
    }
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            Debug.Log("useskill from skill");
            //use skill
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("skill is on cooldown");
        return false;
    }
    public virtual void UseSkill()
    {
        //do some skill stuff
        Debug.Log("skill used");
    }
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, playerData.closestEnemyCheckRadius,playerData.whatIsEnemy);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
                
            }
        }

        return closestEnemy;
    }
}
