using UnityEngine;

public class PlayerHurtState : PlayerState
{
    private Enemy _enemy;
    private float knockbackDuration = 0.2f; // Optional: Duration of neutralizing input
    private Vector2 knockbackForce; // Knockback velocity
    public PlayerHurtState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) 
        : base(_player, _stateMachine, _playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("hurt state enter");
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        stateTimer = knockbackDuration;
        // 其他逻辑代码在这里...

        knockbackForce  = new Vector2(-2, rb.linearVelocity.y);
        // Apply knockback force if applicable
        player.SetVelocityX(knockbackForce.x * -player.facingDirection);
        player.SetVelocityY(knockbackForce.y);
        // if (_enemy.facingDirection == player.facingDirection)
        // {
        //     player.SetVelocityX(knockbackForce.x * -player.facingDirection);
        //     player.SetVelocityY(knockbackForce.y);
        // }
        // Disable player controls (isBusy prevents inputs)
        player.SetIsBusy(true);
        
        // Trigger hurt animation
        player.anim.SetTrigger("Hurt");
        
        // Set a timer (optional) to end the hurt state after knockback

    }

    public override void DoChecks()
    {
        base.DoChecks();

        
    }

    public override void Update()
    {
        base.Update();

        // If timer reaches 0, return to appropriate state
        if (stateTimer < 0)
        {
            // Transition back to `IdleState` or `AirState` depending on context
            if (player.IsGroundDetected())
            {
                Debug.Log("grounded from hurt");
                player.SetIsBusy(false);
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                Debug.Log("air from hurt");
                player.SetIsBusy(false);
                stateMachine.ChangeState(player.airState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Re-enable player controls
        player.SetIsBusy(false);
    }
}