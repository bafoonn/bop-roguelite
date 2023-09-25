using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/MeatballWall")]
    public class WallofMeatballs : Ability
    {
        private Tilemap tileMap;
        private Vector2 center;
        public GameObject MeatBallWall;
        // Start is called before the first frame update
        public override void Activate(GameObject parent)
        {
            tileMap = FindFirstObjectByType<Tilemap>();
            center = tileMap.cellBounds.center;
            Instantiate(MeatBallWall, center, Quaternion.identity);
        }

        public override void Deactivate()
        {
            Destroy(MeatBallWall);
        }
    }
}
