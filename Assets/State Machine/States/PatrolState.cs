using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State {

    public NavigateState navigate;
    public IdleState idle;
    public Transform anchor1;
    public Transform anchor2;

    void GoToNextDestination() {

        Vector2 a1 = (Vector2)anchor1.position;
        Vector2 a2 = (Vector2)anchor2.position;
        //pick whichever one we aren't currently assigned to
        navigate.destination = navigate.destination == a1 ? a2 : a1;

        Set(navigate, true);
    }

    public override void Enter() {
        GoToNextDestination();
    }

    public override void Do() {
        if (machine.state == navigate) { //if we're navigating and we've reached our destination
            if (navigate.isComplete) {
                //stop
                Set(idle, true);
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
            }

        } else { //otherwise if we're idling and we've idled for a bit
            if (machine.state.time > 1) {
                GoToNextDestination();
            }
        }
    }
}
