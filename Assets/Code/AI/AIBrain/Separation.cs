using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Pasta
{
    public class Separation : MonoBehaviour
    {
        private float requiredDistanceFromNeighbors = 1f;
        public List<GameObject> neighbors;
        public bool tooClose = false;
        public Vector3 direction;
        private AIData aidata;

        private void Start()
        {
            aidata = GetComponent<AIData>();
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision detected");
            if (collision.gameObject.TryGetComponent<ICharacter>(out var character))
            {
                neighbors.Add(collision.gameObject);
            }
            
        }

       

        private void OnCollisionExit2D(Collision2D collision)
        {
            neighbors.Remove(collision.gameObject);
        }

        private void Update()
        {
            Seperate();
        }

        public Vector3 Seperate()
        {

            Vector3 ReturnVector = new Vector3(0, 0, 0);
            int averageCounter = 0;

            for (int I = 0; I < neighbors.Count; I++)
            {
                if (Vector3.Distance(neighbors[I].transform.position, transform.position) < requiredDistanceFromNeighbors) //If the neighbor is too close.
                {
                    tooClose = true;
                    averageCounter += 1;
                    ReturnVector += (transform.position - neighbors[I].transform.position);
                }
                else
                {
                    tooClose = false;
                }
            }

            ReturnVector = ReturnVector / averageCounter; //Average the vector.
            direction = ReturnVector;
            return ReturnVector;
        }

    }
}
