using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyState 
{
   protected EnemyStateMachine stateMachine;
   protected Enemy enemyBase;
   protected EnemyData enemyData;
   protected Rigidbody2D rb;
   protected bool triggerCalled;
   private string animBoolName;
   protected float stateTimer;

   public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine,EnemyData _enemyData, string _animBoolName)
   {
      this.enemyBase = _enemyBase;
      this.stateMachine = _stateMachine;
      this.animBoolName = _animBoolName;
      this.enemyData = _enemyData;
   }

   public virtual void Enter()
   {
      rb = enemyBase.rb;
      triggerCalled = false;
      enemyBase.anim.SetBool(animBoolName,true);
   }

   public virtual void Exit()
   {
      enemyBase.anim.SetBool(animBoolName,false);
      enemyBase.AssignLastAnimName(animBoolName);
   }
   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
   }
   public virtual void AnimationFinishTrigger()
   {
      triggerCalled = true;
   }
}
