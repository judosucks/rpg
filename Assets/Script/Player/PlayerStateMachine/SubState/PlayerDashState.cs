using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerDashState : PlayerAbilityState
{
    public bool IsDashing { get; private set; }

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter dash");
        player.skill.dashSkill.CloneOnDash();
        IsDashing = true;
        stateTimer = playerData.dashDuration;
       
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dashSkill.CloneOnArrival();
        IsDashing = false;
        rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        else if (stateTimer < 0f)
        {
            player.FallDownForceAndCountdown(0.1f);
            stateMachine.ChangeState(player.idleState);
        }

        // 设置冲刺速度
        player.SetVelocityX(playerData.dashSpeed * player.facingDirection);

    }

   

    
}