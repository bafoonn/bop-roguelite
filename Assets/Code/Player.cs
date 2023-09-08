using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputReader _inputReader;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _movement;

    private void Awake()
    {
        _inputReader = this.AddOrGetComponent<InputReader>();
        _movement = this.AddOrGetComponent<PlayerMovement>();
        _rigidbody = this.AddOrGetComponent<Rigidbody2D>();
        _inputReader.DodgeCallback = () =>
        {
            _movement.Dodge(_inputReader.movement);
        };


        _movement.Setup(_rigidbody);
    }

    private void FixedUpdate()
    {
        _movement.Move(_inputReader.movement, Time.fixedDeltaTime);
    }
}
