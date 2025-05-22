using UnityEngine;

public class PlayerLedgeClimbDown : PlayerGroundedState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;
    private bool isDropping;
    private bool jumpInput;
    private bool isTouchingCeiling;
    private new int xInput;
    private new int yInput;
    public PlayerLedgeClimbDown(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        playerData.isStaying = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.anim.SetBool("DropDown",false);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.LogWarning("enter ledge down");
        if (player.LedgeDownTriggerDetection.isTouchingLedgeDown)
        {
            player.SetVelocityX(0f);
            player.SetVelocityY(0f);

            // // 让玩家面向悬崖
            // if (player.IsFacingRight() && player.facingDirection != -1)
            // {
            //     Debug.LogWarning("is facing right"+player.facingDirection);
            //     // player.Flip();
            // }
            // else if (!player.IsFacingRight() && player.facingDirection != 1)
            // {
            //     Debug.LogWarning("is facing left"+player.facingDirection);
            //     player.Flip();
            // }
            
            // 设置玩家到达悬崖检测点（减去偏移）
            cornerPos = player.LedgeDownTriggerDetection.ledgePositionDown;

            startPos.Set(cornerPos.x - (player.facingDirection * playerData.ledgeDownStartOffset.x), cornerPos.y - playerData.ledgeDownStartOffset.y);
            stopPos.Set(cornerPos.x + (player.facingDirection * playerData.ledgeDownStopOffset.x), cornerPos.y + playerData.ledgeDownStopOffset.y);

            // 设置玩家位置
            player.transform.position = startPos;
            isDropping = false; // 开始悬崖攀爬前的初始状态
            playerData.isLedgeClimbDown = true; // 标记为悬崖攀爬状态
            player.SetVelocityX(0f);
            player.SetVelocityY(0f);
        }
    }
    public override void Exit()
    {
        base.Exit();
        Debug.LogWarning("exit ledge down");
        playerData.isStaying = false;
        if (isDropping)
        {
            
            player.anim.SetBool("DropDown", false);
            isDropping = false;
        }
        playerData.isLedgeClimbDown = false;
        player.LedgeDownTriggerDetection.ledgeCollider.offset = player.LedgeDownTriggerDetection.originalOffset; // Reset to original offset
        // 启用玩家控制
        // player.SetIsBusy(false);
        // // // 重置悬崖攀爬动画
        // // player.anim.SetBool("isClimbing", false);
    }
 public override void Update()
    {
        base.Update();
            yInput = player.inputController.norInputY;
            xInput = player.inputController.norInputX;
            jumpInput = player.inputController.runJumpInput;
            
            player.SetVelocityX(0f);
            player.SetVelocityY(0f);
            player.transform.position = startPos;
            if (isClimbingLedgeDown && playerData.isStaying && !isDropping)
            {
                Debug.LogWarning("is dropping");
                CheckForSpace();
                isDropping = true;
                player.anim.SetBool("DropDown", true);
            }
            else if (isClimbingLedgeUp && playerData.isStaying && !isDropping)
            {
                Debug.LogWarning("is climbing");
                player.transform.position = stopPos;
                stateMachine.ChangeState(player.idleState);
            }
            else if (jumpInput && !isDropping)
            {
                player.wallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.wallJumpState);
            }
        }
        
    
    public void SetDetectedPosition(Vector2 pos)
    {
        detectedPos = pos;
    }

    private bool CheckForSpace()
    {
        Debug.LogWarning("check for space");
        float capsuleRadius = playerData.standColliderSize.x / 2f;

        Vector2 rayOrigin = cornerPos 
                            + (Vector2.up * (capsuleRadius + playerData.ceilingCheckOffset))
                            + (Vector2.right * player.facingDirection * capsuleRadius);

        isTouchingCeiling = Physics2D.Raycast(
            rayOrigin,
            Vector2.up,
            playerData.standColliderSize.y - capsuleRadius,
            playerData.whatIsCeiling
        );

        Debug.Log("Is touching ceiling: " + isTouchingCeiling);
        player.anim.SetBool("IsTouchingCeiling", isTouchingCeiling);
        Debug.DrawRay(rayOrigin, Vector2.up * (playerData.standColliderSize.y - capsuleRadius), Color.yellow);

        return isTouchingCeiling;
    }
    
    public override void DoChecks()
    {
        base.DoChecks();
        
    }
}

