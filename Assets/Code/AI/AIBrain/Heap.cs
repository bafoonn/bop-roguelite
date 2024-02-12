using System;

namespace Pasta
{
    public class Heap<T> where T : iHeapItem<T>
    {
        T[] items;

        // Current number of items in the heap
        int currentItemCount;

        // Constructor to initialize the heap with a specified maximum size
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        // Add an item to the heap
        public void Add(T item)
        {
            // Set the heap index of the item and add it to the array
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            // Perform a sorting operation to maintain heap property
            SortUp(item);
            currentItemCount++;
        }

        // Move an item up in the heap to maintain heap property
        void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while (true)
            {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    // Swap the item with its parent if necessary
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        // Remove and return the first item in the heap
        public T RemoveFirst()
        {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            // Perform a sorting operation to maintain heap property
            SortDown(items[0]);
            return firstItem;
        }

        // Move an item down in the heap to maintain heap property
        void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount)
                    {
                        // Determine which child to swap with
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }
                    // Swap the item with its child if necessary
                    if (item.CompareTo(items[swapIndex]) < 0)
                    {
                        Swap(item, items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        // Get the current number of items in the heap
        public int Count
        {
            get
            {
                return currentItemCount;
            }
        }

        // Check if the heap contains a specific item
        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        // Swap two items in the heap and update their indices
        void Swap(T itemA, T itemB)
        {
            items[itemA.HeapIndex] = itemB;
            items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }

    // Interface that items in the heap must implement
    public interface iHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
}
