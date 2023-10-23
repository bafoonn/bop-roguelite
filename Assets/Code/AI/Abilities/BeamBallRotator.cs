using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class BeamBallRotator : MonoBehaviour
    {
        [SerializeField] private GameObject beamGameBall;
        public int spawnRadius = 10;
        public int BossRadius = 2; // Bosses personal space to avoid spawning on top of boss
        public int BallRadius = 2;
        private Vector3 originPoint;
        private Vector3 zAxis = new Vector3(0, 0, 1);
        public float rotationSpeed = 0;
        public Rigidbody2D bpdy;
        // Start is called before the first frame update
        void Start()
        {
            originPoint.x += Random.Range(-spawnRadius, spawnRadius);
            originPoint.y += Random.Range(-spawnRadius, spawnRadius);


            BeamBall beamBall = FindFirstObjectByType<BeamBall>();
            float directionFacing = Random.Range(0, spawnRadius);

            DamageArea[] area = gameObject.GetComponentsInChildren<DamageArea>();
            foreach (DamageArea damagearea in area)
            {
                if (beamBall != null)
                {
                    damagearea.Damage = beamBall.damage;
                }

            }

        }



        // Update is called once per frame
        void Update()
        {
            bpdy.MoveRotation(bpdy.rotation + 1 * rotationSpeed);
            //beamGameBall.transform.RotateAround(beamGameBall.transform.position, zAxis, rotationSpeed);
        }
    }
}
