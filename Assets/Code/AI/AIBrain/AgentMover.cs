using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : Movement
{

    [SerializeField]
    public float maxSpeed = 2, acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }

    protected override void Awake()
    {
        Setup(GetComponent<Rigidbody2D>());
        base.Awake();
    }


    //protected override void FixedUpdate()
    //{
    //    if (MovementInput.magnitude > 0 && currentSpeed >= 0)
    //    {
    //        oldMovementInput = MovementInput;
    //        currentSpeed += acceleration * maxSpeed * Time.deltaTime;
    //    }
    //    else
    //    {
    //        currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
    //    }
    //    currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    //    _rigidbody.velocity = oldMovementInput * currentSpeed;

    //}


}
