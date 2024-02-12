using System;
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
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Dead = Animator.StringToHash("Death");

        [SerializeField]
        private SpriteRenderer _renderer = null;
        [SerializeField]
        private Animator _animator = null;
        private PlayerMovement _movement = null;
        private PlayerAttackHandler _attackHandler = null;
        private PlayerInput _input = null;
        private PlayerHealth _health = null;

        public Vector2 _facingDir = Vector2.zero;

        private int _current;

        private void FixedUpdate()
        {
            if (_input.Movement != Vector2.zero) _facingDir = _input.Aim;
            _facingDir.Normalize();
            _renderer.flipX = DoFlip();
            var dir = _input.Aim;
            _animator.SetFloat(MoveX, dir.x);
            _animator.SetFloat(MoveY, dir.y);

            int state = FindState();
            if (state == _current) return;

            _animator.CrossFade(state, 0, 0);
            _current = state;
        }

        private int FindState()
        {
            if (_health.IsDead) return Dead;
            if (_movement.IsDodging) return Run;
            if (_movement.IsMoving) return Run;
            if (_attackHandler.IsAttacking) return Run;
            return Idle;
        }

        private bool DoFlip()
        {
            bool flip = Vector2.Dot(Vector2.right, _facingDir) < 0;
            return flip;
        }

        public void Setup(IPlayer player)
        {
            _movement = player.Movement as PlayerMovement;
            _attackHandler = player.AttackHandler;
            _input = player.Input;
            _health = player.Health as PlayerHealth;
        }
    }
}
