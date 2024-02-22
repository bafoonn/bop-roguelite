using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Pasta
{
    public class SpawnHealthPickup : ItemAbility
    {
        [SerializeField] private HealthRestore _healthRestorePrefab = null;
        [SerializeField] private float _spawnRadius = 3f;
        [SerializeField] private float _restoreDuration = 4f;
        [SerializeField, Range(0, 1f)] private float _healPercentage = 0.15f;

        protected override void Init()
        {
            Assert.IsNotNull(_healthRestorePrefab);
        }

        protected override void Trigger(EventContext context)
        {
            StartCoroutine(SpawnRestore());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }

        private IEnumerator SpawnRestore()
        {
            Vector2 pos = FindPosition();
            var restore = HealthRestore.Spawn(_healthRestorePrefab, pos, _healPercentage);
            float timer = _restoreDuration;
            while (timer > 0)
            {
                float t = timer / _restoreDuration;
                restore.transform.localScale = Vector3.one * t;
                timer -= Time.deltaTime;
                yield return null;
            }
            if (restore != null)
            {
                Destroy(restore.gameObject);
            }
        }

        private Vector2 FindPosition()
        {
            Vector2 randomPos = (Vector2)transform.position + RandomPos();
            if (LevelManager.Exists)
            {
                var tilemap = LevelManager.Current.ActiveLevel.tilemap;
                bool posFound = false;
                while (!posFound)
                {
                    randomPos = (Vector2)transform.position + RandomPos();
                    Vector3Int cell = tilemap.WorldToCell(randomPos);
                    TileBase tile = tilemap.GetTile(cell);
                    posFound = tile != null;
                }
            }

            return randomPos;

            Vector2 RandomPos() => Random.insideUnitCircle * _spawnRadius;
        }
    }
}
