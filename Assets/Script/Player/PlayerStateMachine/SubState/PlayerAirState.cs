using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private bool isTouchingLedge;
    private bool isTouchingWall;
    private bool isTouchingGround;
    private bool isWallBackDetected;
    private int xInput;
    private bool isTouchingHead;
    private bool isTouchingRightEdge;
    private bool isTouchingLeftEdge;
    private bool isTouchingGroundBottom;
    
    private bool isEdgeGrounded;
    public bool oldIsTouchingGround;
    private float fallTime = 0f;
    
    private bool grabInput;
    private bool jumpInput;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool coyoteTime;
    private bool isJumping;
    private bool jumpInputStop;
    private bool isTouchingLedgeFirst;
    private bool isTouchingLeftGround;
    private bool isTouchingRightGround;
    public bool isCoyoteTimeTriggered;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData,
        string _animBoolName) : base(_player,
        _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        fallTime = 0f;
        playerData.isInAir = true;
        // player.startFallHeight = 0f;
        player.startFallHeight = player.transform.position.y;
        
        // 仅对垂直速度进行重置
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        isCoyoteTimeTriggered = false; // 重置coyoteTime触发器


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       
    }


    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingLeftGround = player.IsLeftGroundDetected();
        isTouchingRightGround = player.IsRightGroundDetected();
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge;
        isTouchingWall = player.IsWallDetected();
        isTouchingGround = player.IsGroundDetected();
        oldIsTouchingGround = isTouchingGround;
        isWallBackDetected = player.IsWallBackDetected();
        isTouchingHead = player.CheckIfTouchingHead();
       
        isTouchingLeftEdge = player.isNearLeftEdge;
        isTouchingRightEdge = player.isNearRightEdge;
        isTouchingGroundBottom = player.IsBottomGroundDetected();
    
        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isWallBackDetected;
        if (!wallJumpCoyoteTime && !isTouchingWall && !isWallBackDetected &&
            (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }

        
        if (isTouchingLedge && !playerData.isClimbLedgeState && !isTouchingLedgeFirst)
        {
            Debug.Log("ledgeposition");
            isTouchingLedgeFirst = true;
            player.ledgeClimbState.SetDetectedPosition(LedgeTriggerDetection.ledgePosition);

        }

        
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isInAir = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isWallBackDetected = false;
        isTouchingHead = false;
        playerData.reachedApex = false;
        isTouchingRightEdge = false;
        isTouchingLeftEdge = false;
        isTouchingGroundBottom = false;
        
       
        isEdgeGrounded = false;
        isTouchingGround = false;
        oldIsTouchingGround = false;
        
      
        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingLedgeFirst = false;
    }   


    public override void Update()
    {
        base.Update();
        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        xInput = player.inputController.norInputX;
        grabInput = player.inputController.grabInput;
        jumpInput = player.inputController.runJumpInput;
        jumpInputStop = player.inputController.jumpInputStop;
        // CheckJumpMutiplier();
        
if (!isTouchingGround && !isTouchingWall || !isTouchingGround && isTouchingWall)
{
    player.SetColliderMaterial(player.noFrictionMaterial); // Set no friction in the air
            
            fallTime += Time.deltaTime;
            float newVelocity = rb.linearVelocity.y
                                + (Physics2D.gravity.y * playerData.gravityMultiplier * Time.deltaTime);
            if (newVelocity < -playerData.maxFallSpeed)
            {
                newVelocity = -playerData.maxFallSpeed;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, newVelocity);
            HandleAirMovement();
}
else
{
    player.SetColliderMaterial(player.frictionMaterial); // Set friction when on the ground
}


// Check if close to the ground (either moving up or down slowly)
if (isTouchingGround)
{
    fallTime = 0f;
    float fallDistance = player.startFallHeight - player.transform.position.y;

    // Check if truly on the ground (moving down or stationary)
    
        
        // 优化空中到地面状态的切换条件
        if (fallDistance >= player.highFallThreshold)
            stateMachine.ChangeState(player.highFallLandState);
        else if (fallDistance >= player.midFallThreshold)
            stateMachine.ChangeState(player.fallLandState);
        else
            stateMachine.ChangeState(player.runJumpLandState);
    
}

if (isTouchingLedge && !playerData.isClimbLedgeState && !isTouchingGround && isTouchingLedgeFirst)
{
    Debug.Log("ledge");
    isTouchingLedgeFirst = false;
    stateMachine.ChangeState(player.ledgeClimbState);
}
        // else if (jumpInput && (isTouchingWall || isWallBackDetected || wallJumpCoyoteTime))
        // {   
        //     StopWallJumpCoyoteTime();
        //     isTouchingWall = player.IsWallDetected();
        //     player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
        //     stateMachine.ChangeState(player.wallJumpState);
        // }
        // if (jumpInput && player.jumpState.CanJump())
        // {
        //     Debug.LogWarning("double jump");
        //     stateMachine.ChangeState(player.jumpState);
        // }
      
        if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        if (isTouchingWall && xInput == player.facingDirection && rb.linearVelocity.y < 0f)
        {
            Debug.LogWarning("istouchingwall y < 0 xinput");
            playerData.reachedApex = false;
            playerData.isWallSliding = true;
            stateMachine.ChangeState(player.wallSlideState);
        }
        
        
       
        if (player.isFallingFromEdge)
        {
            Debug.LogWarning("fallingfromedge");
           
            player.isFallingFromEdge = false;
            return;
        }
        
    }

    private void HandleAirMovement()
    {
        
        player.SetVelocityX(playerData.airMovementSpeed * xInput);
        player.anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        player.anim.SetFloat("yVelocity", rb.linearVelocity.y); // 更新动画
        player.CheckIfShouldFlip(xInput); // 检查翻转
    }

    private void CheckJumpMutiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            { 
                Debug.LogWarning("jumpjump");
                player.SetVelocityY(rb.linearVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;    
            }
            else if (rb.linearVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            Debug.Log("checkcoyotetime");
            coyoteTime = false;
            player.jumpState.DecrementAmountOfJumpsLeft();
            player.sprintJumpState.DecrementAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime()
    {
       coyoteTime = true;
       startTime = Time.time;
    } 
    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }
    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
    public void SetIsJumping()=>isJumping = true;
}