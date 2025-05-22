using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerCrossKickState : PlayerState
{      
    
    
    private float cooldownTime = 1f; // 冷却时间，单位：秒
    public PlayerCrossKickState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 使用跨踢击飞的力量（例如：y方向更大的击飞力）
        Vector2 knockbackForce = player.firstKickKnockbackForce; 
        float xVelocity = knockbackForce.x * player.facingDirection;
        float yVelocity = knockbackForce.y;

        player.SetVelocityX(xVelocity);
        player.SetVelocityY(yVelocity);
        // 启动冷却协程
        player.StartCoroutine(CrossKickCooldown());
    }

    public override void Exit()
    {
        base.Exit();
       
        
    }

    public override void Update()
    {
        base.Update();

    }
    private IEnumerator CrossKickCooldown()
    {
        // 禁用玩家输入
        player.SetIsBusy(true);
        
        // 等待冷却时间
        yield return new WaitForSeconds(cooldownTime);
        
        // 冷却结束，恢复输入
        player.SetIsBusy(false);
        // 切换回待机状态
        stateMachine.ChangeState(player.idleState);
    }
   
   
}