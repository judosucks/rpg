using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {
    public AnimationClip anim;
    public override void Enter() {
        animator.Play(anim.name);

    }

    public override void Do() {
        if (!core.groundSensor.grounded) {
            isComplete = true;
        }
    }

    public override void Exit() {

    }
}
