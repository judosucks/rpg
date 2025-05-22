using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerFallLandState : PlayerGroundedState
{
    
    public PlayerFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isFallingFromEdge = false;
        player.isFallingFromJump = false;
        playerData.isFallLandState = true;
        player.SetVelocityY(0f);
        player.SnapToGridSize();
        player.FallDownForceAndCountdown(1f);
        
     
    }

    
    
    public override void Update()
    {
        base.Update();
        // if (xInput != 0)
        // {
        //     stateMachine.ChangeState(player.moveState);
        // }else 
        
        if (!isExitingState)
        {
             
            if (xInput != 0) 
            { 
                Debug.LogWarning("run"); 
                stateMachine.ChangeState(player.moveState);
            }
            if(triggerCalled)
            {
                stateMachine.ChangeState(player.idleState);
            }
            
        }

        
        
    }
    
    public override void Exit()
    {
        base.Exit();
        playerData.isFallLandState = false;
       
    }

    public override void DoChecks()
    {
        base.DoChecks();
      
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
