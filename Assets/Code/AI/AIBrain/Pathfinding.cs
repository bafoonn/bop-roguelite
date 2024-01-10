using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Pathfinding : MonoBehaviour
    {
        // References to the enemy, target, and the grid
        public Transform enemy, target;
        Grid grid;

        private void Awake()
        {
            // Get the Grid component attached to the same GameObject
            grid = GetComponent<Grid>();
        }

        private void Update()
        {
            // Find a path from the enemy's position to the target's position
            FindPath(enemy.position, target.position);
        }

        // A* Pathfinding algorithm to find the shortest path from startPos to targetPos
        void FindPath(Vector3 startPos, Vector3 targetPos)
        {
            // Convert world positions to grid nodes
            Node startNode = grid.NodeFromWorldPos(startPos);
            Node targetNode = grid.NodeFromWorldPos(targetPos);

            // Initialize open and closed sets for nodes
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            // Loop until the open set is empty
            while (openSet.Count > 0)
            {
                // Get the node with the lowest total cost from the open set
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // Path found.
                if(currentNode == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                // Check neighbors and update costs
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if(!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // Calculate the new movement cost to the neighbour
                    int newMomevementCostToNeigbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMomevementCostToNeigbour < currentNode.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMomevementCostToNeigbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        // Retrace the path from endNode to startNode and store it in grid.path
        void RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            // Reverse the list to get the correct order from start to end
            path.Reverse();

            grid.path = path;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            // Diagonal movement is more costly (14) than vertical or horizontal movement (10)
            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
