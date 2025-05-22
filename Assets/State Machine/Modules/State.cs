using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {

    protected float startTime;
    public bool isComplete { get; protected set; }
    public float time => Time.time - startTime;

    //blackboard variables
    protected Core core;
    protected Rigidbody2D body => core.body;
    protected Animator animator => core.animator;

    //our state machine
    public StateMachine machine;

    //the state machine that called us
    protected StateMachine parent;

    //wrappers to avoid having to call machine.state and its functions
    public State state => machine.state;
    //set our child state
    protected void Set(State newState, bool forceReset = false) {
        machine.Set(newState, forceReset);
    }

    //assign the core and initialise our state machine for potential child states.
    public void SetCore(Core _core) {
        machine = new StateMachine();
        core = _core;
    }

    //prepare state for its next usage
    public void Initialise(StateMachine _parent) {
        parent = _parent;
        isComplete = false;
        startTime = Time.time;
    }


    //functions for states to override
    public virtual void Enter() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }
    public virtual void Exit() { }


    //calls all of the "Do" functions in the active branch
    public void DoBranch() {
        Do();
        state?.DoBranch();
    }

    //calls all of the "FixedDo" functions in the active branch
    public void FixedDoBranch() {
        FixedDo();
        state?.FixedDoBranch();
    }

}
