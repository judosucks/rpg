using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;


[CreateAssetMenu(fileName ="newPlayerData",menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
 
    [Header("Collision info")] 
     public float attackCheckRadius;
     public float groundCheckDistance;
     public float wallCheckDistance;
  
     public float bottomGroundCheckDistance;
 
     public float headCheckDistance;
     public float ceilingCheckDistance;
     public LayerMask groundAndEdgeLayer;
     public LayerMask whatIsLedge;
     public LayerMask whatIsSlope;
     public LayerMask whatIsLadder;
     public LayerMask whatIsGround;
     public LayerMask whatIsEdge;
     public LayerMask whatIsWall;
     public LayerMask whatIsCeiling;
     public LayerMask whatIsAllLayer;
     public float slopeCheckDistance;

     public float wallBackCheckDistance;

     public float edgeCheckDistance;

     [Header("Gravity info")]
     public float gravityMultiplier;
     public float maxFallSpeed;
     public float fallForce;
     public float gravity = 9.81f;
     public float initialGravity = 0.5f;
     public float initialLinearDrag = 0.5f;
     [Header("wallslide info")] 
     public float wallSlideVelocity = 3f;
     public float wallSlideDownForce;
     public float climbUpForce;
     public bool isWallSliding;
     public float exitSlideForce =20f;
     [Header("climb state")]
     public float wallClimbVelocity = 3f;
     [Header("ledge info")]
     
     public bool isHanging = false;
     public bool isClimbLedge;
     
     [Header("throw grenade info")]
     public bool isThrowComplete;
     
     [Header("blackhole info")] 
     [Header("grenade info")]
     public bool isAiming;
     public bool isAimCheckDecided;
     public bool rightButtonLocked;
     public bool grenadeCanceled;
     public bool mouseButttonIsInUse;
     [Header("Attack Details")] 
     public Vector2[] attackMovement;
     public float counterAttackDuration = .2f;
     [Header("Movement jump info")] 
     public float movementSpeed = 2f;
     public float movementMiddleSpeed = 1.5f;
     public float movementSpeedReached = 5f;
     public float horizontalSpeed = 1f;
     public float verticalAirSpeed = 3f;
     public float straightJumpForce = 6f;
     public float jumpForce = 6f;
     public float jumpCutMultiplier = 0.5f;
     public float maxJumpHoldTime;
     public float sprintJumpForce = 5.5f;
     public float defaultSprintJumpForce = 5.5f;
     public float grenadeReturnImpact;
     public float defaultMoveSpeed = 2f;
     public float defaultJumpForce  = 6f;
     public float defaultStraightJumpForce  = 6f;
     public int amountOfJumps = 1;
     public float coyoteTime = 0.2f;
     public float variableJumpHeightMultiplier = 0.5f;
     public float airMovementSpeed = 1.6f;
     public float maxAirSpeed = 2f;
     public float sprintSpeed = 8f;
     public float defaultSprintSpeed = 8f;
     public float timeToSprint = 1.5f;
     [Header("Acceleration/Deceleration")]
     public float timeToMaxSpeed = 0.5f; // Time to reach max speed (in seconds)
     public float timeToZeroSpeed = 0.3f; // time to stop (in seconds)
     public float acceleration = 1f; // how fast to change speed
     public float deceleration = 1f; // how fast to change speed
     public float currentMoveSpeed; // keep track of current speed
     [Header("Crouch States")]
     public float crouchMovementSpeed = 0.6f; // 蹲下移动速度

     [Header("Collider Sizes")]
     public Vector2 crouchColliderSize = new Vector2(0.34f, 0.53f); // 蹲下状态碰撞体尺寸 (宽度, 高度)
     public Vector2 standColliderSize = new Vector2(0.34f, .93f);   // 站立状态碰撞体尺寸 (宽度, 高度)

     [Header("Collider Offsets")]
     public Vector2 crouchColliderOffset = new Vector2(0f, 0.27f); // 蹲下状态碰撞体偏移
     public Vector2 standColliderOffset = new Vector2(0f, 0.47f);   // 站立状态碰撞体偏移
     [Header("Raycast Settings")]
     
     public float ceilingCheckOffset = 0.015f; // 射线高度偏移
     [Header("snap grid info")] 
     public float gridSize = 0.16f;

     public float moveDistance = 2f;
     public float moveAlittleDistance = 0.2f;
     public float moveAlotDistance = 0.8f;
     public Vector2 moveDirection = Vector2.right;
     public float highFallDistance = 10f;
     public float fallLandDistance = 7f;
     [Header("clone info")] 
     public float closestEnemyCheckRadius = 8;

     public LayerMask whatIsEnemy;
     [Header("dash")]
     public float defaultDashSpeed  = 3f;
     public float dashSpeed = 3f;
     public float dashDuration=.3f;
     [Header("animation speed")] 
     public float moveStartAnimSpeed=0.8f;
     [Header("status info")]
     public bool isRun;
     public bool isIdle;
     public bool isSprint;
     public bool isInAir;
     public bool isJumpState;
     public bool isHighFallLandState;
     public bool isFallLandState;
     public bool isSprintJumpState;
     public bool isGroundedState;
     public bool isWallSlidingState;
     public bool isClimbLedgeState;
     public bool isGrenadeState;
     public bool isCounterAttackState;
     public bool isBlackholeState;
     public bool isRunJumpLandState;
     public bool isSprintJumpLandState;
     public bool isEdgeClimbState;
     public bool isLedgeClimbState;
     public bool isCrouch;
     public bool isCrouchMoveState;
     public bool isCrouchIdleState;
     public bool isSlopeClimbState;
     public bool isLedgeClimbDown;
     public bool isWalk;
     public bool isInteract;
     [Header("highest jump")] 
     public bool reachedApex;

     public float highestPoint = 0f;
     public readonly float defaultHighestPoint = 0f;
     [Header("stick on ground")] 
     public float stickingForce = 20f;

     public float maxPushForce = 20f;
     [Header("fall info")] 
     public float fallThreshold = 10f;

     public bool isFalling;
     [Header("slope info")] 
     [Header("Slope Settings")]
     public float slopeSlidingSpeed = 5f; // 静止时滑动速度
     public float slopeStandingSlideSpeed = 3f; // 轻微坡度时滑动速度
     public float slopeFrictionMultiplier = 0.5f; // 上坡时的减速比例
     [Header("ledgeclimbdown info")] 
     public bool isStaying;

     [Header("ledge info")] 
     public float ledgeDistance = 1f;
     public Vector2 ledgeDownStartOffset;
        public Vector2 ledgeDownStopOffset;
     [Header("Wall Jump Info")]
     public float wallJumpVelocity = 6f;
     public float wallJumpTime = 0.4f;
     public Vector2 wallJumpAngle = new Vector2(1, 2);
     [Header("ledge edge offset info")]
     public Vector2 startOffset;
     public Vector2 stopOffset;
     public Vector2 startEdgeOffset;
     public Vector2 stopEdgeOffset;
     [Header(("grenade explode fx damage info"))]
     public float explosionRadius = 1f;
     [Header("Jump Settings")]
     [Range(0.1f, 0.5f)] public float minJumpCutMultiplier = 0.3f;
     [Range(0.7f, 1f)] public float maxJumpCutMultiplier = 0.95f;
     private void OnEnable()
     {
         isWalk = false;
         isInteract = false;
         isFalling = false;
         isCrouch = false;
         isLedgeClimbDown = false;
         isSprintJumpLandState = false;
         isSprintJumpState = false;
         isCrouchIdleState = false;
         isCrouchMoveState = false;
         isGroundedState = false;
         isRun=false;
         isIdle=false;
         isHighFallLandState=false;
         isFallLandState=false;
         isSprint=false;
         isInAir=false;
         isJumpState=false;
         isWallSlidingState=false;
         isClimbLedgeState=false;
         isGrenadeState=false;
         isCounterAttackState = false;
         isBlackholeState=false;
         isRunJumpLandState=false;
         isEdgeClimbState=false;
         isLedgeClimbState=false;
         isSlopeClimbState=false;
         highestPoint = defaultHighestPoint;
         movementSpeed = defaultMoveSpeed;
         jumpForce = defaultJumpForce;
         straightJumpForce = defaultStraightJumpForce;
         dashSpeed = defaultDashSpeed;
         sprintSpeed = defaultSprintSpeed;
         sprintJumpForce = defaultSprintJumpForce;
         Debug.Log("player data enabled");
     }
}