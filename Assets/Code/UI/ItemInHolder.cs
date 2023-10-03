using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ItemInHolder : MonoBehaviour
    {
        ItemBase item;
       public void AddItem(ItemBase newItem)
        {
            item = newItem;
        }
    }
}
