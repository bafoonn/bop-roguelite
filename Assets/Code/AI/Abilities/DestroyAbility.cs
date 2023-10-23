using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DestroyAbility : MonoBehaviour
    {
        public float activeTime = 10;
        private void Start()
        {
            destroy();
        }


        private IEnumerator destroy()
        {
            yield return new WaitForSeconds(activeTime);
            Destroy(gameObject);        
        }
    }
}
