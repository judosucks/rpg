using UnityEngine;

public class AngelAttackState : EnemyState
{
    private Enemy_Angel  enemy;
    public AngelAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, EnemyData _enemyData,string _animBoolName,Enemy_Angel enemy) : base(_enemyBase, _stateMachine, _enemyData,_animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.EnemyZeroVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
