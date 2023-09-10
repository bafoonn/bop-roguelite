using Pasta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private const float DODGE_COOLDOWN = 0.2f;
    private const float DODGE_DURATION = 0.5f;

    private Coroutine _rollRoutine = null;
    private bool _isRolling = false;
    public bool IsRolling => _isRolling;
    public bool CanRoll => (_rollRoutine == null && _isRolling == false);

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
        float baseSpeed = Speed;
        float start = baseSpeed * 2f;
        float end = -baseSpeed;

        while (timer < DODGE_DURATION)
        {
            float t = timer / DODGE_DURATION;
            float flip = 1 - t;
            float square = flip * flip;

            Speed = baseSpeed + Mathf.Lerp(start, end, (1 - square));
            timer += Time.deltaTime;
            yield return null;
        }

        _isRolling = false;
        Speed = baseSpeed;

        yield return new WaitForSeconds(DODGE_COOLDOWN);
        _rollRoutine = null;
    }
}
