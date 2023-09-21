using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] enemies;
        private Level level;
        // Start is called before the first frame update
        void Start()
        {
            level = GetComponentInParent<Level>();
            int random = Random.Range(0, enemies.Length);
            Instantiate(enemies[random], transform.position, Quaternion.identity);
            level.AddToEnemyCount();
            Destroy(this.gameObject);
        }
    }
}
