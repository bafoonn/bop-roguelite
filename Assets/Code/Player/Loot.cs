using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Loot : MonoBehaviour, ICollection<IItem>
    {
        private List<IItem> _items = new();
        public IItem this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(IItem item)
        {
            item.Loot();
            _items.Add(item);
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.Drop();
            }
            _items.Clear();
        }

        public bool Contains(IItem item) => _items.Contains(item);
        public void CopyTo(IItem[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public IEnumerator<IItem> GetEnumerator() => _items.GetEnumerator();

        public bool Remove(IItem item)
        {
            bool removed = _items.Remove(item);
            if (removed)
            {
                item.Drop();
            }
            return removed;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
