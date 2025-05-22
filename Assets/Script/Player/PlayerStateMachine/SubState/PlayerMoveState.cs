using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using System;
using UnityEngine.InputSystem.LowLevel;

public class PlayerMoveState : PlayerGroundedState
{
    private bool isEdgeCheck;
    private bool isEdgeWallCheck;
    private bool edgeTouched;
    private bool isRunning;
    private bool sprintInput;
    private bool isGrounded;
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
        isEdgeCheck = player.CheckIfTouchingEdge();
        
        if (!isEdgeCheck && isEdgeWallCheck && !edgeTouched && player.IsGroundDetected())
        {
            Debug.LogWarning("Edge Wall");
            edgeTouched = true;
            player.edgeClimbState.SetDetectedEdgePosition(player.transform.position);
        }
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // player.SlopeCheck(); // 每帧重新检测斜坡状态
        //
        // if(!isEdgeCheck && isEdgeWallCheck && edgeTouched)
        // {
        //     Debug.LogWarning("Edge Wall Check");
        //     stateMachine.ChangeState(player.edgeClimbState);
        // }
        // else if (isGrounded && player.isOnSlope && player.canWalkOnSlope)
        // {
        //     Debug.Log($"State Change to SlopeClimbState | isGrounded: {isGrounded}, isOnSlope: {player.isOnSlope}, canWalkOnSlope: {player.canWalkOnSlope}");
        //     stateMachine.ChangeState(player.slopeClimbState);
        // }
        
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isRun = true;
        
        
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
        playerData.isRun = false;
        isEdgeCheck = false;
        isEdgeWallCheck = false;
        edgeTouched = false;
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        sprintInput = player.inputController.sprintInput;
        player.CheckIfShouldFlip(xInput);

        // if (!isExitingState && player.inputController.runJumpInput && player.jumpState.CanJump())
        // {
        //     Debug.Log("Switching to PlayerJumpState from PlayerMoveState");
        //     stateMachine.ChangeState(player.jumpState);
        //     return;
        // }
        if (!isExitingState)
        {

          
           if (xInput == 0) // 如果停止移动
           {
               isRunning = false;
                   stateMachine.ChangeState(player.idleState); // 进入待机状态
               
           }
           else if (isCrouchInput)
           {
               stateMachine.ChangeState(player.crouchMoveState);
           }else if (player.isOnSlope && player.canWalkOnSlope && isGrounded)
           {
               Debug.LogWarning("change to slop climb state");
               stateMachine.ChangeState(player.slopeClimbState);
           }
           else
           {
               isRunning = true;
               player.SetVelocityX(xInput * playerData.movementSpeed); // 常规移动速度
               // player.StartCoroutine(StartSprintCoroutine(2f));
           }

           

        }
    }

    private IEnumerator StartSprintCoroutine(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        Debug.Log("Sprint duration ended");
        stateMachine.ChangeState(player.sprintState);
    }
 
}