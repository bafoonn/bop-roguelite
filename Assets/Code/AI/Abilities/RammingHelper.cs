using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class RammingHelper : MonoBehaviour
    {
        private Transform parent;
        private Ramming ramming;
        private float speed = 15f;
        public LayerMask obstacleLayer , playerlayer;
        private LayerMask intObstacle;
        private LayerMask intPlayer;
        private bool Charge = true;
        private Vector3 startDist;
        private Vector3 currentDist;
        private float totalDistance = 0f;
        public WeaponParent weaponParent;
        private AgentAnimations agentAnimations;
        private EnemyAi enemyAi;
        private Vector3 direction;
        private Rigidbody2D rbd2d;
        public Vector3 player;
        private bool startChargebool = false;
        private AgentMover agentMover;
        private BoxCollider2D[] boxCollider2Ds;
        public float windupTime;
        // Start is called before the first frame update
        void Start()
        {
            parent = transform.parent;
            ramming = FindFirstObjectByType<Ramming>();
            intObstacle = obstacleLayer;
            intPlayer = playerlayer;
            startDist = transform.position;
            direction = parent.Find("EnemyBody").transform.Find("Weapon").transform.Find("WeaponSprite").transform.right;
            player = GameObject.FindGameObjectWithTag("Player").transform.position;
            enemyAi = this.transform.GetComponentInParent<EnemyAi>();
            weaponParent = parent.GetComponentInChildren<WeaponParent>();
            agentAnimations = parent.GetComponent<AgentAnimations>();
            agentMover = parent.GetComponent<AgentMover>();
            boxCollider2Ds = parent.GetComponentsInChildren<BoxCollider2D>();
            rbd2d = parent.GetComponent<Rigidbody2D>(); 
            Charge = true;
            StartCoroutine(startCharge());
        }

        IEnumerator startCharge()
        {
            enemyAi.movementInput = Vector3.zero;
            agentMover.enabled = false;
            direction = parent.Find("EnemyBody").transform.Find("Weapon").transform.Find("WeaponSprite").transform.right;
            weaponParent.Aim = false;
            agentAnimations.aim = false;
            yield return new WaitForSeconds(windupTime);
            startChargebool = true;
            // Disabled enemy ai movement
            Debug.Log("Adding force");
            rbd2d.AddForce(direction * speed, ForceMode2D.Impulse); // adds Explosion like force to the "charge"
        }

        // Update is called once per frame
        void Update()
        {

            weaponParent.Aim = false;
            agentAnimations.aim = false;
            if (Charge && startChargebool)
            {
                float distanceThisFrame = Vector3.Distance(transform.position, startDist); // Get distance traveled in "time form" 
                totalDistance += distanceThisFrame;
                foreach (var boxCollider in boxCollider2Ds)
                {
                    boxCollider.enabled = false;
                }
                weaponParent.Aim = false;
                
                //this.transform.parent.Translate(direction * speed * Time.deltaTime);
                StartCoroutine(stopCharge());
            }
            if(totalDistance >= 700f)
            {
                Charge = false;
            }
            if(!Charge) 
            {
                agentMover.enabled = true;
                foreach (var boxCollider in boxCollider2Ds)
                {
                    boxCollider.enabled = true;
                }
                Debug.Log("Stopped charge");
                Destroy(gameObject);
            }
            
        }

        IEnumerator stopCharge()
        {
            yield return new WaitForSeconds(2.5f);
            agentMover.enabled = true;
            foreach (var boxCollider in boxCollider2Ds)
            {
                boxCollider.enabled = true;
            }
            Charge = false;
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.layer == obstacleLayer)
            {
                rbd2d.velocity = Vector2.zero;
                Debug.Log("Collided with wall");
                speed = 0f;
                Charge = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == obstacleLayer) // if this dosent work change rigidbody to kinematic
            {
                rbd2d.velocity = Vector2.zero;
                Debug.Log("Collided with wall");
                speed = 0f;
                Charge = false;
            }
            //if(collision.gameObject.layer == playerlayer)
            //{
            //    Charge = false;
            //}
        }
    }
}
