using UnityEngine;

public class AngelGroundedState : EnemyState
{
    protected Enemy_Angel enemy;
    protected Transform player;
    public AngelGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine,EnemyData _enemyData, string _animBoolName,Enemy_Angel _enemy) : base(_enemyBase, _stateMachine,_enemyData, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position,player.transform.position)<2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
