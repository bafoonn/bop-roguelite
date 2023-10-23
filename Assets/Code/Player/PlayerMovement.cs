using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    [SerializeField] private float _dodgeCooldown = 0.2f;
    [SerializeField] private float _dodgeDuration = 0.5f;
    [SerializeField] private float _dodgeSpeed = 10f;

    private Coroutine _rollRoutine = null;
    private bool _isRolling = false;
    public bool IsRolling => _isRolling;
    public bool CanRoll => (_rollRoutine == null && _isRolling == false);
    public Stat _movementSpeed;

    private void Start()
    {
        _movementSpeed = StatManager.Current.GetStat(StatType.MovementSpeed);
        _movementSpeed.ValueChanged += OnMovementSpeedChanged;
    }

    private void OnDestroy()
    {
        _movementSpeed.ValueChanged -= OnMovementSpeedChanged;
    }

    private void OnMovementSpeedChanged(float value)
    {
        BaseSpeed = value;
        SetSpeed();
    }

    public override void Move(Vector2 dir)
    {
        if (_isRolling)
        {
            return;
        }

        base.Move(dir);
    }

    public bool TryRoll(Vector2 dir)
    {
        if (!CanRoll)
        {
            return false;
        }

        if (dir.sqrMagnitude < Mathf.Epsilon)
        {
            return false;
        }

        _rollRoutine = StartCoroutine(Roll(dir));
        return true;
    }

    private IEnumerator Roll(Vector2 dir)
    {
        _isRolling = true;
        dir.Normalize();
        _currentDir = dir;
        _targetDir = dir;

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

        _isRolling = false;
        SetSpeed();

        yield return new WaitForSeconds(_dodgeCooldown);
        _rollRoutine = null;
    }
}
