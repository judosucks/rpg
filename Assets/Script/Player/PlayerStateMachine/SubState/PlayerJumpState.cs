using UnityEngine;

public class PlayerJumpState :PlayerAbilityState
{
    private int amountOfJumpsLeft;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine,PlayerData _playerData, string _animBoolName) : base(_player,
        _stateMachine,_playerData, _animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        
        playerData.isJumpState = true;
        player.SetColliderMaterial(player.noFrictionMaterial); // Set no friction in the air
        player.SetVelocityY( playerData.jumpForce);
        isAbilityDone = true;
        amountOfJumpsLeft--;
        player.airState.SetIsJumping();
    }

  
    public override void Exit()
    {
        base.Exit();
        playerData.isJumpState = false;
        player.isFallingFromJump = false;
        // if (!playerData.reachedApex)
        // {
        //     player.startFallHeight = player.transform.position.y;
        //     playerData.reachedApex = true;
        // }
        // Debug.LogWarning("startfallheight jump state: " + player.startFallHeight);

    }
    public override void Update()
    {
        base.Update();
       
       
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