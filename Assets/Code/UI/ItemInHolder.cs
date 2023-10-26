using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pasta
{
    public class ItemInHolder : MonoBehaviour
    {
        ItemBase item;
        #region Item Info
        private Canvas itemInfoCanvas; // TODO: Change this to something.
        private Image ItemImage;
        private Text ItemName;
        private Text ItemDesc;
        private Text ItemFlavor;
        #endregion
        public void AddItem(ItemBase newItem)
        {
            item = newItem;
        }



        public void OpenItem(ItemBase item)
        {
            itemInfoCanvas.gameObject.SetActive(true);
            #region Get Info Canvas components
            ItemImage = itemInfoCanvas.transform.Find("ItemImage").GetComponent<Image>();
            ItemName = itemInfoCanvas.transform.Find("ItemName").GetComponent<Text>();
            ItemDesc = itemInfoCanvas.transform.Find("ItemDesc").GetComponent<Text>();
            ItemFlavor = itemInfoCanvas.transform.Find("ItemFlavor").GetComponent<Text>();
            #endregion
            ItemImage.sprite = item.Sprite;
            ItemName.text = item.Name;
            ItemDesc.text = item.Description;
            ItemFlavor.text = item.Flavor;
        }
    }
}
