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

        public virtual void Setup(Rigidbody2D rigidbody)
        {
            Debug.Assert(rigidbody != null);
            _rigidbody = rigidbody;
        }

        public virtual void Move(Vector2 dir, float deltaTime)
        {
            _targetDir = dir;
            _currentDir = Vector2.Lerp(_currentDir, _targetDir * Speed, Lerp * deltaTime);
            if (_currentDir.sqrMagnitude - _targetDir.sqrMagnitude < Vector2.kEpsilon) { _currentDir = _targetDir; }
            _rigidbody.MovePosition(_rigidbody.position + _currentDir * deltaTime);
        }
    }
}
