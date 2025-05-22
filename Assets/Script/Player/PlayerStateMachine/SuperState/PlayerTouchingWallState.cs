using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isTouchingWall;
    protected bool isGrounded;
    protected bool grabInput;
    protected int oldXinput;
    protected bool jumpInput;
    protected bool isTouchingLedge;
    protected bool isClimbingLedgeUp;
    protected bool isClimbingLedgeDown;
    public PlayerTouchingWallState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        isClimbingLedgeDown = player.inputController.isClimbLedgeDown;
        isClimbingLedgeUp = player.inputController.isClimbLedgeUp;
        grabInput = player.inputController.grabInput;
        jumpInput = player.inputController.runJumpInput;
        if (isGrounded && !grabInput)
        {
            Debug.LogWarning("gounrded not grab");
            stateMachine.ChangeState(player.idleState);
        }
        else if (!isTouchingWall || (oldXinput != player.facingDirection && !grabInput))
        {
            Debug.LogWarning("not Touching wall but not facing the right direction");
            stateMachine.ChangeState(player.airState);
        }
        else if (isTouchingWall && isTouchingLedge)
        {
            // Verify the ledge position is within a specific vertical range before climbing
            if (Mathf.Abs(player.transform.position.y - LedgeTriggerDetection.ledgePosition.y) < 1f)
            {
                Debug.LogWarning("Ledge Climb Validated, entering climb state.");
                stateMachine.ChangeState(player.ledgeClimbState);
            }
            else
            {
                Debug.LogWarning("Ledge out of reach - climb not triggered.");
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = player.IsWallDetected();
        isGrounded = player.IsGroundDetected();
        oldXinput = xInput;
        isTouchingLedge = LedgeTriggerDetection.isTouchingLedge; // 使用新的 Ledge 检测

        if (isTouchingWall && isTouchingLedge)
        {
            // Verify the ledge position is within a specific vertical range before climbing
            if (Mathf.Abs(player.transform.position.y - LedgeTriggerDetection.ledgePosition.y) < 1f)
            {
                player.ledgeClimbState.SetDetectedPosition(LedgeTriggerDetection.ledgePosition); // 设置悬崖检测点位置
            }
            else
            {
                Debug.LogWarning("Ledge out of reach - climb not triggered.");
            }
            
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

   
}