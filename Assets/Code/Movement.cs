using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Movement : MonoBehaviour
    {

        protected Rigidbody2D _rigidbody = null;
        public float Speed = 5.0f;
        public float Lerp = 15f;
        public Vector2 _currentDir, _targetDir;

        protected virtual void FixedUpdate()
        {
            _currentDir = Vector2.Lerp(_currentDir, _targetDir * Speed, Lerp * Time.fixedDeltaTime);
            if (_currentDir.sqrMagnitude - _targetDir.sqrMagnitude < Vector2.kEpsilon) { _currentDir = _targetDir; }
            _rigidbody.MovePosition(_rigidbody.position + _currentDir * Time.fixedDeltaTime);
        }

        public virtual void Setup(Rigidbody2D rigidbody)
        {
            Debug.Assert(rigidbody != null);
            _rigidbody = rigidbody;
        }

        public virtual void Move(Vector2 dir)
        {
            _targetDir = dir;
        }

        public virtual void Stop()
        {
            _targetDir = Vector2.zero;
        }
    }
}
