using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DestroyAbility : MonoBehaviour
    {
        public float activeTime = 10;
        private void Awake()
        {
            StartCoroutine("DestroyObject");
        }


        private IEnumerator DestroyObject()
        {
            yield return new WaitForSeconds(activeTime);
            Destroy(gameObject);        
        }
    }
}
