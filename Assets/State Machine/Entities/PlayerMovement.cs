using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Core {

    public AirState airState;
    public IdleState idleState;
    public RunState runState;
    public State kneelState;

    //movement properties
    public float acceleration;
    [Range(0f, 1f)]
    public float groundDecay;
    public float maxXSpeed;

    //variables
    public float xInput { get; private set; }
    public float yInput { get; private set; }


    void Start() {
        SetupInstances();
        machine.Set(idleState);

    }

    // Update is called once per frame
    void Update() {
        CheckInput();
        HandleJumpInput();
        SelectState();
    }

    void FixedUpdate() {
        HandleXMovement();
        ApplyFriction();
    }

    void SelectState() {
        if (groundSensor.grounded) {
            if (yInput < 0 && Mathf.Abs(xInput) < 0.1f) {
                machine.Set(kneelState);

            } else if (xInput == 0) {
                machine.Set(idleState);

            } else {
                machine.Set(runState);
            }

        } else {
            machine.Set(airState);
        }

        machine.state.Do();

    }


    void CheckInput() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void HandleXMovement() {
        if (Mathf.Abs(xInput) > 0) {

            //increment velocity by our accelleration, then clamp within max
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -maxXSpeed, maxXSpeed);
            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);

            FaceInput();
        }
    }

    void FaceInput() {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleJumpInput() {
        if (Input.GetButtonDown("Jump") && groundSensor.grounded) {
            body.linearVelocity = new Vector2(body.linearVelocity.x, airState.jumpSpeed);
        }
    }

    void ApplyFriction() {
        if (groundSensor.grounded && xInput == 0 && body.linearVelocity.y <= 0) {
            body.linearVelocity *= groundDecay;
        }
    }
}
