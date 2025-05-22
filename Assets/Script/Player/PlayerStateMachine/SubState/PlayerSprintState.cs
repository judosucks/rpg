using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using Unity.VisualScripting;

public class PlayerSprintState : PlayerGroundedState
{
   
    private bool sprintInput;
    private bool isSprinting;
    public PlayerSprintState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        Debug.Log("Player is sprinting");
        playerData.isSprint = true;
    }

    public override void Exit()
    {
        base.Exit();
        
        playerData.isSprint = false;
        isSprinting = false;
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputController.norInputX;
        sprintInput = player.inputController.sprintInput;
        player.CheckIfShouldFlip(xInput);
        if (!isExitingState)
        {
            if ( xInput == 0)
            {
                
                isSprinting = false;    
                stateMachine.ChangeState(player.idleState);
                
                
            }
            
            // 检查是否按下左键进行膝击
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                stateMachine.ChangeState(player.kneeKickState);
            }
            else
            {
               player.SetVelocityX(xInput * playerData.sprintSpeed);
               isSprinting = true;
               // player.StartCoroutine(FinishSprint(2f));
            }
        }
        // player.SetVelocity(xInput * playerData.movementSpeed, rb.linearVelocity.y);
    }
        private IEnumerator FinishSprint(float duration)
        {
            yield return new WaitForSeconds(duration);
            isSprinting = false;
            stateMachine.ChangeState(player.moveState);
        }

}