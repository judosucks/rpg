﻿using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
   private Player player => GetComponent<Player>();
   private PlayerInputActions playerInputActions;
   private PlayerData playerData => player.playerData;
   public Vector2 RawMovementInput;
   public int norInputX;
   public int norInputY;
   public bool runInput { get; private set; }
   public bool runJumpInput { get; private set; }
   public bool sprintJumpInput { get; private set; }
   public bool sprintInput { get; private set; }
   public bool grabInput { get; private set; } = false;
   public bool jumpInputStop { get; private set; }
   public bool isTouchingWall; // 是否正在触碰墙的标志位
   public bool isCrouchInput { get; private set; }
   public bool isClimbLedgeDown { get; private set; }
   public bool isClimbLedgeUp { get; private set; }
[SerializeField] private float runJumpInputHoldTime = 0.2f;
   [SerializeField] private float sprintJumpInputHoldTime = 0.2f;
   private float runJumpInputStartTime;
   private float sprintJumpInputStartTime;
   private bool isRun;
   private bool isIdle;
   private bool isSprint;
   private bool isCrouch;
   public Vector3 moveDirection; 
   private void Awake()
   {
      playerInputActions = new PlayerInputActions();
   }

   private void OnEnable()
   {
      if (playerInputActions != null)
      {
         playerInputActions.Player.Move.performed += OnMovement;
         playerInputActions.Player.Move.canceled += OnMovement;
         playerInputActions.Player.WallGrab.performed += OnGrabInputPerformed; // 绑定事件
         playerInputActions.Player.WallGrab.Enable();
         playerInputActions.Player.Move.Enable();
      }
   }

   private void OnDisable()
   {
      if (playerInputActions != null)
      {
         playerInputActions.Player.Move.performed -= OnMovement;
         playerInputActions.Player.Move.canceled -= OnMovement;
         playerInputActions.Player.WallGrab.performed -= OnGrabInputPerformed;
         // 绑定事件
         playerInputActions.Player.WallGrab.Disable();
         playerInputActions.Player.Move.Disable();
      }
   }

   private void Update()
   {
      isRun = playerData.isRun;
      isIdle = playerData.isIdle;
      isSprint = playerData.isSprint;
      isCrouch = playerData.isCrouch;
      if (player.isBusy)
      {
         Debug.Log("Player is busy from input");
         return;
      }
      if (norInputX != 0 && isSprint)
      {
         
         CheckSprintJumpInputHoldTime();
      }



      if (norInputX != 0 || norInputX == 0)
      {
         CheckRunJumpInputHoldTime();
      }
         
      
      if (!player.isTouchingWall && grabInput)
      {
         grabInput = false; // 玩家离开墙壁，重置抓取状态
         Debug.Log("Player left the wall, grabInput reset to false.");
         ReleaseInput(); // 调用释放逻辑
      }

        
   
      
}

   public void OnMovement(InputAction.CallbackContext context)
   {
        
            RawMovementInput = context.ReadValue<Vector2>();
         if (Mathf.Abs(RawMovementInput.x) > 0.5f)
         {
            norInputX = Mathf.RoundToInt((RawMovementInput * Vector2.right).normalized.x);
         }
         else
         {
            norInputX = 0;
         }

         if (Mathf.Abs(RawMovementInput.y) > 0.5f)
         {
           norInputY = Mathf.RoundToInt((RawMovementInput * Vector2.up).normalized.y);
         }
         else
         {
            norInputY = 0;
         }
         // 示例：根据输入控制玩家位置移动
         if (context.performed)
         {
            runInput = true; // Set runInput to true when movement input is pressed
         }
         else if (context.canceled)
         {
            runInput = false; // Reset runInput when the movement input is released
         }
   
   }

   public void OnSprintInput(InputAction.CallbackContext context)
   {
      if (context.performed && playerData.isRun)
      {
         
         sprintInput = true;
         
      }

      if (context.canceled)
      {
         UseSprintInput();
      }
   }

   public void OnCouchInput(InputAction.CallbackContext context)
   {
      
      if (context.started && !player.LedgeDownTriggerDetection.isTouchingLedgeDown)
      {
         
         isCrouchInput = !isCrouchInput;
      }
   }
   public void OnClimbLedgeDownInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         isClimbLedgeDown = true;
      }
      if(context.canceled)
         isClimbLedgeDown = false;
   }
   public void OnLedgeClimbUpInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         isClimbLedgeUp = true;
      }
      if(context.canceled)
         isClimbLedgeUp = false;
   }
   
   public void OnGrabInputPerformed(InputAction.CallbackContext context)
   {
      // 检查玩家是否接触到墙壁
      if (!player.isTouchingWall)
      {
         Debug.Log("Cannot grab! Player is not touching the wall.");
         return; // 如果没有接触墙壁，则直接返回
      }

// 只有在按键触发（context.started）时切换 grabInput 的状态
      if (context.started)
      {
         grabInput = !grabInput; // 切换状态

         if (grabInput)
         {
            // grabInput 为 true 时执行
            OnGrabInput();
         }
         else
         {
            // grabInput 为 false 时执行
            ReleaseInput();
         }
      }

   }

   private void OnGrabInput()
   {
      grabInput = true;
      // Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标
      // Cursor.visible = false; // 隐藏鼠标光标
      Debug.Log("Input grabbed");
   }

   private void ReleaseInput()
   {
      grabInput = false;
      // Cursor.lockState = CursorLockMode.None; // 释放鼠标锁定
      // Cursor.visible = true; // 显示鼠标光标
      Debug.Log("Input released");
   }

   public void OnRunJumpInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         
         runJumpInput = true;
         jumpInputStop = false;
         runJumpInputStartTime = Time.time;
      }

      if (context.canceled)
      {
         jumpInputStop = true;
      }

      
   }

   public void OnSprintJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isSprint)
      {
         Debug.Log("Sprint Jump Input");
         
         sprintJumpInput = true;
         sprintJumpInputStartTime = Time.time;
      }

      
   }

   public void UseRunJumpInput()
   {
      
      runJumpInput = false;
      
   }


   public void UseSprintJumpInput()
   {
      Debug.Log("input false");
      sprintJumpInput = false;
      
   }

   
   public void UseSprintInput()
   {
      sprintInput = false;
      Debug.Log("UseSprintInput");
   }

   public void UseCrouchInput()
   {
      isCrouchInput = false;
   }
   public void UseLedgeClimbDownInput()
   {
      isClimbLedgeDown = false;
   }
public void CancelAllJumpInput()
   {
      UseRunJumpInput();
      UseSprintJumpInput();
   }
   
   private void CheckRunJumpInputHoldTime()
   {
      if (Time.time >= runJumpInputStartTime + runJumpInputHoldTime)
      {
         float time = runJumpInputStartTime + runJumpInputHoldTime;
         runJumpInput = false;
         
      }
   }
   

   private void CheckSprintJumpInputHoldTime()
   {
      if (Time.time >= sprintJumpInputStartTime + sprintJumpInputHoldTime)
      {
         float time = sprintJumpInputStartTime + sprintJumpInputHoldTime;
         sprintJumpInput = false;
         
      }
   }
}