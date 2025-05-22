using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KneelState : State {
    public AnimationClip anim;
    public PlayerMovement input;
    public override void Enter() {
        animator.Play(anim.name);
    }

    public override void Do() {
        if (!core.groundSensor.grounded || input.yInput >= 0 || input.xInput != 0) {
            isComplete = true;
        }
    }

    public override void Exit() {

    }
}
