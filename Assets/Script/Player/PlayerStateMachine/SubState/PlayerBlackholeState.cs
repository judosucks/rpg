using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;


public class PlayerBlackholeState : PlayerAbilityState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravityScale;
  
   
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData,_animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
      
        defaultGravityScale = rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        
        rb.gravityScale = 0;
        playerData.isBlackholeState = true;

    }

    
    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0,3);
        }
        

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, 0);
            
            if (!skillUsed)
            {
                if (player.skill.blackholeSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
            
        }
        if (player.skill.blackholeSkill.BlackholeSkillCompleted())
        {
            Debug.Log("blackhole skill completed change to air state");
            stateMachine.ChangeState(player.airState);
        }
        
    }
   
    public override void Exit()
    {
        base.Exit();
        isAbilityDone = true;
        rb.gravityScale = defaultGravityScale;
        PlayerManager.instance.player.entityFX.MakeTransparent(false);
        playerData.isBlackholeState = false;
        // PlayerManager.instance.player.anim.Play("Idle"); // Force idle animation
    }
    
}
