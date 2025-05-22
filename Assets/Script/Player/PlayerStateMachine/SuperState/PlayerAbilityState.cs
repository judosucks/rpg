using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    private bool isGrounded;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.IsGroundDetected();
    }

    public override void Enter()
    {
        base.Enter();
        
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (isAbilityDone)
        {
            if (isGrounded && rb.linearVelocity.y < 0.01f)
            {
                Debug.Log("grounded from ability state");
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                Debug.Log("air from ability state");
                stateMachine.ChangeState(player.airState);
            }
        }
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}