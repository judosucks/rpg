using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ParrySkill : Skill
{
   [SerializeField] private UISkillTreeSlot parryUnlockedButton;
   public bool parryUnlocked;
   [SerializeField] private UISkillTreeSlot restoreUnlockedButton;
   public bool restoreUnlocked;
   [SerializeField] private UISkillTreeSlot parryWithMirageUnlockedButton;
   public bool parryWithMirageUnlocked;
   [Range(0f,1f)]
   [SerializeField]private float restoreHealthPercent;
   public override void UseSkill()
   {
      base.UseSkill();
      if (restoreUnlocked)
      {
         int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercent);
         player.stats.IncreaseHealthBy(restoreAmount);
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
      parryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
      restoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
      parryWithMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
      CheckUnlocked();
   }

   protected override void CheckUnlocked()
   {
      UnlockParry();
      UnlockParryRestore();
      UnlockParryWithMirage();
   }
private void UnlockParry()
   {
      if (parryUnlockedButton.unlocked)
         parryUnlocked = true;
   }

   private void UnlockParryRestore()
   {
      if (restoreUnlockedButton.unlocked)
         restoreUnlocked = true;
   }

   private void UnlockParryWithMirage()
   {
      if (parryWithMirageUnlockedButton.unlocked)
         parryWithMirageUnlocked = true;
   }

   public void MakeMirageOnParry(Transform _respawnTransform)
   {
      if(parryWithMirageUnlocked)
         SkillManager.instance.cloneSkill.CreateCloneOnCounterAttack(_respawnTransform);
   }
   protected override void Update()
   {
      base.Update();
   }
}
