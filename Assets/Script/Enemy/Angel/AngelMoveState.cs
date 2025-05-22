using UnityEngine;

public class AngelMoveState : AngelGroundedState
{
   public AngelMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine,EnemyData _enemyData, string _animBoolName, Enemy_Angel _enemy) : base(_enemyBase, _stateMachine,_enemyData, _animBoolName, _enemy)
   {
      this.enemy = _enemy;
   }


   public override void Enter()
   {
      base.Enter();
   }

   public override void Exit()
   {
      base.Exit();
   }
   public override void Update()
   {
      base.Update();
      enemy.EnemySetVelocity(enemyData.moveSpeed * enemy.facingDirection,rb.linearVelocity.y);
      if (enemy.IsEnemyWallDetected() || !enemy.IsEnemyGroundDetected())
      {
         enemy.Flip();
         stateMachine.ChangeState(enemy.idleState);
      }
   }
}
