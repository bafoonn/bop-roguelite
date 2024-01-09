using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class PriorityQue<T> where T : IComparable<T>
	{
		private List<T> data;
		public int Count => data.Count;
		public PriorityQue()
		{
			data = new List<T>();
		}

		public void Enqueue(T item)
		{
			data.Add(item);

			int m_childIndex = Count - 1;

			while(m_childIndex > 0)
			{
				int m_parentIndex = (m_childIndex - 1) / 2;
				if (data[m_childIndex].CompareTo(data[m_parentIndex]) >= 0)
				{
					break; // IS CORRECT ORDER STOP
				}

				data.Swap(m_childIndex, m_parentIndex);
				m_childIndex = m_parentIndex;
			}
		}

		public T Dequeue()
		{
			if(Count == 0)
			{
				throw new InvalidOperationException("Que is empty");
			}

			T firstItem = data[0];

			int lastIndex = data.Count - 1;

			data[0] = data[lastIndex]; // Copy the last item to the front of data
			data.RemoveAt(lastIndex);

			lastIndex--;

			int parentIndex = 0;
			while (true)
			{
				int childIndex = parentIndex * 2 + 1;
				if(childIndex > lastIndex)
				{
					break; // No more children left 
				}

				int rightChild = childIndex + 1;
				if(rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) <= 0)
				{
					childIndex = rightChild;
				}

				if(data[parentIndex].CompareTo(data[childIndex]) <= 0)
				{
					break;
				}

				data.Swap(childIndex, parentIndex);

				parentIndex = childIndex;
			}

			return firstItem;
		}


		public T Peek()
		{
			if(Count == 0)
			{
				throw new InvalidOperationException("que is empty");
			}

			return data[0];
		}

		public void Clear()
		{
			data.Clear();
		}

		public bool Contains(T item)
		{
			foreach(T current in data)
			{
				if (item.Equals(current))
				{
					return true;
				}
			}
			return false;
		}

		public bool isConsistent()
		{
			if(Count == 0)
			{
				return true;
			}
			int lastIndex = data.Count - 1;
			for(int parentIndex = 0; parentIndex < data.Count; parentIndex++)
			{
				int leftChild = parentIndex * 2 + 1;
				int rightChild = parentIndex * 2 + 2;

				if(leftChild <= lastIndex && data[parentIndex].CompareTo(data[leftChild]) > 0)
				{
					return false;
				}
				if (rightChild <= lastIndex && data[parentIndex].CompareTo(data[rightChild]) > 0)
				{
					return false;
				}
			}
			return true;
		}


	}


    
}
