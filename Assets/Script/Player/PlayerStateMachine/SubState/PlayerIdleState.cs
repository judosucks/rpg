using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerIdleState : PlayerGroundedState
{
    private bool isGrounded;
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
        // rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        playerData.isIdle = true;
        player.colliderManager.EnterCrouch(playerData.standColliderSize, playerData.standColliderOffset);
   
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
        playerData.isIdle = false;
        // rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        player.colliderManager.ExitCrouch(playerData.standColliderSize, playerData.standColliderOffset);
    }

    public override void Update()
    {
        base.Update();
        
        // if (isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.01f)
        // {
        //     rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // 取消横向移动
        // }

        if (!isExitingState)
        {
          
            if (xInput != 0 && !player.isBusy && !player.isAttacking)
            {
                stateMachine.ChangeState(player.moveState); 
            }else if (isCrouchInput && !isTouchingLedgeDown || isTouchingCeiling )
            {
                Debug.LogWarning("crouch from idle");
                stateMachine.ChangeState(player.crouchIdleState);
                return;
            }
            // else if (player.isOnSlope && player.canWalkOnSlope && isGrounded)
            // {
            //     stateMachine.ChangeState(player.slopeClimbState);
            // }
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // player.SlopeCheck(); // 每帧重新检测斜坡状态
        //
        // if (isGrounded && player.isOnSlope && player.canWalkOnSlope)
        // {
        //     Debug.Log($"State Change to SlopeClimbState | isGrounded: {isGrounded}, isOnSlope: {player.isOnSlope}, canWalkOnSlope: {player.canWalkOnSlope}");
        //     stateMachine.ChangeState(player.slopeClimbState);
        // }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
    }
    
}