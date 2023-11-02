using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Drop : MonoBehaviour
    {
        [SerializeField] private Coin coin;
        [SerializeField] private int value = 10;
        [SerializeField] private float dropChance = 10;
        private float dividedDropChance;
        // Start is called before the first frame update
        void Start()
        {
        }

        public void RollDrop()
        {
            dividedDropChance = dropChance / 100;
            if ((Random.Range(0f, 1f) <= dividedDropChance))
            {
                Coin drop = Instantiate(coin, transform.position, Quaternion.identity);
                drop.transform.SetParent(GetComponentInParent<Level>().gameObject.transform);
                drop.SetValue(value);
            }
        }
    }
}
