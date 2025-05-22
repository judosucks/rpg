using UnityEngine;

public static class Settings
{
    public static readonly int IsThrowGrenade;
    public static readonly int IsCounterAttack;
    public static readonly int IsSuccessCounter;
    public static readonly int IsClimbLedge;
    public static readonly int IsComboCounter;
    public static readonly int IsRunJump;
    public static readonly int IsMove;
    public static readonly int IsDash;
    public static readonly int IsIdle;
    public static readonly int IsCrossKick;
    public static readonly int IsKneeKick;
    public static readonly int IsJump;
    public static readonly int IsYvelocity;
    public static readonly int IsRotation;
    public static readonly int IsAttack;
    public static readonly int IsSprint;
    
    static Settings()
    {
        IsThrowGrenade = Animator.StringToHash("AimGrenade");
           IsCounterAttack = Animator.StringToHash("CounterAttack");
          IsSuccessCounter  = Animator.StringToHash("SucessCounter");
          IsClimbLedge  = Animator.StringToHash("ClimbLedge");
           IsComboCounter = Animator.StringToHash("ComboCounter");
          IsDash  = Animator.StringToHash("Dash");
           IsRunJump = Animator.StringToHash("RunJump");
           IsMove = Animator.StringToHash("Move");
           IsIdle = Animator.StringToHash("Idle");
           IsCrossKick = Animator.StringToHash("CrossKick");
           IsJump = Animator.StringToHash("Jump");
          IsYvelocity  = Animator.StringToHash("yVelocity");
           IsKneeKick = Animator.StringToHash("KneeKick");
           IsAttack = Animator.StringToHash("Attack");
           IsSprint = Animator.StringToHash("Sprint");
            IsRotation= Animator.StringToHash("Rotation");    
    }
    
    
     
    
    
}
