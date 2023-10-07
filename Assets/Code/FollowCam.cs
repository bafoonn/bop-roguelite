using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pasta
{
    public class FollowCam : MonoBehaviour
    {
        private Transform _player = null;
        private InputReader _reader = null;
        public float Speed = 5f, AimOffset = 5f, MoveOffset = 2f;

        private async void Start()
        {
            await FindPlayer();
        }

        private void FixedUpdate()
        {
            if (_player == null || _reader == null) return;

            Vector2 targetPos = _player.position;
            targetPos += _reader.Movement * MoveOffset;
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

            _player = player.transform;
            _reader = player.GetComponent<InputReader>();
        }
    }
}
