using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Grid : MonoBehaviour
    {
        // Toggle to only display path gizmos for testing
        public bool onlyDisplayPathGizmos;

		public Transform player; // TODO: Remove this just for testing.

        public LayerMask ObstacleMask;

        public Vector2 gridWorldSize;

        // Radius of each node in the grid
        public float nodeRadius;

        Node[,] grid;

		float nodeDiameter;

		int gridSizeX, gridSizeY;

		private void Start()
		{
			nodeDiameter = nodeRadius = 0.5f;

            // Calculate grid size based on world size and node diameter
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
			CreateGrid();
		}

        // Max size property for external use
        public int MaxSize
		{
			get
			{
				return gridSizeX * gridSizeY;
			}
		}

		void CreateGrid()
		{
			grid = new Node[gridSizeX, gridSizeY];
			Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

			for(int i = 0; i < gridSizeX; i++)
			{
				for (int e = 0; e < gridSizeY; e++)
				{
					Vector3 worldPoint = worldbottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.up * (e * nodeDiameter + nodeRadius);
					bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, ObstacleMask));
					grid[i, e] = new Node(walkable, worldPoint, i, e);
				}
			}
		}

        // Convert world position to grid node
        public Node NodeFromWorldPos(Vector3 worldPos)
		{
			float precentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
			float precentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
			precentX = Mathf.Clamp01(precentX);
			precentY = Mathf.Clamp01(precentY);

			int x = Mathf.RoundToInt((gridSizeX - 1) * precentX);
			int y = Mathf.RoundToInt((gridSizeY - 1) * precentY);
			return grid[x, y];
		}

		public List<Node> GetNeighbours(Node node)
		{
			List<Node> neighbours = new List<Node>();

			for(int x = -1; x <= 1; x++)
			{
                for (int y = -1; y <= 1; y++)
				{
					if(x == 0 && y == 0)
					{
						continue;
					}
					int checkX = node.gridX + x;
					int checkY = node.gridY + y;

					if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					{
						neighbours.Add(grid[checkX,checkY]);
					}
				}

            }

			return neighbours;
		}

        // List to store the calculated path
        public List<Node> path;

		private void OnDrawGizmos()
		{
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
			if (onlyDisplayPathGizmos)
			{
				if(path != null)
				{
					foreach(Node n in path)
					{
                        Gizmos.color = Color.black;
                        Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                    }
				}
			}
			else
			{
                if (grid != null)
                {
                    Node playerNode = NodeFromWorldPos(player.position); // This is how we get random pos and player pos // TODO: Remove this just for testing.
                    foreach (Node node in grid)
                    {
                        Gizmos.color = (node.Walkable) ? Color.white : Color.red;
                        if (path != null)
                        {
                            if (path.Contains(node))
                            {
                                Gizmos.color = Color.black;
                            }
                        }
                        if (playerNode == node)
                        {
                            Gizmos.color = Color.green;
                        }
                        Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
                    }
                }
            }
			
		}
	}
}
