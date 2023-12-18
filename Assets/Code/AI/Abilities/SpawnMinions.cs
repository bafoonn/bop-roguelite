using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Minion")]
    public class SpawnMinions : Ability
    {
        [SerializeField] private EnemyAi Minion;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public float spawnRadius;
        public override void Activate(GameObject parent)
        {
            //Instantiate(Minion);
            originPoint = parent.transform.position;
            var level = LevelManager.Current.ActiveLevel;

            originPoint = parent.transform.position + Random.insideUnitSphere * spawnRadius;
            originPoint2int = Vector2Int.RoundToInt(originPoint);
            originPoint3int = ((Vector3Int)originPoint2int);
            if (level.tilemap.HasTile(originPoint3int))
            {
                level.SpawnEnemy(Minion, originPoint);
            }
        }
    }


}

