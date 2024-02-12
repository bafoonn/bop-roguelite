using UnityEngine;

namespace Pasta
{
    [CreateAssetMenu(menuName = "Abilities/Ramming")]
    public class Ramming : Ability
    {
        [SerializeField] public GameObject rammingGObj;
        private GameObject spawnedRammingOb;
        public EnemyAi enemyai;
        public BossAI bossAI;
        private AIData aidata;
        private AgentMover agentMover;
        private Rigidbody2D rbd2d;
        public float speed = 20f;


        public override void Activate(GameObject parent)
        {

            GameObject spawnedRammingObj = Instantiate(rammingGObj, parent.transform);
            if (parent.gameObject.tag != "Boss")
            {
                enemyai = parent.GetComponent<EnemyAi>();
            }
            else
            {
                bossAI = parent.GetComponent<BossAI>();
            }

            aidata = parent.GetComponent<AIData>();
            agentMover = parent.GetComponent<AgentMover>();

        }





        public override void Deactivate()
        {
            if (agentMover != null)
            {
                agentMover.enabled = true;
            }
            Destroy(spawnedRammingOb);
        }
    }
}
