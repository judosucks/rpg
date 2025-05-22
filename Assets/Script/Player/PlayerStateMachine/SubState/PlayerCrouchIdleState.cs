using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isCrouchIdleState = true;
        player.SetVelocityX(0f);
        player.SetVelocityY(0f);
        // 调整碰撞器到蹲下尺寸和偏移
        player.colliderManager.EnterCrouch(playerData.crouchColliderSize, playerData.crouchColliderOffset);


    }

    public override void Update()
    {
        base.Update();
        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.crouchMoveState);
            }
            else if (!isCrouchInput && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        playerData.isCrouchIdleState = false;
        // 恢复站立状态的碰撞器尺寸和偏移
        player.colliderManager.ExitCrouch(playerData.standColliderSize, playerData.standColliderOffset);

    }

}