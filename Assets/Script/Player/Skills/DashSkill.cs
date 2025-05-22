using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class DashSkill : Skill
{
   
   
   [Header("dash")] 
   public bool dashUnlocked { get;private set; }
   [SerializeField] private UISkillTreeSlot dashUnlockedButton;
   [Header("clone on dash")] 
   public bool cloneOnDashUnlocked{ get;private set; }
   [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;
   [Header("clone on dash arrival")] 
   public bool cloneOnDashArrivalUnlocked{ get;private set; }
   [SerializeField] private UISkillTreeSlot cloneOnDashArrivalUnlockedButton;

   public override void UseSkill()
   {
      base.UseSkill();
      Debug.Log("created clone behind");
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

      Debug.Log("DashSkill: UISkillTreeSlot initialization complete, proceeding...");
      dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
      cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
      cloneOnDashArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDashArrival);
      CheckUnlocked();
   }
   protected override void CheckUnlocked()
   {
      UnlockDash();
      UnlockCloneOnDash();
      UnlockCloneOnDashArrival();
   }
   private void UnlockDash()
   {
      Debug.Log("attempt to unlocked dash");
      if (dashUnlockedButton.unlocked)
      {
        dashUnlocked = true;
        Debug.Log("unlocked dash"); 
      }
   }

   private void UnlockCloneOnDash()
   {
      Debug.Log("attempt to unlocked clone on dash");
      if (cloneOnDashUnlockedButton.unlocked)
      {
         Debug.Log("unlocked clone on dash");
        cloneOnDashUnlocked = true;
      }
   }
   private void UnlockCloneOnDashArrival()
   {
      Debug.Log("attempt to unlocked clone on dash arrival");
      if (cloneOnDashArrivalUnlockedButton.unlocked)
      {
         Debug.Log("unlocked clone on dash arrival");
        cloneOnDashArrivalUnlocked = true;
      }
   }
   public void CloneOnDash()
   {
      if (cloneOnDashUnlocked)
      {
         SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero );
      }
   }

   public void CloneOnArrival()
   {
      if (cloneOnDashArrivalUnlocked)
      {
         SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
      }
   }

   protected override void Update()
   {
      base.Update();
   }
}
