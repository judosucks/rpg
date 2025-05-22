using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Yushan.Enums;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private PlayerData playerData => player.playerData;
    [SerializeField] private GameObject playerGameObject; // Ensure this is referenced correctly in the inspector.

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

   

    private void AnimationTriggerClimbEvent()
    {

        player.ledgeClimbState.AnimationTrigger();

    }


    private void AnimationTriggerClimbEndEvent()
    {
        player.ledgeClimbState.AnimationFinishTrigger();
    }

    private void AnimationFinishEdgeClimbEvent()
    {
        player.edgeClimbState.AnimationFinishTrigger();
    }

    

    private void AnimationFinishStandLandTrigger()
    {
        player.standState.AnimationFinishTrigger();
    }

    private void AnimationFinishHighFallTrigger()
    {
        player.highFallLandState.AnimationFinishTrigger();
    }

    private void AnimationFinishMidFallTrigger()
    {
        player.fallLandState.AnimationFinishTrigger();
    }

private void AnimationLighteningTrigger()
    {
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            var currentState = PlayerManager.instance.player.stateMachine.currentState;

            // Skip triggering if the skill is complete or state is transitioning
            if (PlayerManager.instance.player.skill.blackholeSkill.BlackholeSkillCompleted())
            {
                
                return;
            }

            if (currentState == PlayerManager.instance.player.blackholeState)
            {
                Debug.Log("lightning was fired from animationtrigger");
                player.GetComponent<EntityFX>().PlayerLightningFx(player.transform);
                
            }
            
        }
        
    }
    
    
    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(player.attackCheck.position, playerData.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (player.isCrossKick && hit.GetComponent<Enemy>()!=null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                // 应用 CrossKick 的击打逻辑
                Vector2 crossKickForce = new Vector2(player.specialKnockbackForce , player.firstKickKnockbackYdirection);
                hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
                player.stats.DoDamage(_target);
                Debug.Log("enemy received crosskick knockback");
            }

            if (player.isKneeKick && hit.GetComponent<Enemy>()!=null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                Vector2 kneeKickForce = new Vector2(player.kneeKickKnockbackDirection.x,
                    player.kneeKickKnockbackDirection.y);
                hit.GetComponent<Enemy>().ApplyKnockback(kneeKickForce);
                player.stats.DoDamage(_target);
                Debug.Log("enemy received knee kick knockback");
                
            }
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if (_target != null)
                {
                   player.stats.DoDamage(_target);
                      
                }
                ItemDataEquipment weaponData = Inventory.instance.GetEquipmentByType(EquipmentType.Weapon);
                if (weaponData != null)
                {
                    weaponData.ItemEffect(_target.transform);
                }






            }
        }
        
    }

    private void GrenadeThrownEvent()
    {
        Debug.Log("throw grenade event from animationtrigger");
        player.OnAimingStop();
        SkillManager.instance.grenadeSkill.CreateGrenade();
        
        
        if (player.anim.GetBool("AimGrenade"))
        {
            Debug.Log("aim grenade is true turn to false");
          player.anim.SetBool("AimGrenade",false);
          
        }
      
        Debug.Log("grenade thrown is aiming should be false"+" "+playerData.isAiming);
    }

    private void GrenadeAimingEvent()
    {
       player.OnAimingStart();
       Debug.Log("[GrenadeAimingEvent] Aiming set to true");
    }

    private void OnGrendeCancelEndEvent()
    {
        Debug.Log("grenade cancel set to false");
        playerData.grenadeCanceled = false;
    }
    private void ForceToResetTrigger()
    {
        Debug.Log("force to reset trigger");
        player.anim.ResetTrigger("ThrowGrenade");
    }

    private void ForceToResetBool()
    {
        Debug.Log("force to reset bool");
        player.anim.SetBool("AimGrenade", false);
    }

    private void OnFinalCheckAiming()
    {
        if (playerData.isAiming||playerData.isAimCheckDecided||playerData.grenadeCanceled)
        {
            Debug.LogWarning("final check isaiming grenade canceled animatioin grenade cancel"+playerData.isAiming+playerData.grenadeCanceled+playerData.isAimCheckDecided);
            ForceToResetBool();
            ForceToResetTrigger();
            player.OnAimingStop();
            OnAimCheckDecidedToFalseEvent();
            playerData.grenadeCanceled = false;
            
        }
       
    }

    private void OnGrenadeThrowComplete()
    {
        Debug.Log("grenade throw complete");
        StartCoroutine(player.BusyFor(1.0f));
    }

    

    // private void IsBusyIdleToGrenadeState()
    // {
    //     player.anim.SetBool("IsBusy", player.isBusy);
    // }
    private void OnAimCheckDecidedToFalseEvent()
    {
        playerData.isAimCheckDecided = false;
        OnGrenadeThrowComplete();
    } 
    private void OnAimCheckDecidedToTrueEvent()
    {
        playerData.isAimCheckDecided = true;
    }
    private void AnimationFinishAttackTrigger()
    { 
        player.primaryAttackState.AnimationFinishTrigger();
    }

    private void AnimationLandedTrigger()
    {
        player.runJumpLandState.AnimationFinishTrigger();
    }

    private void AnimationSprintLandTrigger()
    {
        player.sprintJumpLandState.AnimationFinishTrigger();
    }
    private void AnimationCounterAttackFinishTrigger()
    {
        player.counterAttackState.AnimationFinishTrigger();
    }

    private void AnimationIsBusyEvent()
    {
        player.throwGrenadeState.AnimationTrigger();
    }

    private void AnimationIsNotBusyEvent()
    {
        player.throwGrenadeState.AnimationFinishTrigger();
    }
    private void AnimationClimbStayTrigger()
    {
        player.ledgeClimbDown.AnimationTrigger();
    }
    private void AnimationFinishTrigger()
    { 
        player.AnimationFinishTrigger();
    }
}