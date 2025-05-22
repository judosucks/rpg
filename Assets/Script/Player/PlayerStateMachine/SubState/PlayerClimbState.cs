using UnityEngine;
using UnityEngine.PlayerLoop;
public class PlayerClimbState : PlayerTouchingWallState
{
    
    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) 
        : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isClimbLedgeState = true;
       
    }

    public override void Update()
    {
        base.Update();
        if (!isExitingState)
        {
          player.SetVelocityY(playerData.wallClimbVelocity);
          
          if (yInput != 1 && !isExitingState)
          {
             stateMachine.ChangeState(player.wallGrabState);
          }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       
    }

    public override void DoChecks()
    {
        base.DoChecks();
        
    }

    public override void Exit()
    {
        base.Exit();
        player.startFallHeight = player.transform.position.y;
        playerData.isClimbLedgeState = false;
        rb.linearVelocity = Vector2.zero;
    }
}
