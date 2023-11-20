using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IPlayer
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _movement;
    private PlayerAttackHandler _attackHandler;
    private PlayerHealth _health;
    private Loot _loot;
    private PlayerAnimations _anim;
    private EventActions _actions;
    private SpriteRenderer _sprite;
    private StatusHandler _statusHandler;
    public int currency = 10;

    public PlayerInput Input => _input;
    public PlayerHealth PlayerHealth => _health;
    public Loot Loot => _loot;

    private float _iframes = 0;
    private bool _hasIframes => _iframes > Time.timeSinceLevelLoad;

    private Coroutine _buffer = null;
    private float _bufferTime = 0.35f;
    private PlayerAction _dodgeAction = null;
    private PlayerAction _quickAttackAction = null;
    private PlayerAction _heavyAttackAction = null;

    [Header("Iframes")]
    [SerializeField] private float _hitFrames = 0.66f;
    [SerializeField] private float _dodgeFrames = 0.33f;
    [SerializeField] private Color _iFrameColor = new Color(1, 1, 1, 0.5f);
    private Color _baseColor = Color.white;

    public MonoBehaviour Mono => this;

    public Health Health => _health;
    public Movement Movement => _movement;
    public StatusHandler Status => _statusHandler;
    public Rigidbody2D Rigidbody => _rigidbody;

    public static event Action OnPlayerDeath;
    [SerializeField] private Material _damagedMaterial = null;
    private Material _defaultMaterial = null;
    [SerializeField] private float _hitStopTime = 0.3f;
    [SerializeField] private float _movementMultiplierWhileAttacking = 0.36f;

    [SerializeField] private PlayerUIAnimationController _playerUI;
    private bool _havePlayerUI;

    private void Awake()
    {
        _input = this.AddOrGetComponent<PlayerInput>();
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
        _sprite = GetComponent<SpriteRenderer>();
        _defaultMaterial = _sprite.material;
        _statusHandler = this.AddOrGetComponent<StatusHandler>();
        _statusHandler.Setup(this);

        GetComponentInChildren<CurrencyUI>().Setup(this);

        _dodgeAction = new PlayerAction(Dodge, () => _attackHandler.Cancel());
        _quickAttackAction = new PlayerAction(QuickAttack, () => !_movement.IsRolling && _attackHandler.CanAttack);
        _heavyAttackAction = new PlayerAction(HeavyAttack, () => !_movement.IsRolling && _attackHandler.CanAttack);

        _input.DodgeCallback = () => AddAction(_dodgeAction);
        _input.QuickAttackCallback = () => AddAction(_quickAttackAction);
        _input.HeavyAttackCallback = () => AddAction(_heavyAttackAction);

        _movement.Setup(_rigidbody);

        _havePlayerUI = _playerUI != null;
    }

    private void FixedUpdate()
    {
        var movement = _input.Movement;

        if (_attackHandler.IsAttacking)
        {
            movement *= _movementMultiplierWhileAttacking;
        }

        //if (!_attackHandler.IsAttacking)
        //{
        //    _attackHandler.transform.right = _inputReader.Aim;
        //}

        _attackHandler.transform.right = _input.Aim;
        _movement.Move(movement);
    }

    private void OnEnable()
    {
        _health.OnDeath += OnDeath;
        GameManager.OnPause += OnPause;
        GameManager.OnUnpause += OnUnpause;
    }

    private void OnDisable()
    {
        _health.OnDeath -= OnDeath;
        GameManager.OnPause -= OnPause;
        GameManager.OnUnpause -= OnUnpause;
    }

    private void OnPause()
    {
        _input.enabled = false;
    }

    private void OnUnpause()
    {
        _input.enabled = true;
    }

    private void OnDeath()
    {
        if (OnPlayerDeath != null) OnPlayerDeath();
        _input.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ItemPickup>(out var pickup))
        {
            if (!pickup.CheckIfShopItem())
            {
                if (_loot.TryAdd(pickup.Item))
                {
                    pickup.Take();
                }
            }
            else
            {
                if (pickup.Item.cost <= currency)
                {
                    if (_loot.TryAdd(pickup.Item))
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
            if (_havePlayerUI) _playerUI.CoinPickup();
            return;
        }
    }

    private void AddAction(PlayerAction action)
    {
        if (_buffer != null)
        {
            StopCoroutine(_buffer);
        }

        _buffer = StartCoroutine(InputBuffer(action));
    }

    private IEnumerator InputBuffer(PlayerAction action)
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

    private void AddIframes(float time)
    {
        float newTime = Time.timeSinceLevelLoad + time;
        if (_hasIframes && newTime < _iframes) return;
        _iframes = newTime;
    }

    private void Dodge()
    {
        if (_movement.TryDodge(_input.Movement))
        {
            EventActions.InvokeEvent(EventActionType.OnDodge);
            AddIframes(_dodgeFrames);
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
    public bool TryTakeCurrency(int cost)
    {
        if (cost <= currency)
        {
            currency -= cost;
            return true;
        }
        return false;
    }

    public void Hit(float damage, HitType type, ICharacter source = null)
    {
        if (_hasIframes) return;
        _health.TakeDamage(damage);
        AddIframes(_hitFrames);
        StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        _sprite.color = Color.white;
        if (_damagedMaterial != null) _sprite.material = _damagedMaterial;
        HitStopper.Stop(_hitStopTime);
        yield return new WaitForSeconds(0.2f);
        _sprite.material = _defaultMaterial;
    }
}
