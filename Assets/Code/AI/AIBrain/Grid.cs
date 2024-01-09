using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class Grid : MonoBehaviour
    {
		public Transform player;
        public LayerMask ObstacleMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        Node[,] grid;

		float nodeDiameter;
		int gridSizeX, gridSizeY;

		private void Start()
		{
			nodeDiameter = nodeRadius = 0.5f;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
			CreateGrid();
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
					grid[i, e] = new Node(walkable, worldPoint);
				}
			}
		}

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

		private void OnDrawGizmos()
		{
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
			if(grid != null)
			{
				Node playerNode = NodeFromWorldPos(player.position);
				foreach(Node node in grid)
				{
					Gizmos.color = (node.Walkable) ? Color.white : Color.red;
					if(playerNode == node)
					{
						Gizmos.color = Color.green;
					}
					Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .1f));
				}
			}
		}
	}
}
