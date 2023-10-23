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
        private GameObject player;
        public override void Activate(GameObject parent)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            originPoint = player.transform.position + Random.insideUnitSphere * spawnRadius;
            spawnedPoisonCloud = Instantiate(poisonCloudGameobject, originPoint, Quaternion.identity);
            DeactivateAbility();
        }
        private IEnumerator DeactivateAbility()
        {
            yield return new WaitForSeconds(coolDown + 1);
            Destroy(spawnedPoisonCloud);
        }

        public override void Deactivate()
        {
            Destroy(spawnedPoisonCloud);
        }
    }
}
