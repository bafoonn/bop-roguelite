using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pasta
{
    public class PlayerAnimations : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");

        [SerializeField]
        private SpriteRenderer _renderer = null;
        [SerializeField]
        private Animator _animator = null;
        private PlayerMovement _movement = null;
        private PlayerAttackHandler _attackHandler = null;
        private InputReader _input = null;

        private Vector2 _facingDir = Vector2.zero;

        private int _current;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            _facingDir = _movement.IsRolling ? _movement._currentDir : _input.Aim;
            _facingDir.Normalize();
            _renderer.flipX = DoFlip();
            var dir = _attackHandler.IsAttacking ? _input.Aim : _input.Movement;
            _animator.SetFloat(MoveX, dir.x);
            _animator.SetFloat(MoveY, dir.y);

            int state = FindState();
            if (state == _current) return;

            _animator.CrossFade(state, 0, 0);
            _current = state;
        }

        private int FindState()
        {
            if (_movement.IsRolling) return Dodge;
            if (_movement._currentDir.sqrMagnitude > Mathf.Epsilon) return Run;
            if (_attackHandler.IsAttacking) return Run;
            return Idle;
        }

        private bool DoFlip()
        {
            bool flip = Vector2.Dot(Vector2.right, _facingDir) < 0;
            if (_current == Idle) return flip;
            if (_current == Dodge) return flip;
            return false;
        }

        public void Setup(PlayerMovement movement, PlayerAttackHandler attackHandler, InputReader input)
        {
            _movement = movement;
            _attackHandler = attackHandler;
            _input = input;
            enabled = true;
        }
    }
}
