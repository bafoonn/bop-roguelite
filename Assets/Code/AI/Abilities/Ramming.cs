using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Ramming")]
    public class Ramming : Ability
    {
		[SerializeField] public GameObject rammingGObj;
		private GameObject spawnedRammingOb;
		public EnemyAi enemyai;
		private AIData aidata;
		private Rigidbody2D rbd2d;
		public float speed = 20f;
		

        public override void Activate(GameObject parent)
		{
			GameObject spawnedRammingObj = Instantiate(rammingGObj, parent.transform);
			enemyai = parent.GetComponent<EnemyAi>();
			aidata = parent.GetComponent<AIData>();
            
        }

		



		public override void Deactivate()
		{
			Destroy(spawnedRammingOb);
		}
	}
}
