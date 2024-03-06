using System.Collections;
using UnityEngine;

namespace Pasta
{
    public class RammingHelper : MonoBehaviour
    {
        public Transform parent;
        private Ramming ramming;
        private float speed = 15f;
        private bool pointAtPlayer = true;
        public LayerMask obstacleLayer, playerlayer;
        private LayerMask intObstacle;
        private LayerMask intPlayer;
        private bool Charge = true;
        private Vector3 startDist;
        private Vector3 currentDist;
        private float totalDistance = 0f;
        public WeaponParent weaponParent;
        private AgentAnimations agentAnimations;
        private EnemyAi enemyAi;
        private BossAI bossAI;
        private Vector3 direction;
        private Rigidbody2D rbd2d;
        public Vector3 player;
        private bool startChargebool = false;
        private AIData aidata;
        private AgentMover agentMover;
        private CircleCollider2D circleCollider2d;
        private BoxCollider2D[] boxCollider2Ds;
        private float Damage = 10f;
        private AttackEffects attackEffects;
        public float windupTime;
        private GameObject Player;
        private PolygonCollider2D polygonCollider2D;
        // Start is called before the first frame update
        void Start()
        {
            parent = transform.parent;
            aidata = parent.GetComponent<AIData>();
            ramming = FindFirstObjectByType<Ramming>();
            intObstacle = obstacleLayer;
            intPlayer = playerlayer;
            startDist = transform.position;
            Player = GameObject.FindGameObjectWithTag("Player");
            direction = parent.GetComponentInChildren<CleavingWeaponAnimations>().transform.Find("SpritePivot").transform.Find("WeaponSprite").transform.right;
            player = GameObject.FindGameObjectWithTag("Player").transform.position;
            circleCollider2d = GetComponent<CircleCollider2D>();
            circleCollider2d.enabled = false;
            if (parent.gameObject.tag != "Boss")
            {
                enemyAi = this.transform.GetComponentInParent<EnemyAi>();
            }
            else
            {
                bossAI = this.transform.GetComponentInParent<BossAI>();
            }
            weaponParent = parent.GetComponentInChildren<WeaponParent>();
            agentAnimations = parent.GetComponent<AgentAnimations>();
            agentMover = parent.GetComponent<AgentMover>();
            boxCollider2Ds = parent.GetComponentsInChildren<BoxCollider2D>();
            rbd2d = parent.GetComponent<Rigidbody2D>();
            Charge = true;
            StartCoroutine(getTarget());
        }

        IEnumerator getTarget()
        {
            agentMover.enabled = false;
            yield return new WaitForSeconds(0.2f);
            direction = parent.GetComponentInChildren<CleavingWeaponAnimations>().transform.Find("SpritePivot").transform.right;
            StartCoroutine(startCharge());
        }

        IEnumerator startCharge()
        {
            if (parent.gameObject.tag != "Boss")
            {
                enemyAi.movementInput = Vector3.zero;
            }
            else
            {
                bossAI.movementInput = Vector3.zero;
            }
            agentMover.enabled = false;
            direction = parent.GetComponentInChildren<CleavingWeaponAnimations>().transform.Find("SpritePivot").transform.right;
            pointAtPlayer = false;
            weaponParent.Aim = false;
            agentAnimations.aim = false;
            yield return new WaitForSeconds(windupTime);
            startChargebool = true;
            // Disabled enemy ai movement
            Debug.Log("Adding force");
            rbd2d.AddForce(direction * speed, ForceMode2D.Impulse); // adds Explosion like force to the "charge"
            circleCollider2d.enabled = true;
            if ((Player.transform.position - transform.position).magnitude < 2.5f) // Stops enemies from pushing player
            {
                rbd2d.velocity = Vector2.zero;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (pointAtPlayer == false)
            {
                aidata.currentTarget = null;
            }

            if (Charge && startChargebool)
            {
                rbd2d.velocity = new Vector2(direction.x * speed, direction.y * speed);
                float distanceThisFrame = Vector3.Distance(transform.position, startDist); // Get distance traveled in "time form" 
                totalDistance += distanceThisFrame;
                foreach (var boxCollider in boxCollider2Ds)
                {
                    boxCollider.enabled = false;
                }
                weaponParent.Aim = false;
                agentAnimations.aim = false;
                agentMover.enabled = false;
                if (parent.gameObject.tag != "Boss")
                {
                    enemyAi.movementInput = Vector3.zero;
                }
                else
                {
                    bossAI.movementInput = Vector3.zero;
                }
                //this.transform.parent.Translate(direction * speed * Time.deltaTime);
                StartCoroutine(stopCharge());
            }
            if (totalDistance >= 700f)
            {
                Charge = false;
            }
            if (!Charge)
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
            rbd2d.velocity = Vector2.zero;
            foreach (var boxCollider in boxCollider2Ds)
            {
                boxCollider.enabled = true;
            }
            Charge = false;
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == obstacleLayer)
            {
                rbd2d.velocity = Vector2.zero;
                Debug.Log("Collided with wall");
                speed = 0f;
                Charge = false;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == obstacleLayer) // if this dosent work change rigidbody to kinematic
            {
                rbd2d.velocity = Vector2.zero;
                Debug.Log("Collided with wall");
                speed = 0f;
                Charge = false;
            }
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(Damage);
                    rbd2d.velocity = Vector2.zero;
                    Charge = false;
                }
            }
        }
    }
}

