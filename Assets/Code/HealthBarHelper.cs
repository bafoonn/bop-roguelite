using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pasta
{
    public class HealthBarHelper : MonoBehaviour
    {
        private void Start()
        {
            var health = GetComponent<Health>();
            GetComponentInChildren<HealthBar>().Setup(health);
            Destroy(this);
        }
    }
}
