using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;


public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Mouse mouse;
    protected PlayerData playerData;
    protected Rigidbody2D rb;
    protected Gamepad gamepad;
    protected int xInput;
  
    protected int yInput;
    private string animBoolName;
    protected float startTime;
    protected float stateTimer;
    protected bool triggerCalled;
    protected bool isFalling;//flag to track if player is falling
    protected bool isExitingState;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.playerData = _playerData;
        this.animBoolName = _animBoolName;
        rb = player.rb;
        if (rb == null)
        {
            Debug.LogError("Rigidbody is NULL when entering PlayerState");
        }
    }
    
    public virtual void Enter()
    {
     
       DoChecks();
       player.anim.SetBool(animBoolName, true);
      
       triggerCalled = false;
       mouse = Mouse.current;
       gamepad = Gamepad.current;
       startTime = Time.time;
       isExitingState = false;
     
       
       
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
       xInput = player.inputController.norInputX;
       yInput = player.inputController.norInputY;
       

    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void DoChecks()
    {
        if (player == null || rb == null)
        {
            Debug.LogError($"Player or rb is NULL when entering {this.GetType().Name}");
            return;
        }
        // player.SlopeCheck(); // 检测斜坡并更新状态
        //
        // // 根据斜坡状态动态调整摩擦力
        // if (player.isOnSlope && Mathf.Abs(rb.linearVelocity.x) < 0.01f && player.canWalkOnSlope)
        // {
        //    rb.sharedMaterial = player.FullFriction;
        // }
        // else
        // {
        //    rb.sharedMaterial = player.NoFriction;
        // }
        //detect if player is falling after moving upwards
        if (!isFalling && rb.linearVelocity.y <= 0f)
        {
            isFalling = true;
           
        }else if (rb.linearVelocity.y > 0f)//player still going up
        {
            isFalling = false;
        }
        playerData.highestPoint = player.transform.position.y;

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    public virtual void AnimationTrigger() { }
    
}