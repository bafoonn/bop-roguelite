using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Items : Singleton<Items>
    {
        public override bool PersistSceneLoad => true;
        private ItemBase[] _items = null;

        protected override void Init()
        {
            base.Init();
            _items = Resources.LoadAll<ItemBase>("Items");
        }

        /// <returns>All items that can be looted.</returns>
        public IList<ItemBase> GetRewards()
        {
            var list = new List<ItemBase>();
            foreach (var item in _items)
            {
                if (item.CanLoot) list.Add(item);
            }
            return list;
        }

        public IList<ItemBase> GetItems()
        {
            return _items;
        }
    }
}
