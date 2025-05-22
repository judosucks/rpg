using UnityEngine;
using UnityEngine.UIElements;

public class CrystalSkill : Skill
{
   [SerializeField] private GameObject crytalPrefab;
   [SerializeField] private float crystalDuration;
   private GameObject currentCrystal;

   [Header("explosive crystal")] 
   [SerializeField] private bool canExplode;
   
   [Header("moving crystal")]
   [SerializeField] private bool canMoveToEnemy;
   [SerializeField] private float moveSpeed;
   public override void UseSkill()
   {
      base.UseSkill();

      if (currentCrystal == null)
      {
         currentCrystal = Instantiate(crytalPrefab,player.transform.position,Quaternion.identity);
         CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();
         
         currentCrystalScript.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,player);
      }
      else
      {
         Vector2 playerPos = player.transform.position;
         
         player.transform.position = currentCrystal.transform.position;
         currentCrystal.transform.position = playerPos;
         currentCrystal.GetComponent<CrystalSkillController>()?.FinishCryStal();
      }
   }
}
