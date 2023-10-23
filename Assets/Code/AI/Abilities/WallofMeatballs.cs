using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/MeatballWall")]
    public class WallofMeatballs : Ability
    {
        public Tilemap tileMap;
        private Vector2 positiontoSpawn;
        public GameObject MeatBallWall;
        private GameObject spawnedWall;
        public int RandomPos = 1; // 1 = left, 2 = right, 3 = top, 4 = bottom
        [SerializeField] private BeamBall beamBall;
        // Start is called before the first frame update
        public override void Activate(GameObject parent)
        {
            tileMap = FindFirstObjectByType<Tilemap>();
            beamBall.Activate(parent); // DELETE THIS OR KEEP IT NEED TO TEST
            RandomPos = Random.Range(1, 5);
            Debug.Log(RandomPos + "RANDOMPOS");
            if(RandomPos == 1) // LEFT
            {
                positiontoSpawn = new Vector2(tileMap.cellBounds.xMin, 0);
            }
            if(RandomPos == 2) // RIGHT
            {
                positiontoSpawn = new Vector2(tileMap.cellBounds.xMax, 0);
            }
            if (RandomPos == 3) // TOP
            {
                positiontoSpawn = new Vector2(0, tileMap.cellBounds.yMax);
            }
            if (RandomPos == 4) // BOTTOM
            {
                positiontoSpawn = new Vector2(0, tileMap.cellBounds.yMin);
            }
            
            spawnedWall = Instantiate(MeatBallWall, positiontoSpawn, Quaternion.identity);
            DeactivateAbility();
        }


        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);
            Destroy(spawnedWall);
        }

        public override void Deactivate()
        {
            Destroy(spawnedWall);
        }
    }
}
