using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
public class CloneSkill : Skill
{
  [Header("clone attack")] 
  [SerializeField] private UISkillTreeSlot cloneAttackUnlockButton;
  [SerializeField] private float cloneAttackMultiplier;
  [SerializeField] private bool canAttack;
  [Header("aggresive clone attack")]
  [SerializeField] private UISkillTreeSlot aggresiveCloneUnlockButton;
  [SerializeField] private float aggresiveCloneAttackMultiplier;
  public bool canApplyOnHitEffect { get;private set; }
  
  [Header("clone multiple")]
  [SerializeField] private UISkillTreeSlot cloneMultipleUnlockButton;
  [SerializeField] private float multiCloneAttackMultiplier;
  [SerializeField] private bool canDuplicateClone;
  [SerializeField] private float chanceToDuplicate;

  [Header("clone info")] 
  [SerializeField] private GameObject clonePrefab;
  [SerializeField] private float cloneDuration; 
  [SerializeField] private float attackMultiplier;

  [SerializeField] private bool createCloneOnDashStart;
  [SerializeField] private bool createCloneOnDashOver;
  [SerializeField] private bool canCreateCloneOnCounterAttack;

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
    cloneMultipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
    aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
    cloneMultipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
    CheckUnlocked();
  }

  #region unlock region

  private void UnlockCloneAttack()
  {
    if (cloneAttackUnlockButton.unlocked)
    {
      canAttack = true;
      attackMultiplier = cloneAttackMultiplier;
    }
  }

  private void UnlockAggresiveClone()
  {
    if (aggresiveCloneUnlockButton.unlocked)
    {
      canApplyOnHitEffect = true;
      attackMultiplier = aggresiveCloneAttackMultiplier;
    }
  }

  private void UnlockMultiClone()
  {
    if (cloneMultipleUnlockButton.unlocked)
    {
      canDuplicateClone = true;
      attackMultiplier = multiCloneAttackMultiplier;
    }
  }

  #endregion
  
  public void CreateClone(Transform _clonePosition, Vector3 _offset)
  {
    GameObject newClone = Instantiate(clonePrefab);
    newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset,FindClosestEnemy(newClone.transform),
      canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
  }

  

  public void CreateCloneOnCounterAttack(Transform _enemyTransform)
  {
    if (canCreateCloneOnCounterAttack)
    {
      StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDirection, 0)));
    }
  }

  private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
  {
    yield return new WaitForSeconds(0.4f);
    CreateClone(_transform,_offset);
  }

  protected override void CheckUnlocked()
  {
    
    UnlockCloneAttack();
    UnlockAggresiveClone();
    UnlockMultiClone();
  }
  protected override void Update()
  {
    base.Update();
  }
}
