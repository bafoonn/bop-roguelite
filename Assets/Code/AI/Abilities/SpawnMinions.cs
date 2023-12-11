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
        [SerializeField] private GameObject Minion;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        public float spawnRadius;
        private Tilemap tileMap;
        public override void Activate(GameObject parent)
        {
            Instantiate(Minion);
            originPoint = parent.transform.position;
            tileMap = FindFirstObjectByType<Tilemap>();

            originPoint = parent.transform.position + Random.insideUnitSphere * spawnRadius;
            originPoint2int = Vector2Int.RoundToInt(originPoint);
            originPoint3int = ((Vector3Int)originPoint2int);
            if (tileMap.HasTile(originPoint3int))
            {
                Instantiate(Minion, originPoint, Quaternion.identity);
            }
        }
    }


}

