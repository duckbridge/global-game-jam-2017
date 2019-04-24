using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {

	private List<PathFindNode> closedNodes = new List<PathFindNode>();
	private List<PathFindNode> openNodes = new List<PathFindNode> ();

	private const float extraDistancePenalityForNonNeighbour = 1.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}


	public List<PathFindNode> FindBestPathToTargetFrom(List<PathFindNode> allNodes, Vector3 currentPosition, Vector3 target) {

		this.openNodes = allNodes;
		this.closedNodes = new List<PathFindNode> ();

		PathFindNode nodeClosestToTarget = FindNodeClosestTo(openNodes, target);

		PathFindNode currentStartNode = nodeClosestToTarget;
		closedNodes.Add (nodeClosestToTarget);

		while(currentStartNode != null) {
			currentStartNode = GetNeighbourIfExistOrClose(currentStartNode, currentPosition, target);
			if (currentStartNode) {
				closedNodes.Add (currentStartNode);
			}
		}

		closedNodes.Reverse();

		return closedNodes;
	}

	private PathFindNode GetNeighbourIfExistOrClose(PathFindNode currentEndNode, Vector3 currentPosition, Vector3 targetPosition) {

		float distanceBetweenCurrentAndNode = MathUtils.GetDistance2D(currentPosition, currentEndNode.transform.position);

		if (currentEndNode.neighbours.Length > 0) {
			PathFindNode pathfindNode = currentEndNode.neighbours [0];

			float distanceBetweenCurrentAndNeighbour = MathUtils.GetDistance2D(currentPosition, pathfindNode.transform.position);
			if (distanceBetweenCurrentAndNeighbour < (distanceBetweenCurrentAndNode * extraDistancePenalityForNonNeighbour)) {
				return pathfindNode;
			}
		}

		return null;
	}

	private PathFindNode FindNeighbourClosestToNode(PathFindNode currentEndNode, Vector3 currentPosition, Vector3 targetPosition) {
		float closestDistance = float.MaxValue;
		PathFindNode closestNode = null;

		float distanceBetweenGoalAndEndNode = MathUtils.GetDistance2D(currentPosition, currentEndNode.transform.position);

		foreach(PathFindNode pathfindNode in currentEndNode.neighbours) {
			float distanceBetweenCurrentAndNeighbour = MathUtils.GetDistance2D(currentPosition, pathfindNode.transform.position);
			if(distanceBetweenCurrentAndNeighbour < closestDistance && distanceBetweenCurrentAndNeighbour < distanceBetweenGoalAndEndNode) {
				closestDistance = distanceBetweenCurrentAndNeighbour;
				closestNode = pathfindNode;
			}
		}

		return closestNode;
	}

	private PathFindNode FindNodeClosestTo(List<PathFindNode> nodesToUse, Vector3 goalPosition) {
		float closestDistance = float.MaxValue;
		PathFindNode closestNode = null;

		foreach(PathFindNode pathFindNode in nodesToUse) {
			float distance = MathUtils.GetDistance2D(goalPosition, pathFindNode.transform.position);
			if(distance < closestDistance) {
				closestDistance = distance;
				closestNode = pathFindNode;
			}
		}

		return closestNode;
	}

}
