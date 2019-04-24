using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBlockPathFinder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {}

	public static List<RoomNode> FindPathBetween(RoomNode startNode, RoomNode endNode, ref TileBlock tileBlock) {
		
		List<RoomNode> path = new List<RoomNode>();

		List<RoomNode> openNodes = new List<RoomNode>();
		List<RoomNode> closedNodes = new List<RoomNode>();

		RoomNode currentRoomNode = endNode;
		closedNodes.Add (currentRoomNode);

		currentRoomNode.SetPartOfPath();
		path.Add (currentRoomNode);

		int amountofChecksDone = 0;

		while(MathUtils.GetManhattanDistance(startNode.gridLocation, currentRoomNode.gridLocation) > 0) {
			++amountofChecksDone;

			if(amountofChecksDone > 2400) {
				return null;
			}

			for(int x = (int)currentRoomNode.gridLocation.x - 1 ; x < (int)currentRoomNode.gridLocation.x + 2 ; x++) {
				for(int y = (int)currentRoomNode.gridLocation.y - 1 ; y < (int)currentRoomNode.gridLocation.y + 2 ; y++) {

					if(x > -1 && y > -1 && x < tileBlock.roomNodes.GetLength(0) && y < tileBlock.roomNodes.GetLength(1) && !closedNodes.Contains(tileBlock.roomNodes[x, y])) {

						RoomNode roomNodeInGrid = tileBlock.roomNodes[x, y];

						if(MathUtils.GetManhattanDistance(roomNodeInGrid.gridLocation, currentRoomNode.gridLocation) == 1 && roomNodeInGrid.CanBeUsedInPathfinding()) {
		
							if(!openNodes.Contains(roomNodeInGrid)) {
								openNodes.Add (roomNodeInGrid);
							
								roomNodeInGrid.SetParent(currentRoomNode);
								
								int moveCost = currentRoomNode.GetMoveCost() + 10;
								roomNodeInGrid.SetMoveCost(moveCost);
								
								int manHattanDistance = MathUtils.GetManhattanDistance(roomNodeInGrid.gridLocation, startNode.gridLocation);
								roomNodeInGrid.SetTotalMoveCost((manHattanDistance * 10) + moveCost);

							} else {

								int newMoveCost = currentRoomNode.GetMoveCost() + 10;
								if(roomNodeInGrid.GetMoveCost() < newMoveCost) { //iguess..

									roomNodeInGrid.SetParent(currentRoomNode);
									
									int moveCost = currentRoomNode.GetMoveCost() + 10;
									roomNodeInGrid.SetMoveCost(moveCost);
									
									int manHattanDistance = MathUtils.GetManhattanDistance(roomNodeInGrid.gridLocation, startNode.gridLocation);
									roomNodeInGrid.SetTotalMoveCost((manHattanDistance * 10) + moveCost);

								}
							}
						}
					}
				}
			}

			int lowestTotalScore = int.MaxValue;
			RoomNode lowestCostRoomNode = null;

			foreach(RoomNode roomNode in openNodes) {
				if(roomNode.GetTotalMoveCost() < lowestTotalScore) {
					lowestTotalScore = roomNode.GetTotalMoveCost();
					lowestCostRoomNode = roomNode;
				}
			}

			if(lowestCostRoomNode != null) {

				openNodes.Remove (lowestCostRoomNode);
				closedNodes.Add (lowestCostRoomNode);

				currentRoomNode = lowestCostRoomNode;

				if(currentRoomNode == startNode) {
					Logger.Log ("found start node!");

					path = AddAllParentsToPath(currentRoomNode);

					ClearAllParents(openNodes, closedNodes);

					return path;

				}

			} else {

				if(currentRoomNode == startNode) {
					Logger.Log ("current node = start node..");
				}

				Logger.Log ("?something went wrong");

				path = AddAllParentsToPath(currentRoomNode);
				return path;
			}
			
		}

		ClearAllParents(openNodes, closedNodes);
		return path;
	}

	private static List<RoomNode> AddAllParentsToPath(RoomNode lastRoomNode) {
		List<RoomNode> roomNodes = new List<RoomNode>();

		RoomNode currentNode = lastRoomNode;
		while(currentNode.GetParentNode() != null) {

			if(roomNodes.Count > 1200) {
				Logger.Log ("stuck....?");
				return new List<RoomNode>();
			}

			roomNodes.Add (currentNode.GetParentNode());


			Direction directionToParentNode = MathUtils.GetDirection(currentNode.gridLocation, currentNode.GetParentNode().gridLocation);
			currentNode.GetParentNode().AddConnectionDirection(directionToParentNode);
			
			Direction directionFromLastNode = MathUtils.GetDirection(currentNode.GetParentNode().gridLocation, currentNode.gridLocation);
			currentNode.AddConnectionDirection(directionFromLastNode);
			
			currentNode.SetPartOfPath();

			currentNode = currentNode.GetParentNode();
		}

		return roomNodes;

	}

	private static void ClearAllParents(List<RoomNode> openNodes, List<RoomNode> closedNodes) {
		foreach(RoomNode roomNode in openNodes) {	
			if(roomNode.GetParentNode()) {
				roomNode.SetParent(null);
			}
		}

		foreach(RoomNode roomNode in closedNodes) {	
			if(roomNode.GetParentNode()) {
				roomNode.SetParent(null);
			}
		}
	}
}
