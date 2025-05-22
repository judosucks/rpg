using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState
{
   
    public PlayerCrouchMoveState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (!isExitingState)
        {
            player.SetVelocityX(playerData.crouchMovementSpeed * player.facingDirection);
            player.CheckIfShouldFlip(xInput);
            if (xInput == 0 || isCrouchInput)
            {
                stateMachine.ChangeState(player.crouchIdleState);
            }
            else if (!isCrouchInput && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isCrouchMoveState = true;
        player.colliderManager.EnterCrouch(playerData.crouchColliderSize, playerData.crouchColliderOffset);
    }

    public override void Exit()
    {
        base.Exit();
        playerData.isCrouchMoveState = false;
        player.colliderManager.ExitCrouch(playerData.standColliderSize, playerData.standColliderOffset);
    }
}