using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pasta
{
    public class Movement : MonoBehaviour
    {
        private const float MINIMUM_SLOW = 0.1f;

        protected Rigidbody2D _rigidbody = null;
        public float BaseSpeed = 5.0f, Speed = 0f;
        public float Acceleration = 15f;
        public float SlipperyAcceleration = 1f;
        public Vector2 _currentDir, _targetDir;
        private float _slow = 0f;
        private int _slipperyCount = 0;

        public bool IsSlowed => _slow > 0f;
        public bool IsMoving => _currentDir != Vector2.zero;

        protected virtual void Awake()
        {
            UpdateSpeed();
        }

        protected virtual void FixedUpdate()
        {
            float acceleration = _slipperyCount > 0 ? SlipperyAcceleration : Acceleration;
            _currentDir = Vector2.Lerp(_currentDir, _targetDir * Speed, acceleration * Time.fixedDeltaTime);
            if (_currentDir.sqrMagnitude - _targetDir.sqrMagnitude < Vector2.kEpsilon) { _currentDir = _targetDir; }
            _rigidbody.MovePosition(_rigidbody.position + _currentDir * Time.fixedDeltaTime);
        }

        public virtual void Setup(Rigidbody2D rigidbody)
        {
            Debug.Assert(rigidbody != null);
            _rigidbody = rigidbody;
        }

        public void UpdateSpeed()
        {
            Speed = BaseSpeed - Mathf.Clamp(_slow, 0f, BaseSpeed - BaseSpeed * MINIMUM_SLOW);
        }

        public virtual void Move(Vector2 dir)
        {
            _targetDir = dir;
        }

        public virtual void Slow(float percentage, float duration)
        {
            float slow = BaseSpeed * Mathf.Clamp01(percentage);
            StartCoroutine(SlowRoutine(slow, duration));
        }

        private IEnumerator SlowRoutine(float slowAmount, float duration)
        {
            _slow += slowAmount;
            UpdateSpeed();
            yield return new WaitForSeconds(duration);
            _slow -= slowAmount;
            UpdateSpeed();
        }

        public virtual void MakeSlippery()
        {
            _slipperyCount++;
        }

        public virtual void Unslip()
        {
            if (_slipperyCount > 0) _slipperyCount--;
        }

        public virtual void Stop()
        {
            _targetDir = Vector2.zero;
        }
    }
}
