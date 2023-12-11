using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class PlayerMovement : Movement
{
    [Header("Dodge")]
    [SerializeField] private float _dodgeDelay = 0.2f;
    [SerializeField] private float _dodgeDuration = 0.5f;
    [SerializeField] private float _dodgeSpeed = 10f;
    [SerializeField] private int _currentDodgeCount = 1;
    [SerializeField] private float _dodgeTimer = 0f;

    public int DodgeCount => _currentDodgeCount;
    public float DodgeCooldown => _dodgeCooldown.Value;
    public float DodgeTimer => _dodgeTimer;
    public float DodgeCooldownProgress => _dodgeTimer / DodgeCooldown;
    public bool IsDodgeRecharging => _currentDodgeCount < _maxDodgeCount;
    public UnityEvent<int> OnDodgeGained;

    private Coroutine _dodgeRoutine = null;
    private bool _isDodging = false;
    public bool IsDodging => _isDodging;
    public bool CanDodge => _dodgeRoutine == null && _isDodging == false && _currentDodgeCount > 0;

    private Stat _movementSpeed;
    private Stat _dodgeCount;
    private Stat _dodgeCooldown;

    private int _maxDodgeCount => (int)_dodgeCount.Value;

    private VisualEffect dodgeEffect;
    private bool _haveDodgeEffect;

    public UnityEvent OnDodge;

    private void Start()
    {
        _movementSpeed = StatManager.Current.GetStat(StatType.Movementspeed);
        _dodgeCount = StatManager.Current.GetStat(StatType.DodgeCount);
        _dodgeCooldown = StatManager.Current.GetStat(StatType.DodgeCooldown);
        _movementSpeed.ValueChanged += OnMovementSpeedChanged;
        OnMovementSpeedChanged(_movementSpeed.Value);
        dodgeEffect = this.transform.Find("DodgeRollEffect").GetComponent<VisualEffect>();
        _haveDodgeEffect = dodgeEffect != null;
    }

    private void OnDestroy()
    {
        _movementSpeed.ValueChanged -= OnMovementSpeedChanged;
    }

    private void Update()
    {
        if (_currentDodgeCount < _maxDodgeCount)
        {
            _dodgeTimer += Time.deltaTime;
            if (_dodgeTimer > DodgeCooldown)
            {
                _currentDodgeCount++;
                _dodgeTimer = 0;
                if (OnDodgeGained != null)
                    OnDodgeGained.Invoke(_currentDodgeCount);
            }
        }
    }

    private void OnMovementSpeedChanged(float value)
    {
        BaseSpeed = value;
        UpdateSpeed();
    }

    public override void Move(Vector2 dir)
    {
        if (_isDodging)
        {
            return;
        }

        base.Move(dir);
    }

    public bool TryDodge(Vector2 dir)
    {
        if (!CanDodge)
        {
            return false;
        }

        if (dir.sqrMagnitude < Mathf.Epsilon)
        {
            return false;
        }
        OnDodge.Invoke();
        _dodgeRoutine = StartCoroutine(Dodge(dir));
        return true;
    }

    private IEnumerator Dodge(Vector2 dir)
    {
        _isDodging = true;
        _currentDodgeCount--;
        dir.Normalize();
        _currentDir = dir;
        _targetDir = dir;
        if (_haveDodgeEffect) dodgeEffect.SendEvent("Dodge");

        float timer = 0;
        float baseSpeed = _dodgeSpeed;
        float start = baseSpeed * 2f;
        float end = -baseSpeed;

        while (timer < _dodgeDuration)
        {
            float t = timer / _dodgeDuration;
            float flip = 1 - t;
            float square = flip * flip;

            Speed = baseSpeed + Mathf.Lerp(start, end, (1 - square));
            timer += Time.deltaTime;
            yield return null;
        }

        _isDodging = false;
        UpdateSpeed();
        if (_haveDodgeEffect) dodgeEffect.SendEvent("Stop");

        yield return new WaitForSeconds(_dodgeDelay);
        _dodgeRoutine = null;
    }
}
