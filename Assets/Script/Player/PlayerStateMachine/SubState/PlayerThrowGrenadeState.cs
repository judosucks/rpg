
using System.IO;
using Unity.Cinemachine;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem; // Add this for mouse input

public class PlayerThrowGrenadeState : PlayerAbilityState
{
    private NewCamera newCamera;
    private CinemachinePositionComposer positionComposer;

    private CinemachineCamera currentCam;
    // Add a mouse variable
    private Mouse mouse;

    public PlayerThrowGrenadeState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
        // Initialize the mouse variable in the constructor
        mouse = Mouse.current;
    }

    public override void Enter()
    {
        base.Enter();
        playerData.isGrenadeState = true;

       
            newCamera = CameraManager.instance.newCamera;
            if (newCamera != null)
            {
                currentCam = CameraManager.instance.GetCurrentActiveCamera();
                if (currentCam != null)
                {
                    positionComposer = currentCam.GetComponent<CinemachinePositionComposer>();
                    if (positionComposer != null)
                    {
                        newCamera.temporaryScreenX = positionComposer.Composition.ScreenPosition.x;
                    }
                    else
                    {
                        Debug.LogError("no CinemachinePositionComposer found");
                    }
                }
                else
                {
                    Debug.LogError("currentCam not found");
                }
              
            }
            else
            {
                Debug.LogError("NewCamera not found");
            }
            
            
           
            
        

        

        if (!playerData.isAiming && playerData.grenadeCanceled)
        {
            Debug.Log("grenade canceled not aiming");
            player.skill.grenadeSkill.DotsActive(false);
            if (player.anim.GetBool("AimGrenade"))
            {
                Debug.Log("grenade canceled not aiming");
                player.anim.SetBool("AimGrenade", false);
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        rb.linearVelocity = Vector2.zero; // Use rb.velocity instead of rb.linearVelocity

        if (mouse.rightButton.isPressed)
        {
            if (playerData.isAiming)
            {
                player.skill.grenadeSkill.DotsActive(true);
                Debug.Log("right button pressed from throw grenade");
                UpdateTargetScreenX();
                SmoothCameraMove();
                CameraManager.instance.newCamera.AdjustCameraScreenX(newCamera.temporaryScreenX,newCamera.smoothTime);
                
            }
        }

        if (mouse.rightButton.wasReleasedThisFrame && playerData.isAiming)
        {
            Debug.Log("right button releaseed isaim");
            return;
        }
        if (mouse.rightButton.wasReleasedThisFrame && !playerData.isAiming)
        {
            Debug.Log("right mouse button was released change to idle state");
            stateMachine.ChangeState(player.idleState);
        }

            Vector2 mousePositon = mouse.position.ReadValue();
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePositon);
            if(Camera.main == null)
            {
                Debug.LogError("Camera.main is null");
                return;
            }
            if (player.transform.position.x > mouseWorldPosition.x && player.facingDirection == 1)
           {
              player.Flip();
           }else if (player.transform.position.x < mouseWorldPosition.x && player.facingDirection == -1)
           {
               player.Flip();
           }
        

        
    }


    private void UpdateTargetScreenX()
    {
        // 根据玩家面向调整目标 ScreenX
        if (player.facingDirection == 1)
        {
            Debug.LogWarning("playerfacingdirection is 1");
            // positionComposer.Composition.ScreenPosition.x = -0.4f;
            newCamera.targetScreenX = 0.25f; // 向右偏移，玩家在屏幕左侧
        }
        else if(player.facingDirection == -1)
        {
            Debug.LogWarning("playerfacingdirection is -1");
            // positionComposer.Composition.ScreenPosition.x = 0.4f;
            newCamera.targetScreenX = 0.75f; // 向左偏移，玩家在屏幕右侧
        }
    }

    // private void SmoothCameraMove()
    // {
    //     // 平滑过渡到目标 ScreenX
    //     positionComposer.Composition.ScreenPosition.x = Mathf.SmoothDamp(
    //         newCamera.temporaryScreenX, 
    //         positionComposer.Composition.ScreenPosition.x, 
    //         ref newCamera.currentVelocity, 
    //         newCamera.smoothTime);
    // }
    private void SmoothCameraMove()
    {
        Debug.LogWarning("smooth");
        // 平滑过渡到目标 ScreenX
        newCamera.temporaryScreenX = Mathf.SmoothDamp(
            newCamera.temporaryScreenX, 
            newCamera.targetScreenX, 
            ref newCamera.currentVelocity, 
            newCamera.smoothTime);
    }
    public override void Exit()
    {
        base.Exit();
        Debug.LogWarning("exit grenade state");

        player.skill.grenadeSkill.DotsActive(false);
        player.skill.grenadeSkill.ResetGrenadeState();
        CameraManager.instance.newCamera.ResetZoom();
        //Reset aiming values when exiting
            player.anim.SetBool("AimGrenade", false);
         
        player.StartCoroutine(player.BusyFor(0.5f));
        playerData.isGrenadeState = false;
        // stateMachine.ChangeState(player.idleState);
        
        
        
       
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        player.SetIsBusy(true);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.SetIsBusy(false);
        isAbilityDone = true;
    }
}


   
