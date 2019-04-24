using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GridMap : DispatchBehaviour {

	private int width, height;
	private GridCell[,] cells;

	public float cellSize = 0.5f;
	private Bounds bounds;
	private Vector3 topLeftCorner;
	public float maxPathFindTime = 1000f;

	public GameObject gridCellOverlay;
	public bool drawOverlay = false;
	private List<GameObject> overlay = new List<GameObject>();
	
	public float ceilingOffset = 2f;
	public float objectFindingRayLength = 2f;

	public static bool isBusy = false;

	// Use this for initialization
	void Awake () {
		bounds = GetComponent<Renderer>().bounds;

		topLeftCorner = bounds.center - bounds.extents + new Vector3(0, bounds.size.y, 0);

		width = Mathf.RoundToInt(bounds.size.x / cellSize);
		height = Mathf.RoundToInt(bounds.size.z / cellSize);

		cells = new GridCell[width, height];

		ScanGridMap();
	}

	public void ScanGridMap() {
		for(int x = 0; x < width ; x++) {
			for(int y = 0 ; y < height; y++) {
				Vector3 currentPosition = topLeftCorner + new Vector3(x * cellSize, ceilingOffset, y * cellSize);

				RaycastHit hit;
				GridCell gridCell = new GridCell();
				cells[x, y] = gridCell;

				
				Debug.DrawLine(currentPosition, currentPosition - Vector3.up * objectFindingRayLength, Color.red, 5f);
				if(Physics.Raycast(currentPosition, -Vector3.up, out hit, objectFindingRayLength)) {
					gridCell.height = hit.point.y;
					if(hit.collider.gameObject.GetComponent<Wall>()) {
						gridCell.walkable = false;
					}
				}
			}
		}
	}

	public void ReScanGridMap() { //called when a PathFollowingComponent or a zombie 'sees' a movableObject in front of him...or sometrhing?

	}

	public void FindAStarPathBetween(Vector3 startPosition, Vector3 endPosition) { //has to be seperate!!
		if(GridMap.isBusy) { Debug.Log ("isbusy"); return; } 
		GridMap.isBusy = true;

		var currentTime = DateTime.Now;

		//ScanGridMap(); need a better solution!
		
		GridPosition start = GetGridPosition(startPosition);
		GridPosition end = GetGridPosition(endPosition);

		Dictionary<GridPosition, Node> closedNodes = new Dictionary<GridPosition, Node>();
		Dictionary<GridPosition, Node> openNodes = new Dictionary<GridPosition, Node>();
		Dictionary<GridPosition, Node> allNodes = new Dictionary<GridPosition, Node>();

		Node startNode = new Node();
		startNode.Fcost = end.Distance(start);

		startNode.Gcost = 0f;

		allNodes.Add(start, startNode);
		openNodes.Add(start, startNode);

		
		while(openNodes.Count > 0) {
			if((DateTime.Now - currentTime).Milliseconds > maxPathFindTime) {
				break;
			}

			KeyValuePair<GridPosition, Node> best = openNodes.Aggregate((a, b) => a.Value.Fcost < b.Value.Fcost ? a : b); //find cheapest node
			openNodes.Remove(best.Key);
			closedNodes.Add(best.Key, best.Value); //add node to move to to closed list

			//AssertIfEndGridIsFoundAndBackTrack!!
			if(best.Key.Equals(end)) {
				Debug.Log("found end!");
				List<Vector3> path = new List<Vector3>();
				Node bestNode = best.Value;

				path.Add(endPosition);

				//backtrack!
				while(bestNode != null && !bestNode.parentGridPosition.Equals(GridPosition.Zero)) {
					path.Insert(0, GetWorldPosition(bestNode.parentGridPosition));
					bestNode = allNodes[bestNode.parentGridPosition];
					
					if(drawOverlay) {
						GameObject gcOverlay = (GameObject) GameObject.Instantiate(gridCellOverlay, GetWorldPosition(bestNode.parentGridPosition), Quaternion.identity);
						gcOverlay.GetComponentInChildren<SpriteRenderer>().color = Color.red;
						gcOverlay.transform.parent = GameObject.Find("Paths").transform;
						overlay.Add(gcOverlay);
					}
				}
				Debug.Log("took : " + (DateTime.Now - currentTime).Milliseconds + "ms to generate a path!");
				DispatchMessage("OnPathFound", path);

				GridMap.isBusy = false;
			}

			//LoopThroughWalkablewNeighBours
			foreach(GridPosition gridPosition in GetNeighbours(best.Key).Where(gc => GetCell(gc).walkable)) {
				Node alreadyExistingNode;
				if(closedNodes.TryGetValue(gridPosition, out alreadyExistingNode)) {
					continue;
				}

				float tenativeGcost = best.Value.Gcost + 1;
				
				Node currentNode;
				if(!openNodes.TryGetValue(gridPosition, out currentNode)) { //AddNodeToOpenAndAllIfNotAlreadyAdded
					currentNode = new Node();
					allNodes.Add(gridPosition, currentNode);
					openNodes.Add(gridPosition, currentNode);
				}

				if(tenativeGcost < currentNode.Gcost) { //AssertIfNodeGCostIsLowerThanTenativeGCost
					currentNode.Gcost = tenativeGcost;
					currentNode.parentGridPosition = best.Key;
					currentNode.Fcost = tenativeGcost + gridPosition.Distance(end);
				}
			}
		}
		GridMap.isBusy = false;
	}

	public GridPosition GetGridPosition(Vector3 worldPosition)
	{
		worldPosition -= topLeftCorner;
		return new GridPosition { x = Mathf.FloorToInt(worldPosition.x / cellSize), y = Mathf.FloorToInt(worldPosition.z / cellSize) };
	}

	//Convert a grid position into a world position on the navmesh
	public Vector3 GetWorldPosition(GridPosition gridPosition)
	{
		var worldPosition = new Vector3(gridPosition.x * cellSize, GetCell(gridPosition).height, gridPosition.y * cellSize);
		return worldPosition + new Vector3(topLeftCorner.x, 0, topLeftCorner.z);
	}

	public List<GridPosition> GetNeighbours(GridPosition currentGridPosition, int distance = 1) {
		List<GridPosition> neighbours = new List<GridPosition>();
		
		for(int x = -distance; x <= distance; x ++ ) {
			for(int y = -distance; y <= distance; y ++) {
				GridPosition currentPosition = new GridPosition(x + currentGridPosition.x, y + currentGridPosition.y);		
				if(currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < width && currentPosition.y < height)
					neighbours.Add(currentPosition);
			}
		}
		return neighbours;
	}
	
	public GridCell GetCell(GridPosition gridPosition) {
		return cells[gridPosition.x, gridPosition.y];
	}

	public Vector2 GetSize() {
		return new Vector2(width, height);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
