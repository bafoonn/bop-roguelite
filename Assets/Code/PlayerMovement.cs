using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private const float DODGE_COOLDOWN = 0.3f;
    private const float DODGE_DURATION = 0.5f;

    private bool _isDodging = false;
    private bool _canDodge = true;
    public override void Move(Vector2 dir, float deltaTime)
    {

        base.Move(dir, deltaTime);
    }
    public void Dodge(Vector2 dir)
    {
        if (!_canDodge)
        {
            return;
        }
        StartCoroutine(DodgeRoutine(dir));
    }

    private IEnumerator DodgeRoutine(Vector2 dir)
    {
        float timer = 0;
        float duration = 0.5f;
        float baseSpeed = Speed;
        _isDodging = true;
        _canDodge = false;
        float start = baseSpeed * 1.5f;
        float end = -baseSpeed;
        while (timer < duration)
        {
            float t = timer / duration;
            float flip = 1 - t;
            float square = flip * flip;

            Speed = baseSpeed + Mathf.Lerp(start, end, (1 - square));
            timer += Time.deltaTime;
            yield return null;
        }
        Speed = baseSpeed;
        _isDodging = false;
        yield return new WaitForSeconds(DODGE_COOLDOWN);
        _canDodge = true;
    }
}
