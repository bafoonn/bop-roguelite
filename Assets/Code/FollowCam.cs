using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pasta
{
    public class FollowCam : MonoBehaviour
    {
        private Player _player = null;
        private InputReader _reader = null;
        public float Speed = 5f;
        public float OffsetSpeed = 5f;
        public float MaxDistanceFromPlayer = 4f;

        [Header("Controller Settings")]
        public float ControllerAimOffset = 2f;
        public float ControllerMoveOffset = 1f;

        [Header("Mouse Settings")]
        public float MouseAimOffset = 3f;
        public float MouseMoveOffset = 1f;
        [Range(0f, 1f)] public float OffsetMultiplier = 0.5f;

        private Vector2 _currentMove, _currentAim;

        private async void Start()
        {
            await FindPlayer();
        }

        private void FixedUpdate()
        {
            if (_player == null || _reader == null) return;
            Vector2 screenCenter = transform.position;

            Vector2 playerPos = _player.transform.position;
            Vector2 aimOffset = Vector2.zero;
            Vector2 moveOffset = Vector2.zero;

            if (_reader.IsMouseAim)
            {
                var aim = _reader.MouseWorldPos - screenCenter;
                if (aim.sqrMagnitude > 0.5f)
                    aimOffset += Vector2.ClampMagnitude(aim * OffsetMultiplier, MouseAimOffset);
                moveOffset += _reader.Movement * MouseMoveOffset;
            }
            else
            {
                if (_reader.IsAiming) aimOffset += _reader.Aim * ControllerAimOffset;
                else if (_reader.HasRecentlyAttacked) aimOffset += _reader.Aim;
                moveOffset += _reader.Movement * ControllerMoveOffset;
            }

            _currentAim = Vector2.Lerp(_currentAim, aimOffset, OffsetSpeed * Time.fixedDeltaTime);
            _currentMove = Vector2.Lerp(_currentMove, moveOffset, OffsetSpeed * Time.fixedDeltaTime);

            Vector2 offset = _currentAim + _currentMove;
            offset = Vector2.ClampMagnitude(offset, MaxDistanceFromPlayer);
            var targetPos = playerPos + offset;

            Vector3 newPos = Vector2.Lerp(transform.position, targetPos, Speed * Time.fixedDeltaTime);
            newPos.z = -10;
            transform.position = newPos;
        }

        private async Awaitable FindPlayer()
        {
            Player player = null;

            while (player == null)
            {
                player = FindFirstObjectByType<Player>();
                if (player == null)
                {
                    await Awaitable.WaitForSecondsAsync(10);
                }
            }

            _player = player;
            _reader = player.GetComponent<InputReader>();
        }
    }
}
