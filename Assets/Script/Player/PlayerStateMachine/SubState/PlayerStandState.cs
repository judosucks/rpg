using System.Data.Common;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

public class PlayerStandState : PlayerGroundedState
{
    private bool isGrounded;
    private bool isFrontBottomCheck;
    private bool isLeftEdgeDetected;
    private bool isRightEdgeDetected;
    public PlayerStandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        playerData.reachedApex = false;
        Debug.LogWarning("grounded snap to grid enter");
        player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
        player.SnapToGridSize(); 
        // player.FallDownForceAndCountdown(0.5f);
        

    }

    public override void Update()
    {
        base.Update();
        if (!isExitingState)
        {
            if (xInput != 0)
            {
                if (!isGrounded)
                {
                    Debug.Log("not grounded");
                    stateMachine.ChangeState(player.airState);
                }
                stateMachine.ChangeState(player.moveState);
            }
             else if (triggerCalled)
            {
                if (!isGrounded)
                {
                    Debug.Log("not grounded");
                    stateMachine.ChangeState(player.airState);
                }
                Debug.Log("trigger called");
               stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        isGrounded = false;
        isFrontBottomCheck = false;
        isLeftEdgeDetected = false;
        isRightEdgeDetected = false;

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
       
        isLeftEdgeDetected = player.isNearLeftEdge;
        isRightEdgeDetected = player.isNearRightEdge;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.IsGroundDetected())
        {
            Debug.Log("grounded snap to grid");
            player.SnapToGridSize();
            rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
            
        }
        // if (!isGrounded && player.IsRightGroundDetected() || !isGrounded && player.IsLeftGroundDetected())
        // {
        //     Debug.Log("not grounded moveforward right left");
        //     player.MoveTowardSmooth(playerData.moveDirection * player.facingDirection,playerData.moveDistance);
        //     if (player.IsGroundDetected())
        //     {
        //         Debug.Log("grounded");
        //         player.SnapToGridSize(playerData.gridSize);
        //         rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
        //     }
        // }
        if (!player.IsRightGroundDetected()&& player.facingDirection == 1 ||  player.facingDirection == -1 && !player.IsLeftGroundDetected())
        { 
            if (!isGrounded  || player.isFallingFromEdge && !isGrounded )
            {
                Debug.Log("isfalling");
                
                if (Mathf.RoundToInt(rb.linearVelocity.x) < 0)
                {
                  stateMachine.ChangeState(player.airState);
                }
                
            }
            
        }
        
        
        // else if (isGrounded)
        // {
        //     Debug.Log("grounded");
        //     player.SnapToGrid();
        //     // rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
        // }
        
    }
}