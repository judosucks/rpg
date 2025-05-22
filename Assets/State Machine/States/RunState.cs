using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State {
    public AnimationClip anim;
    public float maxXSpeed;
    public override void Enter() {
        animator.Play(anim.name);
    }

    public override void Do() {
        float velX = body.linearVelocity.x;
        //play animator up to 1.5x speed based on how fast we're moving relative to max
        animator.speed = Helpers.Map(Mathf.Abs(velX), 0, maxXSpeed, 0, 1.5f, true);


        //if we leave the ground, finish
        if (!core.groundSensor.grounded) {
            isComplete = true;
        }
    }

}
