using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{

	private LayerMask unwalkableMask;
	private Vector2 gridWorldSize;
	private float nodeRadius = 1.0f;
	Node[,] grid;

	float nodeDiameter;

	[HideInInspector]
	public int gridSizeX, gridSizeY;

	void Start()
	{
		unwalkableMask = LayerMask.GetMask("Obstacle");
		var colliderSize = GetComponent<BoxCollider>().size;
		gridWorldSize.x = colliderSize.x * 150;
		gridWorldSize.y = colliderSize.y * 150;
		
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
				grid[x, y] = new Node(walkable, worldPoint);
			}
		}
	}

	public Node NodeFromXYCoords(int x, int y)
	{
		return grid[x, y];
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


		if (grid != null)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}

public class Node
{

	public bool walkable;
	public Vector3 worldPosition;

	public Node(bool _walkable, Vector3 _worldPos)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
	}
}

