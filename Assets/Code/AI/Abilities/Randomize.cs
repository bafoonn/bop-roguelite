using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Randomize : MonoBehaviour
    {
        public bool canuseability = true;
        private float cooldown = 10f;
        private int randomInt;
        // Start is called before the first frame update
        

        // Update is called once per frame
        void Update()
        {
            cooldown -= Time.deltaTime;
            if(cooldown <= 0)
			{
                canuseability = true;
			}
            randomInt = Random.Range(1, 6);
            if(randomInt == 1 && canuseability)
			{
                canuseability = true;
			}
			else
			{
                canuseability = false;
			}
        }
    }
}
