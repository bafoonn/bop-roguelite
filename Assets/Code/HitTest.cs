using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class HitTest : MonoBehaviour, IHittable
    {
        private void Awake()
        {
            var stat = StatManager.Current.GetStat(StatType.Health);
        }


        public void Hit(float damage)
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
            Debug.Log(damage);
        }
    }
}
