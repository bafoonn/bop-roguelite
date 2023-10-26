using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class OpenInventoryItem : MonoBehaviour
    {
        private ItemsUI uiItems;
        private ItemBase item;
        private Canvas itemInfoCanvas;


        private void Start()
        {
            uiItems = GetComponent<ItemsUI>();
        }


        public void CloseItemInfo()
        {
            itemInfoCanvas.gameObject.SetActive(false);
        }
    }
}
