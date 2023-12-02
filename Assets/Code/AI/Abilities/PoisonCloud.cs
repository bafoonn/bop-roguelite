using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/PoisonCloud")]
    public class PoisonCloud : Ability
    {
        public GameObject poisonCloudGameobject;
        private GameObject spawnedPoisonCloud;
        private Vector2 originPoint;
        public float spawnRadius = 2f;
        public float minSpawnDistance = 5.0f; // Adjust the minimum spawn distance as needed
        private float radius = 10f;
        private GameObject player;
        private GameObject[] allPoisons;
        private DestroyAbility destroyAbility;
        private float Activetime;
        public override void Activate(GameObject parent)
        {
            allPoisons = GameObject.FindGameObjectsWithTag("Ability");

            bool canSpawn = true;
            player = GameObject.FindGameObjectWithTag("Player");
            originPoint = player.transform.position + Random.insideUnitSphere * spawnRadius;
            foreach (var poisonCloud in allPoisons)
            {
                float distance = Vector2.Distance(poisonCloud.transform.position, originPoint);

                if (distance < minSpawnDistance)
                {
                    canSpawn = false;
                    break;
                }
            }

			if (canSpawn)
			{
                player = GameObject.FindGameObjectWithTag("Player");
                spawnedPoisonCloud = Instantiate(poisonCloudGameobject, originPoint, Quaternion.identity);
                Activetime = ActiveTime;
                destroyAbility = spawnedPoisonCloud.AddComponent<DestroyAbility>();
                destroyAbility.activeTime = Activetime;
                DeactivateAbility();
            }
                
            
           
        }
        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(ActiveTime + 1);
            Destroy(spawnedPoisonCloud);
        }

        public override void Deactivate()
        {
           if(spawnedPoisonCloud != null) Destroy(spawnedPoisonCloud);
        }
    }
}
