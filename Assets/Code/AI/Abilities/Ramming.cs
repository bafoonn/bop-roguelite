using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Ramming")]
    public class Ramming : Ability
    {
		[SerializeField] private GameObject rammingGObj;
		public override void Activate(GameObject parent)
		{
			GameObject spawnedRammingObj = Instantiate(rammingGObj, parent.transform);
		}



		public override void Deactivate()
		{
			base.Deactivate();
		}
	}
}
