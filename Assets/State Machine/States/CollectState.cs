using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : State {
    public List<Transform> souls;

    //check for any souls in the vicinity
    public Transform target;

    //if you find one, run over to it.
    public NavigateState navigate;

    //idle for one second afterward, then complete;
    public IdleState idle;

    public float collectRadius;

    public float vision = 1;

    public override void Enter() {
        navigate.destination = target.position;
        Set(navigate, true);
    }

    public override void Do() {

        if (state == navigate) {
            ChaseTarget();
        } else {
            EndPursuit();
        }

        //if we have no target at all, exit
        if (target == null) {
            isComplete = true;
            return;
        }

    }

    void ChaseTarget() {
        if (IsWithinReach(target.position)) {//if the target is close enough, stop and collect it
            Set(idle, true);
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
            target.gameObject.SetActive(false);

        } else if (!IsInVision(target.position)) { //if the target is out of vision, stop and idle
            Set(idle, true);
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);

        } else { //otherwise, keep chasing
            navigate.destination = target.position;
            Set(navigate, true);
        }
    }

    //if we've idled for a bit, we're done
    void EndPursuit() {
        if (state.time > 2) {
            isComplete = true;
        }
    }

    public bool IsWithinReach(Vector2 targetPos) {
        return Vector2.Distance(core.transform.position, targetPos) < collectRadius;
    }

    public bool IsInVision(Vector2 targetPos) {
        return Vector2.Distance(core.transform.position, targetPos) < vision;
    }

    public void CheckForTarget() {
        foreach (Transform soul in souls) {
            if (IsInVision(soul.position) && soul.gameObject.activeSelf) {
                target = soul;
                return;
            }
        }

        target = null;
    }

}
