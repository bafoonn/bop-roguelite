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
        public float Slow
        {
            get => _slow;
            set
            {
                if (value <= 0)
                {
                    _slow = 0;
                }
                else
                {
                    _slow = Mathf.Max(value, MINIMUM_SLOW);
                }

                UpdateSpeed();
            }
        }
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
            float slow = BaseSpeed * _slow;
            Speed = BaseSpeed - slow;
        }

        public virtual void Move(Vector2 dir)
        {
            _targetDir = dir;
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
