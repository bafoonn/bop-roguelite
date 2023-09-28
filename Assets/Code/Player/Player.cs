using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IHittable
{
    private InputReader _inputReader;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _movement;
    private PlayerAttackHandler _attackHandler;
    private PlayerHealth _health;
    private Loot _loot;

    public void Hit(float damage)
    {
        _health.TakeDamage(damage);
    }

    private void Awake()
    {
        _inputReader = this.AddOrGetComponent<InputReader>();
        _movement = this.AddOrGetComponent<PlayerMovement>();
        _attackHandler = GetComponentInChildren<PlayerAttackHandler>();
        Assert.IsNotNull(_attackHandler, "Player has no PlayerAttackHandler component in children.");
        _rigidbody = this.AddOrGetComponent<Rigidbody2D>();
        _health = this.AddOrGetComponent<PlayerHealth>();
        _loot = this.AddOrGetComponent<Loot>();

        _inputReader.DodgeCallback = () =>
        {
            if (!_attackHandler.Cancel())
            {
                return;
            }
            if (_movement.TryRoll(_inputReader.Movement))
            {
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pickup>(out var pickup))
        {
            if (_loot.TryAdd(pickup.Item))
            {
                pickup.Take();
            }
        }
    }
}
