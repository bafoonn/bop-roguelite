using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class ItemsUI : Singleton<ItemsUI>
    {
        public List<ItemBase> Items = new List<ItemBase>();

        public Transform ItemContent;
        public GameObject ItemHolder;

        public ItemInHolder[] itemsInUi;

        public override bool DoPersist => false;

        public void Add(ItemBase item)
        {
            if (!Items.Contains(item))
            {
                Items.Add(item);
            }
            ListItems();
        }

        public void ListItems()
        {
            foreach (Transform item in ItemContent)
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
            for (int i = 0; i < Items.Count; i++)
            {
                itemsInUi[i].AddItem(Items[i]);
            }
        }
    }
}
