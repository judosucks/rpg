using UnityEngine;

public class PlayerSlopesClimbState : PlayerGroundedState
{
    private bool isGrounded;
    public PlayerSlopesClimbState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        playerData.isSlopeClimbState = true;
        if (player.isOnSlope && Mathf.Abs(rb.linearVelocity.x) < 0.01f && player.canWalkOnSlope)
        {
            Debug.Log("fullfriction from slope");
            rb.sharedMaterial = player.FullFriction;
        }
        else
        {
            Debug.Log("nofriction from slope");
            rb.sharedMaterial = player.NoFriction;
        }
    }
    public override void Update()
    {
        base.Update();
        // 检测是否可以退出坡地攀爬状态
        if (!isExitingState)
        {
          if (!player.isOnSlope && isGrounded &&  xInput == 0)
          {
              Debug.Log("Exiting SlopeClimbState: Reached flat ground");
              stateMachine.ChangeState(player.idleState); // 或 player.moveState，根据需求调整
          }
          else if (!player.isOnSlope && isGrounded &&  xInput != 0)
          {
              Debug.Log("exiting slope climb state: move state");
              stateMachine.ChangeState(player.moveState);
          } // 如果仍在坡地上，并且有输入，则继续坡地移动逻辑
          else if (player.isOnSlope && !player.canWalkOnSlope)
          {
              Debug.Log("slope climb state: return");
              return;
          }
        }
        if (player.isOnSlope && player.canWalkOnSlope && xInput != 0)
        {
            Debug.Log($"Slope Normal: {player.slopeNormalPerp}");
            player.SetVelocityX(playerData.movementSpeed * player.slopeNormalPerp.x * -xInput);
            player.SetVelocityY(playerData.movementSpeed * player.slopeNormalPerp.y * -xInput);
        }

        
    }
    public override void Exit()
    {
        base.Exit();
        playerData.isSlopeClimbState = false;
        rb.sharedMaterial = player.NoFriction;
        isGrounded = false;
        // 确保 `jumpState` 在状态退出后不会被打断
        Debug.Log("Exiting State: " + this.GetType().Name);

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.IsGroundDetected();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    

    
}