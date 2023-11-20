using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : Movement
{
    [SerializeField] private float _dodgeCooldown = 0.2f;
    [SerializeField] private float _dodgeDuration = 0.5f;
    [SerializeField] private float _dodgeSpeed = 10f;

    private Coroutine _dodgeRoutine = null;
    private bool _isDodging = false;
    public bool IsRolling => _isDodging;
    public bool CanDodge => (_dodgeRoutine == null && _isDodging == false);
    public Stat _movementSpeed;

    private VisualEffect dodgeEffect;
    private bool _haveDodgeEffect;

    private void Start()
    {
        _movementSpeed = StatManager.Current.GetStat(StatType.Movementspeed);
        _movementSpeed.ValueChanged += OnMovementSpeedChanged;
        OnMovementSpeedChanged(_movementSpeed.Value);
        dodgeEffect = this.transform.Find("DodgeRollEffect").GetComponent<VisualEffect>();
        _haveDodgeEffect = dodgeEffect != null;
    }

    private void OnDestroy()
    {
        _movementSpeed.ValueChanged -= OnMovementSpeedChanged;
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

        _dodgeRoutine = StartCoroutine(Dodge(dir));
        return true;
    }

    private IEnumerator Dodge(Vector2 dir)
    {
        _isDodging = true;
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

        yield return new WaitForSeconds(_dodgeCooldown);
        _dodgeRoutine = null;
    }
}
