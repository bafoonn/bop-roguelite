using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Node : iHeapItem<Node>
    {
        // Indicates whether the node is walkable or an obstacle
        public bool Walkable;

        // World position of the node
        public Vector3 worldPosition;


        public int gCost;
        public int hCost;

        // Grid coordinates of the node
        [Header("Grid coordinates of the node")]
        public int gridX;
        public int gridY;

        public int movementPenalty; // Probs not used in but added just in case comment out if needed(if want to add weights).

        // Reference to the parent node for reconstructing the path
        public Node parent;

        int heapIndex;

        // Constructor for creating a new node
        public Node(bool walkable, Vector3 worldPos, int GridX, int GridY, int penalty) // Probs not used in but added just in case comment out if needed(if want to add weights).
        {
            Walkable = walkable;
            worldPosition = worldPos;
            gridX = GridX;
            gridY = GridY;
            movementPenalty = penalty; // Probs not used in but added just in case comment out if needed(if want to add weights).
        }

        // Total cost of the node (gCost + hCost)
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        // Property to get or set the heap index of the node
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }

        // Compare nodes based on their total cost (fCost) and cost (hCost)
        public int CompareTo(Node other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if(compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            // Returning the negation because the heap should be a min-heap
            return -compare;
        }
    }
}
