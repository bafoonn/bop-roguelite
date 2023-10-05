using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class ItemsUI : MonoBehaviour
    {
        public static ItemsUI instance;
        public List<ItemBase> Items = new List<ItemBase> ();

        public Transform ItemContent;
        public GameObject ItemHolder;

        public ItemInHolder[] itemsInUi;
        private void Awake()
        {

            instance = this;
        }

        public void Add(ItemBase item)
        {
            if (item.CanStack)
            {
                bool alreadyIn = false;
                foreach(ItemBase Item in Items)
                {
                    if(Item.Name == item.Name)
                    {
                        Item.Amount += item.Amount;
                    }
                }
                if (!alreadyIn)
                {
                    Items.Add(item);
                }
            }

            else
            {
                Items.Add(item);
            }
            
        }

        public void ListItems()
        {
            foreach(Transform item in ItemContent)
            {
                Destroy(item.gameObject);
            }
            foreach (var item in Items)
            {
                GameObject obj = Instantiate(ItemHolder, ItemContent);
                var itemIcon = obj.transform.Find("ItemImage").GetComponent<Image>();
                var ItemAmount = obj.transform.Find("ItemAmount").GetComponent<Text>();
                itemIcon.sprite = item.Sprite;
                ItemAmount.text = item.Amount.ToString();
            }
            SetUIitems();
        }


        public void SetUIitems()
        {
            itemsInUi = ItemContent.GetComponentsInChildren<ItemInHolder>();
            for(int i = 0; i < Items.Count; i++)
            {
                itemsInUi[i].AddItem(Items[i]);
            }
        }
    }
}
