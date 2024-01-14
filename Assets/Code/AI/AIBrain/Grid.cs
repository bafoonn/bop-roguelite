using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Grid : MonoBehaviour
    {
        // Toggle to only display path gizmos for testing
        public bool displayGridGizmos;

		public Transform player; // TODO: Remove this just for testing.

        public LayerMask ObstacleMask;

        public Vector2 gridWorldSize;

        // Radius of each node in the grid
        public float nodeRadius;

		public LayerMask enemyLayer;

        Node[,] grid;

		float nodeDiameter;

		int gridSizeX, gridSizeY;

		public TerrainType[] walkableRegions; // Probs not used in but added just in case comment out if needed(if want to add weights).
		LayerMask walkableMask; // Probs not used in but added just in case comment out if needed(if want to add weights).
		Dictionary<int, int> walkableRegionsDict = new Dictionary<int, int>(); // Probs not used in but added just in case comment out if needed(if want to add weights).
		int penaltyMin = int.MaxValue; // Probs not used in but added just in case comment out if needed(if want to add weights).
		int penaltyMax = int.MinValue; // Probs not used in but added just in case comment out if needed(if want to add weights).
		public int obstacleProximityPenalty = 10;

		private void Awake()
		{
			nodeDiameter = nodeRadius * 2;

            // Calculate grid size based on world size and node diameter
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

			foreach (TerrainType region in walkableRegions) // Probs not used in but added just in case comment out if needed(if want to add weights).
			{
				walkableMask.value |= region.terrainMask.value;
				walkableRegionsDict.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
			}

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
			//Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
			Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

			for (int i = 0; i < gridSizeX; i++)
			{
				for (int e = 0; e < gridSizeY; e++)
				{
					//Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.up * (e * nodeDiameter + nodeRadius);
					Vector2 worldPoint = worldBottomLeft + Vector2.right * (i * nodeDiameter + nodeRadius) + Vector2.up * (e * nodeDiameter + nodeRadius);
					bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, ObstacleMask));

					int movementPenalty = 0; // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.

					Vector3 worldPointV3 = worldPoint; // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
					

					worldPointV3.z = 0;
					Ray ray = new Ray(worldPointV3 + Vector3.back * 50, Vector3.forward); // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
					RaycastHit hit;// Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
					if (Physics.Raycast(ray, out hit, 100, walkableMask)) // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
					{
						walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
					}

					if (!walkable)
					{
						movementPenalty += obstacleProximityPenalty;
					}


					if (grid != null) // Added here to prevent stupid error
					{
						grid[i, e] = new Node(walkable, worldPoint, i, e, movementPenalty);  // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
					}
					
				}
			}
			BlurPenalty(3); // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
		}

		// Convert world position to grid node
		public Node NodeFromWorldPos(Vector2 worldPos)
		{
			float precentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
			float precentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
			precentX = Mathf.Clamp01(precentX);
			precentY = Mathf.Clamp01(precentY);
			int x = Mathf.FloorToInt(Mathf.Clamp((gridSizeX) * precentX, 0, gridSizeX - 1));
			int y = Mathf.FloorToInt(Mathf.Clamp((gridSizeY) * precentY, 0, gridSizeY - 1));
			//int x = Mathf.RoundToInt((gridSizeX - 1) * precentX);
			//int y = Mathf.RoundToInt((gridSizeY - 1) * precentY);
			if (grid != null) // Added here to prevent stupid error
			{
				return grid[x, y];
			}
			else
			{
				return null;
			}
			
		}

		public Node ClosestWalkableNode(Node node)
		{
			int maxRadius = Mathf.Max(gridSizeX, gridSizeY) / 2;
			for (int i = 1; i < maxRadius; i++)
			{
				Node n = FindWalkableInRadius(node.gridX, node.gridY, i);
				if (n != null)
				{
					return n;

				}
			}
			return null;
		}

		Node FindWalkableInRadius(int centreX, int centreY, int radius)
		{

			for (int i = -radius; i <= radius; i++)
			{
				int verticalSearchX = i + centreX;
				int horizontalSearchY = i + centreY;

				// top
				if (InBounds(verticalSearchX, centreY + radius))
				{
					if (grid[verticalSearchX, centreY + radius].Walkable)
					{
						return grid[verticalSearchX, centreY + radius];
					}
				}

				// bottom
				if (InBounds(verticalSearchX, centreY - radius))
				{
					if (grid[verticalSearchX, centreY - radius].Walkable)
					{
						return grid[verticalSearchX, centreY - radius];
					}
				}
				// right
				if (InBounds(centreY + radius, horizontalSearchY))
				{
					if (grid[centreX + radius, horizontalSearchY].Walkable)
					{
						return grid[centreX + radius, horizontalSearchY];
					}
				}

				// left
				if (InBounds(centreY - radius, horizontalSearchY))
				{
					if (grid[centreX - radius, horizontalSearchY].Walkable)
					{
						return grid[centreX - radius, horizontalSearchY];
					}
				}

			}

			return null;

		}

		bool InBounds(int x, int y)
		{
			return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
		}


		void BlurPenalty(int blurSize) // Probs not used in but added just in case comment out if needed(if want to add weights)ALSO DELETE IF NOT USED.
		{
			int kernelSize = blurSize * 2 + 1;
			int kernelExtends = (kernelSize - 1) / 2;

			int[,] penaltiesHorizontal = new int[gridSizeX, gridSizeY];
			int[,] penaltiesVertical = new int[gridSizeX, gridSizeY];

			for (int y = 0; y < gridSizeY; y++)
			{
				for (int x = -kernelExtends; x <= kernelExtends; x++)
				{
					int sampleX = Mathf.Clamp(x, 0, kernelExtends);
					penaltiesHorizontal[0, y] += grid[sampleX, y].movementPenalty;
				}
				for (int x = 1; x < gridSizeX; x++)
				{
					int removeIndex = Mathf.Clamp(x - kernelExtends - 1, 0, gridSizeX);
					int addIndex = Mathf.Clamp(x + kernelExtends, 0, gridSizeX - 1);

					penaltiesHorizontal[x, y] = penaltiesHorizontal[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
				}
			}
			for (int x = 0; x < gridSizeX; x++)
			{
				for (int y = -kernelExtends; y <= kernelExtends; y++)
				{
					int sampleY = Mathf.Clamp(x, 0, kernelExtends);
					penaltiesVertical[x, 0] += penaltiesHorizontal[x, sampleY];
				}
				int blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, 0] / (kernelSize * kernelSize));
				grid[x, 0].movementPenalty = blurredPenalty;
				for (int y = 1; y < gridSizeY; y++)
				{
					int removeIndex = Mathf.Clamp(y - kernelExtends - 1, 0, gridSizeY);
					int addIndex = Mathf.Clamp(y + kernelExtends, 0, gridSizeY - 1);

					penaltiesVertical[x, y] = penaltiesVertical[x, y - 1] - penaltiesHorizontal[x, removeIndex] + penaltiesHorizontal[x, addIndex];
					blurredPenalty = Mathf.RoundToInt((float)penaltiesVertical[x, y] / (kernelSize * kernelSize));
					grid[x, y].movementPenalty = blurredPenalty;

					if (blurredPenalty > penaltyMax)
					{
						penaltyMax = blurredPenalty;
					}
					if (blurredPenalty < penaltyMin)
					{
						penaltyMin = blurredPenalty;
					}
				}
			}
		}

		public List<Node> GetNeighbours(Node node, int depth = 1)
		{
			List<Node> neighbours = new List<Node>();

			for (int x = -depth; x <= depth; x++)
			{
				for (int y = -depth; y <= depth; y++)
				{
					if (x == 0 && y == 0)
						continue;

					int checkX = node.gridX + x;
					int checkY = node.gridY + y;

					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					{
						neighbours.Add(grid[checkX, checkY]);
					}
				}
			}

			return neighbours;
		}

		// List to store the calculated path

		private void OnDrawGizmos()
		{
			Node playerNode = NodeFromWorldPos(player.position);
			Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
			if (grid != null && displayGridGizmos)
			{
				foreach (Node n in grid)
				{
					Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
					Gizmos.color = (n.Walkable) ? Color.white : Color.red;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
					if (playerNode == n)
					{
						Gizmos.color = Color.green;
					}
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
				}
				
			}
		}
	}

	[System.Serializable]
	public class TerrainType // Probs not used in but added just in case comment out if needed(if want to add weights).
	{
		public LayerMask terrainMask;
		public int terrainPenalty;
	}
}
