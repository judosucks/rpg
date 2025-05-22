using UnityEngine;

namespace YushanBehaviours
{
   public class BodyAttackAnimation : MonoBehaviour
   {
      [SerializeField] private AnimatorOverrideController[] animatorOverrideController;
      [SerializeField] private AnimationOverride animationOverride;
   
      public void Set(int value)
      {
         animationOverride.SetAnimations(animatorOverrideController[value]);
      }
   }
   
}
