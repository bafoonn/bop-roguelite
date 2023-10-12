using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Roll : MonoBehaviour // PROBS BETTER NAME WOULD HAVE BEEN DASH so no need to animate enemies with roll
    {
        [SerializeField] private float coolDown = 15f;
        private int RandomizeIfWillRoll;
        private bool canRoll = false;
        private GameObject player;
        [SerializeField] private float speed = 20f;
        private Rigidbody2D rbd2d;
        // Start is called before the first frame update
        void Start()
        {
            rbd2d = GetComponent<Rigidbody2D>();
            RandomizeIfWillRoll = Random.Range(1, 3);
            if (RandomizeIfWillRoll == 1)
            {
                canRoll = true;
                player = GameObject.FindGameObjectWithTag("Player");
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
                        rbd2d.AddForce(transform.up * speed);
                        Debug.Log("DASHING");
                        coolDown = 15;
                    }
                }
            }
            
                
        }
    }
}
