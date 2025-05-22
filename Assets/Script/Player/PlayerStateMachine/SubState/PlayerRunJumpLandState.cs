using UnityEngine;

public class PlayerRunJumpLandState : PlayerGroundedState
{
    private bool isGrounded;
    public PlayerRunJumpLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputController.UseRunJumpInput();
        player.isFallingFromJump = false;
        playerData.isRunJumpLandState = true;
        player.SetVelocityY(0f);
        player.SetVelocityX(0f);
        // player.SlopeCheck(); // 刷新坡地检测
        // if (player.isOnSlope && player.canWalkOnSlope)
        // {
        //     Debug.Log("Switching to SlopeClimbState on Enter");
        //     stateMachine.ChangeState(player.slopeClimbState);
        // }

    }

    public override void Exit()
    {
        base.Exit();
       playerData.isRunJumpLandState = false;
    }

    public override void Update()
    {
        base.Update();

        if (!isExitingState)
        {
            if(triggerCalled)
            {
                Debug.Log("Trigger Called");
                stateMachine.ChangeState(player.idleState);
            }else if (xInput != 0)
            {
                Debug.Log("input not 0");
                stateMachine.ChangeState(player.moveState);
            }
        }
            
            
            // else if (player.isOnSlope && player.canWalkOnSlope && isGrounded)
            // {
            //     stateMachine.ChangeState(player.slopeClimbState);
            // }  
            
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // player.SlopeCheck(); // 每帧重新检测斜坡状态
        //
        //
        // if (isGrounded && player.isOnSlope && player.canWalkOnSlope)
        // {
        //     Debug.Log($"State Change to SlopeClimbState | isGrounded: {isGrounded}, isOnSlope: {player.isOnSlope}, canWalkOnSlope: {player.canWalkOnSlope}");
        //     stateMachine.ChangeState(player.slopeClimbState);
        //
        // }
    }
    
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}