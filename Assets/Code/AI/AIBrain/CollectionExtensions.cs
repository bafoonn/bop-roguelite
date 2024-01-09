using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public static class CollectionExtensions
    {
        public static void Swap<T>(this IList<T> list, int first, int second)
		{
            if(list == null)
			{
                throw new System.ArgumentException(nameof(list));
			}

            if(first < 0 || second < 0 || first >= list.Count || second >= list.Count)
			{
                throw new System.ArgumentOutOfRangeException();
			}

            T temp = list[first];
            list[first] = list[second];
            list[second] = temp;
		}
    }
}
