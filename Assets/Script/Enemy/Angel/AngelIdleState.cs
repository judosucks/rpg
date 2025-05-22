using UnityEngine;

public class AngelIdleState : AngelGroundedState
{
    public AngelIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine,EnemyData _enemyData, string _animBoolName, Enemy_Angel _enemy) : base(_enemyBase, _stateMachine, _enemyData,_animBoolName, _enemy)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
{
    base.Enter();
    stateTimer = enemyData.idleTime;
}

public override void Update()
{
    base.Update();
    if (stateTimer < 0)
    {
        stateMachine.ChangeState(enemy.moveState);
    }
}

public override void Exit()
{
    base.Exit();
    // Custom logic for exiting the idle state
}
}
