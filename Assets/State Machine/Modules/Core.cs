using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour {

    //blackboard instances
    public Rigidbody2D body;
    public Animator animator;
    public GroundSensor groundSensor;

    //our state machine
    public StateMachine machine;

    //wrappers to avoid having to call machine.state and its functions
    public State state => machine.state;

    protected void Set(State newState, bool forceReset = false) {
        machine.Set(newState, forceReset);
    }

    //assign this core to all of the states found in the scene hierarchy for this game object
    public void SetupInstances() {
        machine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates) {
            state.SetCore(this);
        }
    }

    //print out all of the active states in the tree, only in the editor
    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if (Application.isPlaying && state != null) {
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States: " + string.Join(" > ", states));
        }
#endif

    }

}
