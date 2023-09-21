using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/BeamBall")]
    public class BeamBall : Ability
    {
        [SerializeField] private GameObject beamGameBall;
        public int spawnRadius = 10;
        public int BossRadius = 2; // Bosses personal space to avoid spawning on top of boss
        public int BallRadius = 2;
        private Vector3 originPoint;
        private Vector3 zAxis = new Vector3(0, 0, 1);
        public override void Activate(GameObject parent)
        {

            originPoint = Random.insideUnitSphere * spawnRadius;



            float directionFacing = Random.Range(0, spawnRadius);
            Instantiate(beamGameBall, originPoint, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));


        }

        public override void Deactivate()
        {
            Destroy(beamGameBall);
        }
    }
}
