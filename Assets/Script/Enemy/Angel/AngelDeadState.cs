using UnityEngine;

public class AngelDeadState : EnemyState
{
    private Enemy_Angel enemy;
    public AngelDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, EnemyData _enemyData,string _animBoolName,Enemy_Angel _enemy) : base(_enemyBase, _stateMachine,_enemyData, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 3);
        }
    }
}
