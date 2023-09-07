using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputReader _inputReader;
    private Mover _mover;
    private Rigidbody2D _rigidbody;
    [SerializeField] private Motion _basicMotion;
    [SerializeField] private Motion _dodgeMotion;

    private void Awake()
    {
        _inputReader = this.AddOrGetComponent<InputReader>();
        _mover = this.AddOrGetComponent<Mover>();
        _rigidbody = this.AddOrGetComponent<Rigidbody2D>();
        _inputReader.DodgeCallback += () =>
        {
            _mover.AddMotion(_dodgeMotion);
        };

        _rigidbody.gravityScale = 0;

        _dodgeMotion.Type = Motion.MotionType.Single;

        _mover.Setup(_rigidbody);
        _mover.AddMotion(_basicMotion);
    }

    private void Update()
    {
        _basicMotion.Direction = _inputReader.movement;
        _dodgeMotion.Direction = _inputReader.movement;
    }
}
