using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Node 
    {
        public bool Walkable;
        public Vector3 worldPosition;

        public Node(bool walkable, Vector3 worldPos)
		{
            Walkable = walkable;
            worldPosition = worldPos;        
		}
    }
}
