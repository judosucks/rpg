using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//demo NPC class which uses the state machine
//toggles between patrol and collect states
public class NPC : Core {

    public PatrolState patrol;
    public CollectState collect;
    // Start is called before the first frame update
    void Start() {
        SetupInstances();
        Set(patrol);
    }

    // Update is called once per frame
    void Update() {
        if (state.isComplete) {
            if (state == collect) {
                Set(patrol);
            }
        }

        if (state == patrol) {
            collect.CheckForTarget();
            if (collect.target != null) {
                Set(collect, true);
            }
        }

        state.DoBranch();
    }

    void FixedUpdate() {
        state.FixedDoBranch();
    }
}
