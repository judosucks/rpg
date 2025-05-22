using UnityEngine;

public class PlayerSprintJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;
    public PlayerSprintJumpState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.inputController.UseSprintJumpInput();
        playerData.isSprintJumpState = true;
        player.SetVelocityY( playerData.sprintJumpForce);
        isAbilityDone = true;
        amountOfJumpsLeft--;
        player.airState.SetIsJumping();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.isFallingFromJump = false;
        playerData.isSprintJumpState = false;
        // if (!playerData.reachedApex)
        // {
        //     player.startFallHeight = player.transform.position.y;
        //     playerData.reachedApex = true;
        // }
        // Debug.LogWarning("startfallheight sprint jump state: " + player.startFallHeight);
    }
    public bool CanJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumps()
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }
    public void DecrementAmountOfJumpsLeft()=> amountOfJumpsLeft--;
}