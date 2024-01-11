using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Pasta
{
    public class Pathfinding : MonoBehaviour
    {
        PathRequestManager requestManager;
        // References to the enemy, target, and the grid
        Grid grid;

        private void Awake()
        {
            // Get the Grid component attached to the same GameObject
            grid = GetComponent<Grid>();
            requestManager = GetComponent<PathRequestManager>();
        }

        private void Update()
        {
        }

        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
		{
            StartCoroutine(FindPath(startPos, targetPos));
		}

        // A* Pathfinding algorithm to find the shortest path from startPos to targetPos
        IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;
            // Convert world positions to grid nodes
            Node startNode = grid.NodeFromWorldPos(startPos);
            Node targetNode = grid.NodeFromWorldPos(targetPos);


            if(startNode.Walkable && targetNode.Walkable)
			{
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
                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    // Check neighbors and update costs
                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.Walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        // Calculate the new movement cost to the neighbour
                        int newMomevementCostToNeigbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newMomevementCostToNeigbour < currentNode.gCost || !openSet.Contains(neighbour))
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
            
            yield return null;
			if (pathSuccess)
			{
                waypoints = RetracePath(startNode, targetNode);
            }
            requestManager.Finished(waypoints, pathSuccess);
        }

        // Retrace the path from endNode to startNode and store it in grid.path
        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            Vector3[]waypoints = simplifyPath(path);
            // Reverse the list to get the correct order from start to end
            Array.Reverse(waypoints);
            return waypoints;

            
        }

        Vector3[] simplifyPath(List<Node> path)
		{
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for(int i = 1; i < path.Count; i++)
			{
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if(directionNew != directionOld)
				{
                    waypoints.Add(path[i].worldPosition);
				}
                directionOld = directionNew;
			}
            return waypoints.ToArray();
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
