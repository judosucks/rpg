using UnityEngine;

namespace YushanBehaviours
{
    public class AnimationOverride : MonoBehaviour
    {
        private Animator anim;
    
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }
    
        public void SetAnimations(AnimatorOverrideController controller)
        {
            anim.runtimeAnimatorController = controller;
        }
    }
    
}
