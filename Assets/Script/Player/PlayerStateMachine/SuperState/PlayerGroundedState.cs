using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    private bool runJumpInput;
    private bool sprintJumpInput;
    protected int xInput;
    protected int yInput;
    private bool isTouchingWall;
    protected bool isTouchingLedge;
    private bool isTouchingHead;
    private bool isTouchingGround;
    protected bool isTouchingWallBack;
    private bool grabInput;
    protected bool isTouchingCeiling;
    protected bool isTouchingWallBottom;
    protected bool isTouchingLedgeDown;
    protected bool isTouchingRightGround;
    protected bool isTouchingLeftGround;
    protected bool isCrouchInput;
    protected bool isClimbingLedgeUp;
    protected bool isClimbingLedgeDown;
    private bool isLedgeClimbDown;
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    
    public override void Enter()
    {
        base.Enter();
        playerData.isGroundedState = true;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
        player.SetColliderMaterial(player.frictionMaterial); // Set no friction in the air
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isGroundedState = false;
        
        isTouchingGround = false;
        isTouchingHead = false;
        isTouchingWall = false;
        isTouchingLedge = false;
        isTouchingWallBack = false;
        isTouchingCeiling = false;
        isTouchingWallBottom = false;
        isTouchingLedgeDown = false;
        isTouchingLeftGround = false;
        isTouchingRightGround = false;
        isClimbingLedgeUp = false;
        isClimbingLedgeDown = false;
        isLedgeClimbDown = false;
    }

    

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingCeiling = player.CheckIfTouchingCeiling();
        isTouchingGround = player.IsGroundDetected();
        isTouchingWall = player.IsWallDetected();
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge;
        isTouchingHead = player.CheckIfTouchingHead();
        isTouchingWallBack = player.IsWallBackDetected();
        player.sprintJumpState.ResetAmountOfJumps();
        player.jumpState.ResetAmountOfJumps();
        isTouchingLedgeDown = player.LedgeDownTriggerDetection.isTouchingLedgeDown;
        isTouchingRightGround = player.rightEdgeTrigger.isNearRightEdge;
        isTouchingLeftGround = player.leftEdgeTrigger.isNearLeftEdge;
        if (isTouchingLedgeDown && isClimbingLedgeDown)
        {
            player.ledgeClimbDown.SetDetectedPosition(player.LedgeDownTriggerDetection.ledgePositionDown);
            isLedgeClimbDown = true;
        }
        


    }

    public override void Update()
    {
        base.Update();
        runJumpInput = player.inputController.runJumpInput;
        sprintJumpInput = player.inputController.sprintJumpInput;
        grabInput = player.inputController.grabInput;
        isClimbingLedgeUp = player.inputController.isClimbLedgeUp;
        isClimbingLedgeDown = player.inputController.isClimbLedgeDown;
        isCrouchInput = player.inputController.isCrouchInput;
        
        // if the player is not on the ground transition to air state
        if (!isTouchingGround&&!isTouchingCeiling&&!isTouchingLedgeDown  && !player.isAttacking && !playerData.isCounterAttackState && !playerData.isBlackholeState && !playerData.isGrenadeState && !playerData.isSlopeClimbState&&!playerData.isLedgeClimbDown)
        {
            player.startFallHeight = player.transform.position.y;
            
            Debug.LogWarning("istouchingground");
            stateMachine.ChangeState(player.airState);
            return;
        }
        

        if (runJumpInput && playerData.isRun && player.jumpState.CanJump()  || runJumpInput && playerData.isIdle && player.jumpState.CanJump() )
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            Debug.Log("jump from ground");
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (sprintJumpInput && playerData.isSprint && player.sprintJumpState.CanJump() )
        {
            playerData.highestPoint = player.transform.position.y;
            if (isTouchingHead) return;
            stateMachine.ChangeState(player.sprintJumpState);
            return;
        }

        
        if (isTouchingWall && grabInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
            return;
        }
        if (isTouchingLedgeDown && isLedgeClimbDown&& isLedgeClimbDown)
        {
            isLedgeClimbDown = false;
            stateMachine.ChangeState(player.ledgeClimbDown);
            return;
        }

        if (isTouchingWall)
        {
           
            return;
        }
        
        if (Keyboard.current.rKey.wasPressedThisFrame && player.skill.blackholeSkill.unlockBlackhole)
        {
            Debug.LogWarning("blackhole");
            stateMachine.ChangeState(player.blackholeState);
            return;
        }

        if (mouse.rightButton.isPressed && playerData.rightButtonLocked)
        {
            return;
        }
    if (mouse.rightButton.isPressed && !player.grenade && player.skill.grenadeSkill.grenadeUnlocked)
        {
            playerData.mouseButttonIsInUse = true;
            if (playerData.grenadeCanceled)
            {
                playerData.rightButtonLocked = true;
                return;
            }
            
            playerData.rightButtonLocked = true;
            stateMachine.ChangeState(player.throwGrenadeState);
            return;
        }

        if (mouse.rightButton.wasReleasedThisFrame)
        {
            if (playerData.grenadeCanceled || !playerData.isAiming)
            {
                playerData.mouseButttonIsInUse = false;
                playerData.rightButtonLocked = false;
            }
        }
        
        if (Keyboard.current.qKey.wasPressedThisFrame && player.skill.parrySkill.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame&&!playerData.isLedgeClimbDown && !playerData.isSlopeClimbState && !playerData.isCrouchIdleState && !playerData.isCrouchMoveState || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame) && !playerData.isCrouchIdleState && !playerData.isSlopeClimbState && !playerData.isCrouchMoveState)
        {
            if (playerData.mouseButttonIsInUse)
            {
                return;
            }
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
        isClimbingLedgeDown = player.inputController.isClimbLedgeDown;
        isClimbingLedgeUp = player.inputController.isClimbLedgeUp;
        // if close to the edge and not on the ground fall
        if (isTouchingGround)
        {
          if (xInput <= 0 && player.facingDirection == -1  || xInput >= 0 && player.facingDirection == 1 )
          {
            if ((xInput <= 0 && !isTouchingLeftGround && isTouchingRightGround) || (xInput >= 0 && !isTouchingRightGround && isTouchingLeftGround))
            {
                player.isFallingFromEdge = !isTouchingGround;
                Debug.LogWarning("exit left or right edge");
                if (!isTouchingLeftGround && isTouchingRightGround && isTouchingLedgeDown&&isClimbingLedgeDown)
                {
                      Debug.LogWarning("ledge down");
                      isLedgeClimbDown = true;
                      player.ledgeClimbDown.SetDetectedPosition(player.LedgeDownTriggerDetection.ledgePositionDown);
                    
                }else if (!isTouchingRightGround && isTouchingLeftGround && isTouchingLedgeDown && isClimbingLedgeDown)
                {
                        isLedgeClimbDown = true;
                        player.ledgeClimbDown.SetDetectedPosition(player.LedgeDownTriggerDetection.ledgePositionDown);
                        Debug.LogWarning("ledge down left ground");
                }
                if (player.isFallingFromEdge)
                {
                    player.startFallHeight = player.transform.position.y;
                    player.airState.StartCoyoteTime();
                    stateMachine.ChangeState(player.airState);
                    Debug.Log("falling from left edge startcoyotetime");
                }
            }
          }
            
        }
    }
    
}