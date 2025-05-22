using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    private bool runJumpInput;
    public PlayerWallGrabState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        HoldPosition();
    }

    public override void Update()
    {
        base.Update();
        runJumpInput = player.inputController.runJumpInput;
        if (!isExitingState)
        {
            HoldPosition();
           if (isClimbingLedgeUp)
           {
              Debug.LogWarning("Wall Grab State Exit climb");
              stateMachine.ChangeState(player.climbState);
           }
           else if (isClimbingLedgeDown || !grabInput)
           {
              Debug.LogWarning("Wall Grab State Exit");
              stateMachine.ChangeState(player.wallSlideState);
           }else if (runJumpInput)
           {
               player.wallJumpState.DetermineWallJumpDirection(isTouchingWall);
               stateMachine.ChangeState(player.wallJumpState);
           }
        }
    }

    private void HoldPosition()
    {
        player.transform.position = holdPosition;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
    }

    public override void Exit()
    {
        base.Exit();
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

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
