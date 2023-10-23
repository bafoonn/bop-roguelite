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
    private PlayerAnimations _anim;
    private EventActions _actions;
    public int currency = 100;

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
        _anim = GetComponent<PlayerAnimations>();
        _anim.Setup(_movement, _attackHandler, _inputReader);
        _actions = GetComponentInChildren<EventActions>();
        Assert.IsNotNull(_actions);
        _actions.Setup(this);
        GetComponentInChildren<CurrencyUI>().Setup(this);

        _inputReader.DodgeCallback = () =>
        {
            if (!_attackHandler.Cancel())
            {
                return;
            }
            if (_movement.TryRoll(_inputReader.Movement))
            {
                EventActions.InvokeEvent(EventActionType.OnDodge);
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
        if (collision.TryGetComponent<ItemPickup>(out var pickup))
        {
            if (_loot.TryAdd(pickup.Item))
            {
                if (!pickup.CheckIfShopItem())
                {
                    pickup.Take();
                }
                else
                {
                    if (pickup.Item.cost <= currency)
                    {
                        currency -= pickup.Item.cost;
                        pickup.Take();
                    }
                }
            }
            return;
        }

        if (collision.TryGetComponent<HealthRestore>(out var restore))
        {
            restore.Heal(_health);
            return;
        }

        if (collision.TryGetComponent<Coin>(out var coin))
        {
            currency += coin.Value;
            coin.Take();
            return;
        }
    }
    public void AddCurrency(int addedCurrency)
    {
        currency += addedCurrency;
    }
}
