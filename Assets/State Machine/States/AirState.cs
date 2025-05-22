using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State {
    public AnimationClip anim;

    public float jumpSpeed;

    public override void Enter() {
        animator.Play(anim.name);
    }

    public override void Do() {
        //seek the animator to the frame based on our y velocity
        float time = Helpers.Map(body.linearVelocity.y, jumpSpeed, -jumpSpeed, 0, 1, true);
        animator.Play(anim.name, 0, time);
        animator.speed = 0;

        if (core.groundSensor.grounded) {
            isComplete = true;
        }
    }

    public override void Exit() {

    }
}
