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
        public override void Activate(GameObject parent)
        {
            spawnedPoisonCloud = Instantiate(poisonCloudGameobject);
            spawnedPoisonCloud.transform.SetParent(parent.transform);
        }


        public override void Deactivate()
        {
            Destroy(spawnedPoisonCloud);
        }
    }
}
