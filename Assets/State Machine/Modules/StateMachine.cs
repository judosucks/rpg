using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {
    //our state
    public State state;

    //tries to set a new state
    public void Set(State newState, bool forceReset = false) {
        if (state != newState || forceReset) {
            state?.Exit();
            state = newState;
            state.Initialise(this);
            state.Enter();
        }
    }

    //returns a list of all states in the active branch
    public List<State> GetActiveStateBranch(List<State> list = null) {
        if (list == null) {
            list = new List<State>();
        }

        if (state == null) {
            return list;
        } else {
            list.Add(state);
            return state.machine.GetActiveStateBranch(list);
        }
    }


}
