using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHittable
{
    private InputReader _inputReader;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _movement;
    private PlayerAttackHandler _attackHandler;

    public void Hit(float damage)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        _inputReader = this.AddOrGetComponent<InputReader>();
        _movement = this.AddOrGetComponent<PlayerMovement>();
        _attackHandler = GetComponentInChildren<PlayerAttackHandler>();
        Debug.Assert(_attackHandler != null, $"Player has no PlayerAttackHandler component in children.");
        _rigidbody = this.AddOrGetComponent<Rigidbody2D>();

        _inputReader.DodgeCallback = () =>
        {
            if (!_attackHandler.Cancel())
            {
                return;
            }
            _movement.TryRoll(_inputReader.Movement);
        };

        _inputReader.QuickAttackCallback = () =>
        {
            if (_movement.IsRolling)
            {
                return;
            }
            _attackHandler.TryToAttack(_inputReader.Movement, PlayerAttackHandler.AttackType.Quick);
        };

        _inputReader.HeavyAttackCallback = () =>
        {
            if (_movement.IsRolling)
            {
                return;
            }
            _attackHandler.TryToAttack(_inputReader.Movement, PlayerAttackHandler.AttackType.Heavy);
        };

        _movement.Setup(_rigidbody);
    }

    private void FixedUpdate()
    {
        var movement = _inputReader.Movement;

        if (_attackHandler.IsAttacking)
        {
            movement *= 0.3f;
        }

        if (!_attackHandler.IsAttacking)
        {
            _attackHandler.transform.right = _inputReader.Aim;
        }
        _movement.Move(movement);
    }
}
