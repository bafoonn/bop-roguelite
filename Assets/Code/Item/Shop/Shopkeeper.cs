using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Shopkeeper : MonoBehaviour
    {
        [SerializeField]
        private ShopItemGeneration[] itemGenerators;
        [SerializeField]
        private HealShop healShop;
        // Start is called before the first frame update
        void Start()
        {
            GenerateItems();
        }

        public void GenerateItems()
        {
            for (int i = 0; i < itemGenerators.Length; i++)
            {
                itemGenerators[i].GenerateItem();
            }

            healShop.gameObject.SetActive(true);
        }
    }
}
