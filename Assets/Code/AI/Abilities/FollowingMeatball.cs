using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/FollowingMeatball")]
    public class FollowingMeatball : Ability
    {
        private Vector3 originPoint;
        public int spawnRadius = 2;
        public GameObject MeatBall;
        public override void Activate(GameObject parent)
        {
            originPoint = Random.insideUnitSphere * spawnRadius;



            float directionFacing = Random.Range(0, spawnRadius);
            Instantiate(MeatBall, originPoint, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
        }

        
    }
}
