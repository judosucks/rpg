using UnityEngine;

public class PlayerEdgeClimbState : PlayerGroundedState
{
    private Vector2 detectedEdge;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private bool isClimbing;
    private int xInput;
    private int yInput;
    public PlayerEdgeClimbState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.linearVelocity = Vector2.zero;
        player.transform.position = detectedEdge;
        // cornerPos = player.DetermineEdgeCornerPosition();
        startPos.Set(cornerPos.x - (player.facingDirection * playerData.startEdgeOffset.x), cornerPos.y - playerData.startEdgeOffset.y);
        stopPos.Set(cornerPos.x + (player.facingDirection * playerData.stopEdgeOffset.x), cornerPos.y + playerData.stopEdgeOffset.y);
        player.transform.position = startPos;
        isClimbing = false;

        playerData.isEdgeClimbState = true;
    }

    public override void Update()
    {
        base.Update();
        if (!isExitingState)
        {

            if (xInput != 0)
            {
                Debug.LogWarning("move from edge climb");
                stateMachine.ChangeState(player.moveState);
            }  
          if (triggerCalled)
          {
             stateMachine.ChangeState(player.idleState);
             Debug.LogWarning("idle from edge climb");
          }
            
        }
        // else
        // {
        //     
        //     if (xInput == player.facingDirection && !isClimbing)
        //     {
        //         Debug.Log("climbing");
        //         isClimbing = true;
        //         player.anim.SetBool("ClimbEdge", true);
        //     }
        //     else if (xInput != player.facingDirection && !isClimbing)
        //     {
        //         rb.AddForce(Vector2.right * playerData.exitSlideForce * -player.facingDirection, ForceMode2D.Impulse);
        //         stateMachine.ChangeState(player.standState);
        //         
        //     }
        // }
    }

    public override void Exit()
    {
        base.Exit();
        // if (isClimbing)
        // {
            player.transform.position = stopPos;
        //     isClimbing = false;
        // }
        playerData.isEdgeClimbState = false;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        xInput = player.inputController.norInputX;
        yInput = player.inputController.norInputY;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        
    }
    public void SetDetectedEdgePosition(Vector2 pos)
    {
        detectedEdge = pos;
    }
}
