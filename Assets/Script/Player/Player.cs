using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class Player : Entity
{
    
    
    public EntityFX entityFX;
    public HeadDetection headDetection;
    public SkillManager skill { get; private set; }
    public GameObject grenade { get; private set; }
    public PlayerInput playerInput;
    public PlayerColliderManager colliderManager;  // 添加碰撞管理功能
    
    
    public bool isBusy { get; private set; } // 私有字段

    public void SetIsBusy(bool value) // 公开方法设置属性
    {
        isBusy = value;
    }
    public bool isAttacking { get; set; } // 公开属性，用于指示玩家当前是否处于攻击状态
    public bool isFalling { get; private set; }
    public void SetIsFalling(bool value)
    {
        isFalling = value;
    }
    public void ResetStartFallHeight()
    {
        startFallHeight = -1000f; // 重置起始高度
    }
   
    

    #region state variable

    

   
    public PlayerInputController inputController { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    
    
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    
    public PlayerSlopesClimbState slopeClimbState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerCrouchMoveState crouchMoveState { get; private set; }
    public PlayerRunJumpLandState runJumpLandState { get; private set; }
    public PlayerSprintJumpInAirState sprintJumpInAirState { get; private set; }
    public PlayerSprintJumpState sprintJumpState { get; private set; }
    public PlayerSprintJumpLandState sprintJumpLandState { get; private set; }
    public PlayerLedgeClimbDown ledgeClimbDown { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerCrossKickState crossKickState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerSprintState sprintState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerEdgeClimbState edgeClimbState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerThrowGrenadeState throwGrenadeState { get; private set; }
    
    public PlayerBlackholeState blackholeState { get; private set; }
   
    public PlayerKneeKickState kneeKickState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerStandState standState { get; private set; }

    public PlayerFallLandState fallLandState { get; private set; }
    public PlayerHighFallLandState highFallLandState { get; private set; }
    #endregion
    #region awake
    protected override void Awake()
    {
        base.Awake();
        
        stateMachine = new PlayerStateMachine();
        playerInput = GetComponent<PlayerInput>();
        inputController = GetComponent<PlayerInputController>();
        entityFX = GetComponent<EntityFX>();
        colliderManager = GetComponent<PlayerColliderManager>(); // 确保在 Player 上挂载
        leftEdgeTrigger.isNearLeftEdge = true;
        rightEdgeTrigger.isNearRightEdge = true;
        sprintJumpState = new PlayerSprintJumpState(this, stateMachine, playerData, "SprintJump");
        fallLandState = new PlayerFallLandState(this, stateMachine, playerData, "FallLand");
        standState = new PlayerStandState(this, stateMachine, playerData, "Stand");
        sprintJumpInAirState = new PlayerSprintJumpInAirState(this, stateMachine, playerData, "SprintJump");
        sprintJumpLandState = new PlayerSprintJumpLandState(this, stateMachine, playerData, "SprintJumpLand");
        runJumpLandState = new PlayerRunJumpLandState(this, stateMachine, playerData, "Land");
        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, playerData, "CrouchIdle");
        crouchMoveState = new PlayerCrouchMoveState(this, stateMachine, playerData, "CrouchMove");
        ledgeClimbDown = new PlayerLedgeClimbDown(this, stateMachine, playerData, "LedgeClimbDown");
        sprintState = new PlayerSprintState(this, stateMachine,playerData, "Sprint");
        crossKickState = new PlayerCrossKickState(this, stateMachine, playerData,"CrossKick");
        edgeClimbState = new PlayerEdgeClimbState(this, stateMachine, playerData,"ClimbEdgeState");
        slopeClimbState = new PlayerSlopesClimbState(this, stateMachine, playerData,"ClimbSlopeState");
        idleState = new PlayerIdleState(this, stateMachine, playerData,"Idle");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, playerData,"WallGrab");
        moveState = new PlayerMoveState(this, stateMachine, playerData,"Move");
        jumpState = new PlayerJumpState(this, stateMachine, playerData,"RunJump");
        airState = new PlayerAirState(this, stateMachine, playerData,"RunJump");
        dashState = new PlayerDashState(this, stateMachine,playerData, "Dash");
        wallJumpState = new PlayerWallJumpState(this, stateMachine,playerData, "RunJump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerData,"WallSlide");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, playerData,"Attack");
        highFallLandState = new PlayerHighFallLandState(this, stateMachine, playerData,"HighFallLand");
        ledgeClimbState = new PlayerLedgeClimbState(this,stateMachine,playerData,"LedgeClimbState");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, playerData,"CounterAttack");
        kneeKickState = new PlayerKneeKickState(this, stateMachine, playerData,"KneeKick");
        throwGrenadeState = new PlayerThrowGrenadeState(this, stateMachine, playerData,"AimGrenade");
        blackholeState = new PlayerBlackholeState(this, stateMachine,playerData, "Blackhole");
        climbState = new PlayerClimbState(this, stateMachine,playerData, "Climb");
        deadState = new PlayerDeadState(this, stateMachine,playerData, "Dead");
        hurtState = new PlayerHurtState(this, stateMachine, playerData,"Hurt");
    }
    
    #endregion
    protected override void Start()
    {
        EnsureSkillManagerIsInitialized(); // 确保初始化
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        // CanWalkOnSlope(true); // 在初始化时默认允许行走
        // SlopeCheck();          // 初次检测坡地
    }
    private void EnsureSkillManagerIsInitialized()
    {
        if (SkillManager.instance == null)
        {
            SkillManager.instance = Object.FindFirstObjectByType<SkillManager>();
            if (SkillManager.instance == null)
            {
                Debug.LogError("SkillManager instance could not be initialized.");
            }
        }
    }
    protected override void Update()
    {
       
        
        stateMachine?.currentState?.Update();
        
        
        if (isBusy)
        { 
            return; // 如果玩家处于忙碌状态，禁止其他输入
        }

        if (isKnocked)
        {
            stateMachine.ChangeState(hurtState);
        }
        DashInput(); // 冲刺输入处理

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Inventory.instance.UseFlask();
        }
        
    }
    
    public override void SlowEntityBy(float _slowPercent, float _slowDuration)
    {
        playerData.movementSpeed = playerData.movementSpeed * (1 - _slowPercent);
        playerData.dashSpeed = playerData.dashSpeed * (1 - _slowPercent);
        playerData.jumpForce = playerData.jumpForce * (1 - _slowPercent);
        playerData.straightJumpForce = playerData.straightJumpForce * (1 - _slowPercent);
        playerData.sprintSpeed = playerData.sprintSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);
        Invoke("ReturnDefaultSpeed", _slowDuration);
        
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        playerData.movementSpeed = playerData.defaultMoveSpeed;
        playerData.dashSpeed = playerData.defaultDashSpeed;
        playerData.jumpForce = playerData.defaultJumpForce;
        playerData.straightJumpForce = playerData.defaultStraightJumpForce;
        playerData.sprintSpeed = playerData.defaultSprintSpeed;

    }
    public void AssignNewGrenade(GameObject _newGrenade)
    {
        if (grenade)
        {
            Destroy(_newGrenade);
            return;
        }
        grenade = _newGrenade;
    }

    public void ClearGrenade()
    {
        if (grenade == null) {
            Debug.LogWarning("No grenade to clear.");
            return;
        }

        Debug.Log("[ClearGrenade] Destroying grenade after delay...");
        StartCoroutine(DestroyGrenadeAfterDelay(.1f));
    }

    private IEnumerator DestroyGrenadeAfterDelay(float delay)
    {
        if (grenade == null) {
            yield break;
        }

        yield return new WaitForSeconds(delay);

        if (grenade != null) {
            Destroy(grenade);
            grenade = null;
        }
    }
    public void FallDownForceAndCountdown(float duration)
    {
        // 停止移动并清除当前速度
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // 添加向下的力
        rb.AddForce(Vector2.down * playerData.maxPushForce, ForceMode2D.Impulse);

        // 对齐地图网格
        SnapToGridSize();

        // 开始冻结逻辑
        StartCoroutine(FreezeOnGround(duration));
    }

    private IEnumerator FreezeOnGround(float duration)
    {
        isBusy = true;
        Debug.Log("Player is frozen: " + isBusy);

        // 暂时将刚体转换为静态
        rb.bodyType = RigidbodyType2D.Static;
        

        // 等待冻结时间
        yield return new WaitForSeconds(duration);

        // 恢复动态物理行为
        rb.bodyType = RigidbodyType2D.Dynamic;
        isBusy = false;
        

        Debug.Log("Player is unfrozen: " + isBusy);
    }
    

    public void CancelThrowGrenade()
    {
       
        playerData.grenadeCanceled = true;
        // Destroy grenade object
        if (grenade != null)
        {
            Destroy(grenade);
            grenade = null;
        }
        
        SkillManager.instance.grenadeSkill.DotsActive(false);

        // Reset player states
        OnAimingStop();
        if (anim.GetBool("AimGrenade"))
        {
          anim.SetBool("AimGrenade", false);
          anim.SetTrigger("AimAbort");
        }
        
        stateMachine.ChangeState(idleState);
        if (!anim.GetBool("Idle"))
        {
            anim.SetBool("Idle",true);
        }

    }


    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
        
    }

    
    
    private void DashInput()
    {
       
        if (isBusy || IsWallDetected() || !IsGroundDetected())
        {
            
            return;
        }

        if (skill.dashSkill.dashUnlocked == false)
        {
            
            return;
        }


        if ((Keyboard.current.fKey.wasPressedThisFrame && skill.dashSkill.CanUseSkill()) || (Gamepad.current != null &&
                Gamepad.current.buttonEast.wasPressedThisFrame && skill.dashSkill.CanUseSkill()))
        {

            stateMachine.ChangeState(dashState);
        }

    }
 

    
    
    public void AnimationFinishTrigger()
    {
        playerData.isRunJumpLandState = false;
        playerData.isSprintJumpLandState = false;
        playerData.isFallLandState = false;
        playerData.isHighFallLandState = false;
        playerData.isLedgeClimbState = false;
        playerData.isEdgeClimbState = false;
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }
    
    
    public void OnAimingStart()
    {
        playerData.isAiming = true;
        Debug.Log("isaiming is set to true");
    }

    public void OnAimingStop()
    {
        Debug.Log("isaiming is set to false");
        playerData.isAiming = false;
    }

    
  


   
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    // protected override void OnDrawGizmos()
    // { 
    //     if (LedgeTriggerDetection.isTouchingLedge)
    //     { 
    //         Gizmos.color = Color.green; 
    //         Gizmos.DrawWireSphere(LedgeTriggerDetection.ledgePosition, 0.1f); // 显示悬崖检测点
    //     }
    // }
}