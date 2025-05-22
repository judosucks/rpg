//
// using UnityEngine;
// using UnityEngine.InputSystem;
// using System.Collections.Generic;
// using Random = UnityEngine.Random;
//
// public class BlackholeSkillController : MonoBehaviour
// {
//    [SerializeField] private GameObject hotKeyPrefab;
//    [SerializeField] private List<Key> keyCodeList;//input system
//    
//    private float maxSize;
//    private float growSpeed;
//    private float shrinkSpeed;
//    private float blackholeTimer;
//    private bool canGrow = true;
//    private bool canShrink;
//    private bool canCreateHotKeys = true;
//    private bool cloneAttackReleased;
//    private bool playerCanDisapear = true;
//    private int amountOfAttacks = 4;
//    private float cloneAttackCooldown = .3f;
//    private float cloneAttackTimer;
//    [SerializeField] private float rotationSpeed = 100f;
//    private bool shouldRotate = false;
//    private List<Transform> targets = new List<Transform>();
//    private List<GameObject> createdHotKey = new List<GameObject>();
//    private FireOrbitController fireOrbitController;// 火焰的公轉控制腳本
//    public bool playerCanExitState { get; private set; } = false;
//    private float elapsedTime = 0f;
//    private float blackholeDuration;
//    private bool isDestroyed = false; // 添加销毁标志
//    private void Awake()
//    {
//       fireOrbitController = GetComponent<FireOrbitController>();
//       if (fireOrbitController == null)
//       {
//          Debug.LogError("FireOrbitController script is missing! Please attach it to the GameObject.");
//       }
//    }
//
//    public void StartRotation()
//    {
//       shouldRotate = true;
//    }
//    public void StopRotation()
//    {
//       shouldRotate = false;
//    }
//    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
//    {
//       Debug.Log("setup blackhole");
//       maxSize = _maxSize;
//       growSpeed = _growSpeed;
//       shrinkSpeed = Mathf.Clamp(_shrinkSpeed, 0.1f, 10f); // Avoid invalid speeds
//       amountOfAttacks = _amountOfAttacks;
//       cloneAttackCooldown = _cloneAttackCooldown;
//       blackholeTimer = _blackholeDuration;
//       
//    }
//    private void Update()
//    {
//       if (isDestroyed)
//       {
//          return; // 若标记为已销毁，Update 不再执行任何逻辑
//       }
//       blackholeTimer -= Time.deltaTime;
//       cloneAttackTimer -= Time.deltaTime;
//       elapsedTime += Time.deltaTime;
//
//       // Update playerCanExitState when duration ends
//       if (elapsedTime >= blackholeTimer && !playerCanExitState)
//       {
//          playerCanExitState = true;
//          Debug.Log("Blackhole duration complete. Player can exit state.");
//       }
//       if (shouldRotate)
//       {
//          transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
//       }
//       if (blackholeTimer < 0)
//       {
//          blackholeTimer = Mathf.Infinity;
//
//          if (targets.Count > 0 && !isDestroyed)
//          {
//             //檢查銷毀狀態
//             ReleaseCloneAttack();
//          }
//          else
//          {
//             Debug.LogWarning("Calling FinishBlackholeAbility due to timer expiration or no targets.");
//             FinishBlackholeAbility();// This updates playerCanExitState and shrinks the blackhole
//          }
//       }
//       
//       if (Keyboard.current.rKey.wasPressedThisFrame)
//       {
//          Debug.LogWarning("r key pressed release clone");
//          ReleaseCloneAttack();
//       }
//
//       CloneAttackLogic();
//       if (canGrow && !canShrink)
//       {
//          Debug.Log("Growing " + amountOfAttacks + " attacks");
//       transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growSpeed);
//          StartRotation();
//         
//          
//       }
//
//       if (canShrink)
//       {
//          Debug.Log($"Shrinking logic running. Current Scale: {transform.localScale}, shrinkSpeed: {shrinkSpeed}");
//          
//          
//          
//          // Reduce the scale step by step towards the target
//          float newX = Mathf.MoveTowards(transform.localScale.x, -1, shrinkSpeed * Time.deltaTime);
//          float newY = Mathf.MoveTowards(transform.localScale.y, -1, shrinkSpeed * Time.deltaTime);
//          transform.localScale = new Vector2(newX, newY);
//          if (transform == null)
//          {
//             Debug.LogWarning("transform is null no shrink");
//          }
//          else
//          {
//             transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * shrinkSpeed);
//             Debug.Log("transform is not null"+transform.localScale.x+" "+transform.localScale.y+" "+shrinkSpeed*Time.deltaTime);
//          }
//          if (shrinkSpeed <= 0)
//          {
//             Debug.LogError("Shrink speed must be greater than 0!");
//             canShrink = false;
//             return;
//          }
//          if (transform.localScale.x <= 0)
//          {
//             
//             Debug.LogWarning("Shrinking complete. Destroying blackhole.");
//             StopRotation();
//             Destroy(gameObject);
//             playerCanExitState = true;
//             canShrink = false;
//          }
//          
//
//       }else
//       {
//          Debug.LogWarning("canShrink is false. Shrinking logic not running.");
//       }
//       if (elapsedTime >= (blackholeTimer * 2))
//       {
//          Debug.LogWarning("Blackhole effect timeout! Forcing state exit.");
//          playerCanExitState = true;
//          if (!canShrink) // Ensure shrinking starts
//          {
//             Debug.LogWarning("Forcing shrink on timeout.");
//             FinishBlackholeAbility(); // Safe fallback to trigger shrink
//          }
//       }
//       Debug.Log($"Update: blackholeTimer = {blackholeTimer}, canShrink = {canShrink}, elapsedTime = {elapsedTime}");
//    }
//
//    private void ReleaseCloneAttack()
//    {
//       if (targets.Count <= 0)
//       {
//          Debug.Log("no targets");
//          FinishBlackholeAbility();
//          return;
//       }
//       Debug.Log("release clone");
//       DestroyHotKey();
//       cloneAttackReleased = true;
//       canCreateHotKeys = false;
//       if (playerCanDisapear)
//       {
//         playerCanDisapear = false;
//         PlayerManager.instance.player.MakeTransparent(true);
//       }
//       Debug.Log($"ReleaseCloneAttack called. Targets.Count: {targets.Count}, isDestroyed: {isDestroyed}");
//    }
//
//    private void CloneAttackLogic()
//    {
//       Debug.Log("cloneattack logic");
//       if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
//       {
//          cloneAttackTimer = cloneAttackCooldown;
//          if (targets.Count > 0)
//          {
//             
//          int randomIndex = Random.Range(0, targets.Count);
//
//          float xOffset;
//
//          if (Random.Range(0, 100) > 50)
//          {
//             xOffset = 1;
//          }
//          else
//          {
//             xOffset = -1;
//          }
//          SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex],new Vector3(xOffset,0));
//
//          amountOfAttacks--;
//          }
//          
//          if (amountOfAttacks <= 0)
//          {
//             Debug.Log("All attacks completed. Finalizing blackhole.");
//             FinishBlackholeAbility();
//          }
//            
//          
//          
//       }
//    }
//
//    private void FinishBlackholeAbility()
//    {
//       
//       canShrink = true;
//       playerCanExitState = true;
//       
//       DestroyHotKey();
//       isDestroyed = true; // 标记为已销毁
//       cloneAttackReleased = false;
//       if (transform != null)
//       { 
//          Debug.Log("transform is not null from finishblackhole");
//          transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * shrinkSpeed);
//          if (transform.localScale.x <= 0)
//          {
//             
//             Debug.LogWarning("transform Shrinking complete. Destroying blackhole.");
//             StopRotation();
//             Destroy(gameObject);
//             playerCanExitState = true;
//             canShrink = false;
//          }
//       }
//    }
//
//    private void DestroyHotKey()
//    {
//       Debug.Log("destroy hotkey");
//       if (createdHotKey.Count <= 0)
//       {
//          return;
//       }
//
//       for (int i = 0; i < createdHotKey.Count; i++)
//       {
//          Destroy(createdHotKey[i]);
//       }
//    }
//
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//       
//       if (other.GetComponent<Enemy>() != null)
//       {
//          Debug.Log("number of targets"+targets.Count);
//          other.GetComponent<Enemy>().FreezeTime(true);
//          AddEnemyToList(other.transform);
//          CreateHotkey(other);
//         
//       }
//       
//    }
//
//    private void OnTriggerExit2D(Collider2D other)
//    {
//       if (isDestroyed) // 确保黑洞已被标记为销毁
//       {
//          Debug.LogWarning("OnTriggerExit2D called after object destruction.");
//          other.GetComponent<Enemy>()?.FreezeTime(false);
//          return;
//       }
//       Debug.Log("exit trigger");
//       
//    }
//       
//
//    private void CreateHotkey(Collider2D other)
//    {
//       Debug.Log("create hotkey");
//       if (keyCodeList.Count <= 0)
//       {
//          Debug.Log("no more keys");
//          return;
//       }
//
//       if (!canCreateHotKeys)
//       {
//          Debug.Log("no hotkey");
//          return;
//       }
//       GameObject newHotKey =
//          Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
//       createdHotKey.Add(newHotKey);
//       
//       Key choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
//       keyCodeList.Remove(choosenKey);
//       BlackholeHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackholeHotkeyController>();
//       newHotkeyScript.SetHotKey(choosenKey,other.transform,this);
//    }
//
//    public void AddEnemyToList(Transform _enemyTransforms) => targets.Add(_enemyTransforms);
// }
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{
   [SerializeField] private GameObject hotKeyPrefab;
   [SerializeField] private List<Key> keyCodeList;//input system
   
   private float maxSize;
   private float growSpeed;
   private float shrinkSpeed;
   private float blackholeTimer;
   private bool canGrow = true;
   private bool canShrink;
   private bool canCreateHotKeys = true;
   private bool cloneAttackReleased;
   private bool playerCanDisapear = true;
   private int amountOfAttacks = 4;
   private float cloneAttackCooldown = .3f;
   private float cloneAttackTimer;
   [SerializeField] private float rotationSpeed = 100f;
   private bool shouldRotate = false;
   private List<Transform> targets = new List<Transform>();
   private List<GameObject> createdHotKey = new List<GameObject>();
   private FireOrbitController fireOrbitController;// 火焰的公轉控制腳本
   private EntityFX entityFX;
   public bool playerCanExitState { get; private set;}

   private void Awake()
   {
      fireOrbitController = GetComponent<FireOrbitController>();
      if (fireOrbitController == null)
      {
         Debug.LogError("FireOrbitController script is missing! Please attach it to the GameObject.");
      }
      entityFX = PlayerManager.instance.player.GetComponent<EntityFX>();
      transform.position = PlayerManager.instance.player.transform.position;
      Debug.Log("trasform name" + transform.name);
   }

   public void StartRotation()
   {
      shouldRotate = true;
   }
   public void StopRotation()
   {
      shouldRotate = false;
   }
   public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
   {
      
      maxSize = _maxSize;
      growSpeed = _growSpeed;
      shrinkSpeed = _shrinkSpeed;
      amountOfAttacks = _amountOfAttacks;
      cloneAttackCooldown = _cloneAttackCooldown;
      blackholeTimer = _blackholeDuration;
      
   }
   private void Update()
   {
      blackholeTimer -= Time.deltaTime;
      cloneAttackTimer -= Time.deltaTime;
      if (shouldRotate)
      {
         transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
      }
      if (blackholeTimer < 0)
      {
         blackholeTimer = Mathf.Infinity;

         if (targets.Count > 0)
         {
            ReleaseCloneAttack();
         }
         else
         {
            FinishBlackholeAbility();
         }
      }
      
      if (Keyboard.current.rKey.wasPressedThisFrame)
      {
         Debug.Log("r key pressed release clone");
         ReleaseCloneAttack();
         entityFX.ForceDestroyLightning();
         FinishBlackholeAbility();
      }

      CloneAttackLogic();
      if (canGrow && !canShrink)
      {
        
      transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growSpeed);
         StartRotation();
         
         
        
         
      }

      if (canShrink)
      {
         Debug.Log("Shrinking");
         if (transform != null)
         {
            Debug.Log("transform not null");
         transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * shrinkSpeed);
         if (transform.localScale.x < 0)
         {
            Debug.Log("destroy blackhole");
            StopRotation();
            Destroy(gameObject);
            
            
         }
         }

      }
      
   }

   private void ReleaseCloneAttack()
   {
      if (targets.Count <= 0)
      {
        
         return;
      }
      
      DestroyHotKey();
      cloneAttackReleased = true;
      canCreateHotKeys = false;
      if (playerCanDisapear)
      {
        playerCanDisapear = false;
        PlayerManager.instance.player.entityFX.MakeTransparent(true);
      }
   }

   private void CloneAttackLogic()
   {
      Debug.Log("cloneattack logic");
      if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
      {
         cloneAttackTimer = cloneAttackCooldown;
         
         int randomIndex = Random.Range(0, targets.Count);

         float xOffset;

         if (Random.Range(0, 100) > 50)
         {
            xOffset = .5f;
         }
         else
         {
            xOffset = -.5f;
         }
         SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex],new Vector3(xOffset,0));

         amountOfAttacks--;
         
         if (amountOfAttacks <= 0)
         {
            
            Invoke("FinishBlackholeAbility",.5f);;
         }
           
         
         
      }
   }

   private void FinishBlackholeAbility()
   {
      
      Debug.Log("finish blackhole ability");
      DestroyHotKey();
      playerCanExitState = true;
      canShrink = true;
      
      cloneAttackReleased = false;
   }

   private void DestroyHotKey()
   {
     
      if (createdHotKey.Count <= 0)
      {
         return;
      }

      for (int i = 0; i < createdHotKey.Count; i++)
      {
         Destroy(createdHotKey[i]);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      
      if (other.GetComponent<Enemy>() != null)
      {
         
         other.GetComponent<Enemy>().FreezeTime(true);
        
         CreateHotkey(other);
        
      }
      
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      other.GetComponent<Enemy>()?.FreezeTime(false);
      
   }
      

   private void CreateHotkey(Collider2D other)
   {
      
      if (keyCodeList.Count <= 0)
      {
        
         return;
      }

      if (!canCreateHotKeys)
      {
         
         return;
      }
      GameObject newHotKey =
         Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
      createdHotKey.Add(newHotKey);
      
      Key choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
      keyCodeList.Remove(choosenKey);
      BlackholeHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackholeHotkeyController>();
      newHotkeyScript.SetHotKey(choosenKey,other.transform,this);
   }

   public void AddEnemyToList(Transform _enemyTransforms) => targets.Add(_enemyTransforms);
}