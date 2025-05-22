using UnityEngine;

public class PlayerHighFallLandState : PlayerGroundedState
{
    public PlayerHighFallLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isFallingFromEdge = false;
        player.isFallingFromJump = false;
        playerData.isHighFallLandState = true;
        CameraManager.instance.newCamera.ShakeCamera(.9f, .9f);
        player.SetVelocityY(0f);
        player.SnapToGridSize();
        player.FallDownForceAndCountdown(1.5f);
        
    }
   
    public override void Update()
    {
        base.Update(); 
       
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
        playerData.isHighFallLandState = false;
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
