using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IHittable
{
    private InputReader _input;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _movement;
    private PlayerAttackHandler _attackHandler;
    private PlayerHealth _health;
    private Loot _loot;
    private PlayerAnimations _anim;
    private EventActions _actions;
    public int currency = 10;

    public InputReader Input => _input;
    public PlayerHealth Health => _health;
    public Loot Loot => _loot;

    private Coroutine _buffer = null;
    private float _bufferTime = 0.35f;
    private PlayerAction _dodgeAction = null;
    private PlayerAction _quickAttackAction = null;
    private PlayerAction _heavyAttackAction = null;

    private void Awake()
    {
        _input = this.AddOrGetComponent<InputReader>();
        _movement = this.AddOrGetComponent<PlayerMovement>();
        _attackHandler = GetComponentInChildren<PlayerAttackHandler>();
        Assert.IsNotNull(_attackHandler, "Player has no PlayerAttackHandler component in children.");
        _rigidbody = this.AddOrGetComponent<Rigidbody2D>();
        _health = this.AddOrGetComponent<PlayerHealth>();
        _loot = this.AddOrGetComponent<Loot>();
        _anim = GetComponent<PlayerAnimations>();
        _anim.Setup(_movement, _attackHandler, _input);
        _actions = GetComponentInChildren<EventActions>();
        Assert.IsNotNull(_actions);
        _actions.Setup(this);
        GetComponentInChildren<CurrencyUI>().Setup(this);

        _dodgeAction = new PlayerAction(Dodge, () => _attackHandler.Cancel());
        _quickAttackAction = new PlayerAction(QuickAttack, () => !_movement.IsRolling && _attackHandler.CanAttack);
        _heavyAttackAction = new PlayerAction(HeavyAttack, () => !_movement.IsRolling && _attackHandler.CanAttack);

        _input.DodgeCallback = () => AddAction(_dodgeAction);
        _input.QuickAttackCallback = () => AddAction(_quickAttackAction);
        _input.HeavyAttackCallback = () => AddAction(_heavyAttackAction);

        _movement.Setup(_rigidbody);
    }

    private void FixedUpdate()
    {
        var movement = _input.Movement;

        if (_attackHandler.IsAttacking)
        {
            movement *= 0.3f;
        }

        //if (!_attackHandler.IsAttacking)
        //{
        //    _attackHandler.transform.right = _inputReader.Aim;
        //}
        _attackHandler.transform.right = _input.Aim;
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
            coin.Take(ref currency);
            return;
        }
    }

    private void AddAction(PlayerAction action)
    {
        if (_buffer != null)
        {
            StopCoroutine(_buffer);
        }

        _buffer = StartCoroutine(Buffer(action));
    }

    private IEnumerator Buffer(PlayerAction action)
    {
        float timer = 0f;
        do
        {
            if (action.Condition())
            {
                action.Action();
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < _bufferTime);
        _buffer = null;
    }

    private void Dodge()
    {
        if (_movement.TryRoll(_input.Movement))
        {
            EventActions.InvokeEvent(EventActionType.OnDodge);
        }
    }

    private void QuickAttack()
    {
        _attackHandler.Attack(_input.Aim, PlayerAttackHandler.AttackType.Quick);
    }

    private void HeavyAttack()
    {
        _attackHandler.Attack(_input.Aim, PlayerAttackHandler.AttackType.Heavy);
    }

    private class PlayerAction
    {
        public Action Action;
        public Func<bool> Condition;

        public PlayerAction(Action action, Func<bool> condition)
        {
            Action = action;
            Condition = condition;
        }
    }

    public void AddCurrency(int addedCurrency)
    {
        currency += addedCurrency;
    }

    public void Hit(float damage)
    {
        _health.TakeDamage(damage);
    }
}
