using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class DartShooter : MonoBehaviour
    {
        private bool OnCooldown;
        [SerializeField]
        private float cooldown = 3f;
        // Start is called before the first frame update
        
        IEnumerator Cooldown()
        {
            OnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            OnCooldown = true;
        }

        public bool ReturnOnCooldown()
        {
            return OnCooldown;
        }

        public void PutOnCooldown()
        {
            StartCoroutine(Cooldown());
        }
    }
}
