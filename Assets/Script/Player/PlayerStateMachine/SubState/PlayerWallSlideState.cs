using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isTouchingLedge;
 
    private bool isClimbing;
    private bool isWallBottomDetected;
    private bool runJumpInput;
    private bool sprintJumpInput;
    private bool isTouchingGroundBottom;
    private bool isEdgeGrounded;
    
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player,
        _stateMachine, _playerData, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        playerData.isWallSliding = true;
        playerData.isWallSlidingState = true;
        
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isWallSliding = false;
        playerData.isWallSlidingState = false;
        isTouchingGround = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isClimbing = false;
        isWallBottomDetected = false;
        runJumpInput = false;
        sprintJumpInput = false;
        playerData.reachedApex = false;
        isEdgeGrounded = false;
        isTouchingGroundBottom = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge;
     
        isTouchingGroundBottom = player.IsBottomGroundDetected();
    }
    

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
       
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        if (!isExitingState)
        {
           player.SetVelocityY(-playerData.wallSlideVelocity);
           if (yInput == 0 && grabInput)
           {
              stateMachine.ChangeState(player.wallGrabState);
           }
           else if (runJumpInput)
           {
               player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
               stateMachine.ChangeState(player.wallJumpState);
           }
           else if (isTouchingGroundBottom)
           {
              Debug.Log("force to exit wall slide");
              player.MoveTowardSmooth(playerData.moveDirection * -player.facingDirection,playerData.moveAlittleDistance);
              rb.AddForce(Vector2.down * playerData.stickingForce,ForceMode2D.Impulse);
              stateMachine.ChangeState(player.airState);
           }
        }
    }
}


