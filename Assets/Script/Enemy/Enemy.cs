using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : Entity
{
   
   [SerializeField] protected GameObject counterImage;
   [HideInInspector] public float lastTimeAttacked;
   
   public EnemyStateMachine stateMachine { get; private set; }
   public string lastAnimBoolName { get; private set; }

   protected override void Awake()
   {
      base.Awake();
      stateMachine = new EnemyStateMachine();
      enemyData.defaultMoveSpeed = enemyData.moveSpeed;
   }

   protected override void Update()
   {
      base.Update();
      // 检查 stateMachine 和 currentState 是否为 null
      if (stateMachine != null && stateMachine.currentState != null)
      {
         stateMachine.currentState.Update();
      }
      else
      {
         Debug.LogWarning("State machine or current state is not initialized.");
      }
      var playerHit = IsPlayerDetected();

      // 检查 RaycastHit2D 的 collider 是否为 null
      if (playerHit.collider != null)
      {
         
      }
      else
      {
         
      }
      
   }

   public virtual void AssignLastAnimName(string _animBoolName)
   {
      lastAnimBoolName = _animBoolName;
   }
   public virtual RaycastHit2D IsPlayerDetected()=>Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, enemyData.playerCheckDistance, enemyData.whatIsPlayer);

   protected override void OnDrawGizmos()
   {
      
      Gizmos.color = Color.red;
      Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + enemyData.ledgeCheckDistance, ledgeCheck.position.y));
      Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + enemyData.playerCheckDistance,wallCheck.position.y));
      Gizmos.DrawLine(wallBackCheck.position, new Vector3(wallBackCheck.position.x + enemyData.wallBackCheckDistance, wallBackCheck.position.y));
      Gizmos.DrawLine(groundCheck.position,
         new Vector3(groundCheck.position.x, groundCheck.position.y - enemyData.groundCheckDistance));
      Gizmos.DrawLine(wallCheck.position,
         new Vector3(wallCheck.position.x + enemyData.wallCheckDistance, wallCheck.position.y));
      Gizmos.DrawWireSphere(attackCheck.position,enemyData.attackCheckRadius);
      Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + enemyData.attackDistance * facingDirection,transform.position.y));
   }

   public virtual void FreezeTime(bool _timeFreeze)
   {
      if (_timeFreeze)
      {
         enemyData.moveSpeed = 0;
         anim.speed = 0;
      }
      else
      {
         enemyData.moveSpeed = enemyData.defaultMoveSpeed;
         anim.speed = 1;
      }
      
   }
   public virtual void FreezeTimeFor(float _duration)=>StartCoroutine(FreezeTimerCoroutine(_duration));
   protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
   {
      FreezeTime(true);
      yield return new WaitForSeconds(_seconds);
      FreezeTime(false);
   }

   public void ApplyKnockback(Vector2 force)
   {
      Debug.LogError("apply knockback");
      if (EnemyManager.instance.enemy.facingDirection == PlayerManager.instance.player.facingDirection)
      {
         rb.linearVelocity = new Vector2(force.x * facingDirection, force.y);
      }
      rb.linearVelocity = new Vector2(force.x * - facingDirection, force.y );
   }

   public virtual void OpenCounterAttackWindow()
   {
      enemyData.canBeStunned = true;
      counterImage.SetActive(true);
   }

   public virtual void CloseCounterAttackWindow()
   {
      enemyData.canBeStunned = false;
      counterImage.SetActive(false);
   }

   
   public virtual bool CanBeStunned()
   {
      if (enemyData.canBeStunned)
      {
         CloseCounterAttackWindow();
         return true;
      }
      return false;
   }
   public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
