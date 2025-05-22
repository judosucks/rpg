using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Collections.Generic;
using System.Collections;
public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;
    private bool grabInput;
    private bool isTouchingWall;
    
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Wall Jump");
        
        player.inputController.UseRunJumpInput();
        player.inputController.UseSprintJumpInput();
        player.jumpState.ResetAmountOfJumps();
        player.sprintJumpState.ResetAmountOfJumps();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle,wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.jumpState.DecrementAmountOfJumpsLeft();
        player.sprintJumpState.DecrementAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
        isTouchingWall = false;

    }

    public override void Update()
    {
        base.Update();
        grabInput = player.inputController.grabInput;
        player.anim.SetFloat("yVelocity",rb.linearVelocity.y);
        player.anim.SetFloat("xVelocity",Mathf.Abs(rb.linearVelocity.x));
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            Debug.LogWarning("isabilityDone");
            isAbilityDone = true;
        }
        if(grabInput && isTouchingWall)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = player.IsWallDetected();
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -player.facingDirection;
        }
        else
        {
            wallJumpDirection = player.facingDirection;
        }
    }
}