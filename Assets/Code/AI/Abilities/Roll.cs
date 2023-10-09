using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Roll : MonoBehaviour
    {
        [SerializeField] private float coolDown = 15f;
        private int RandomizeIfWillRoll;
        private bool canRoll = false;
        private GameObject player;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            RandomizeIfWillRoll = Random.Range(1, 3);
            if(RandomizeIfWillRoll == 1)
            {
                canRoll = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (canRoll)
            {
                if ((player.transform.position - transform.position).magnitude < 2.0f)
                {
                    if (coolDown > 0)
                    {
                        coolDown -= Time.deltaTime;

                    }
                    else
                    {
                        // TODO: ROLL HERE
                        coolDown = 15;
                    }
                }
            }
            
                
        }
    }
}
