using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DodgeSkill : Skill
{
    [SerializeField]private int evasionAmount;
    [Header("Dodge Skill")] [SerializeField]
    private UISkillTreeSlot unlockDodgeButton;

    public bool dodgeUnlocked;

    [SerializeField] private UISkillTreeSlot unlockDodgeMirageButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(WaitForSkillTreeSlotInitialization());
        
    }

    protected override void CheckUnlocked()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found, using PlayerManager instance");
            player = PlayerManager.instance.player;
            if(player != null)
            {
                Debug.LogWarning("Player found");
                
                UnlockDodge();
                UnlockDodgeMirage();
            }
            
        }
        else
        {
            UnlockDodge();
            UnlockDodgeMirage();
        }
        
    }


    private IEnumerator WaitForSkillTreeSlotInitialization()
    {
       while (!isSkillTreeSlotInitialized)
       {
          yield return null; // Wait for one frame
       }
      unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
      unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
     
        CheckUnlocked();
     
    }

private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            if(player.stats.evasion == null)
            {
                Debug.LogWarning("Evasion stat not found on player");
                PlayerManager.instance.player.stats.evasion.AddModifier(evasionAmount);
                Inventory.instance.UpdateStatSlotUI();
                dodgeUnlocked = true;
                return;
            }
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatSlotUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void DodgeWithMirage()
    {
        if (dodgeMirageUnlocked)
        {
          Debug.Log("dodge with mirage");
          SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(2,0));
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}
