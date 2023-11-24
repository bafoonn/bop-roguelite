using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RammingHelper : MonoBehaviour
    {
        private Transform parent;
        private Ramming ramming;
        private float speed = 20f;
        public LayerMask obstacleLayer , playerlayer;
        private LayerMask intObstacle;
        private LayerMask intPlayer;
        private bool Charge = true;
        // Start is called before the first frame update
        void Start()
        {
            parent = this.transform.GetComponentInParent<Transform>();
            ramming = FindFirstObjectByType<Ramming>();
            speed = ramming.speed;
            intObstacle = obstacleLayer;
            intPlayer = playerlayer;
        }

        // Update is called once per frame
        void Update()
        {
            if (Charge)
            {
                parent.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == obstacleLayer)
            {
                Charge = false;
            }
            if(collision.gameObject.layer == playerlayer)
            {
                Charge = false;
            }
        }
    }
}
