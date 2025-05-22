using UnityEngine;

public class PlayerPrimaryAttackState : PlayerAbilityState
{
    public int comboCounter{ get;private set;}
    private float lastTimeAttacked;
    private float comboWindow = 0.5f;
    
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        
        //we need this to fix bug on attack direction
        xInput = 0;
        player.isAttacking = true;
        if (comboCounter > 4 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        player.anim.SetInteger("ComboCounter",comboCounter);
        float attackDirection = player.facingDirection;
        if (xInput != 0)
        {
            attackDirection = xInput;
        }
        
        player.SetVelocityX(playerData.attackMovement[comboCounter].x * attackDirection);
        player.SetVelocityY(playerData.attackMovement[comboCounter].y);
        stateTimer = .2f;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("xinput"+xInput+"facing"+player.facingDirection);
        player.StartCoroutine("BusyFor",.01f);
        comboCounter++;
        lastTimeAttacked = Time.time;
        player.isAttacking = false; // 设置攻击状态为 false
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        if (triggerCalled)
        {
            Debug.LogWarning("Trigger Called");
            stateMachine.ChangeState(player.idleState);
        }
        

    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
       
    }
}