using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class PlayerCloseSensor : MonoBehaviour
    {
        public float radius = 4;

        public float precentage = 40;
        private float precentageSubtract;
        public float result;
        private FixedEnemyAI enemyai;
        [SerializeField] private LayerMask layermask;
        private SeekBehaviour seekBehaviour;
        private int layer;
        public int maxEnemiesThatcanAttack = 4;
        private float timer = 3f; // Initial timer value
        private ApproachPlayerState approachState;

        // Start is called before the first frame update
        void Start()
        {

            precentageSubtract = precentage / 100;
            layer = layermask;
            StartCoroutine(updateAttackers());
        }

        private void OnEnable()
        {
            ItemAbilities.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            ItemAbilities.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext obj)
        {
            if (obj.EventType != EventActionType.OnRoomEnter) return;
            StartCoroutine(updateAttackers());
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {

                // Reset the timer
                timer = 3f;

                // Call the coroutine logic
                StartCoroutine(updateAttackers());
            }
        }

        private IEnumerator updateAttackers()
		{
            yield return new WaitForSeconds(0.5f);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layermask); 

            // sorts closest hitcolliders to player.
            System.Array.Sort(hitColliders, (a, b) => Vector2.Distance(a.transform.position, transform.position).CompareTo(Vector2.Distance(b.transform.position, transform.position)));
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (i < maxEnemiesThatcanAttack) // Check if the current hitcollider is inside the result
                {
                    if (hitColliders[i].gameObject.TryGetComponent(out FixedEnemyAI enemyAi))
                    {
                        
                        if ((transform.position - hitColliders[i].gameObject.transform.position).magnitude < radius)
                        {
                           
                            AIData aidata = hitColliders[i].gameObject.GetComponent<AIData>();               
                            Debug.Log("Can attack");
                            seekBehaviour = hitColliders[i].gameObject.GetComponentInChildren<SeekBehaviour>();
                            enemyai = hitColliders[i].gameObject.GetComponent<FixedEnemyAI>();
                            enemyai.ToggleMaintainDistance(false);
                            enemyAi.ActivateIndicator();
                            enemyai.canAttack = true;
                        }
                        
                    }
                }
                else
                {
                    if (hitColliders[i].gameObject.TryGetComponent(out FixedEnemyAI enemyAi))
                    {
                        enemyai = hitColliders[i].gameObject.GetComponent<FixedEnemyAI>();
                        enemyai.ToggleMaintainDistance(true);
                        enemyAi.DeActivateIndicator();
                        enemyai.canAttack = false;
                        AIData aidata = hitColliders[i].gameObject.GetComponent<AIData>();
                        //aidata.currentTarget = null;
                        
                    }
                }

            }
            yield break;
        }


        

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}
