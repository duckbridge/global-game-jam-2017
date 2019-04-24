using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBuilderHelper : MonoBehaviour {

	public static List<RoomNode> CreateMaze(RoomNode startNode, RoomNode endNode, ref RoomNode[,] roomNodesInGrid, 
		Vector2 startNodeDirection,
		Vector2 endNodeDirection) {

		RoomNode currentRoomNode = endNode;
		currentRoomNode.SetPartOfPath();

		List<RoomNode> closedNodes = new List<RoomNode>();
		closedNodes.Add (currentRoomNode);

		while(currentRoomNode.gridLocation != startNode.gridLocation) {

			List<Vector2> directions = new List<Vector2>();

			if(currentRoomNode == endNode) {

				directions.Add (endNodeDirection); //left
			
			} else if(currentRoomNode == startNode) {
			
				directions.Add (startNodeDirection); //right

			} else {
				if((int)currentRoomNode.gridLocation.x - 1 > -1 &&
			       !closedNodes.Contains(roomNodesInGrid[(int)currentRoomNode.gridLocation.x - 1, (int)currentRoomNode.gridLocation.y]) &&
					roomNodesInGrid[(int)currentRoomNode.gridLocation.x - 1, (int)currentRoomNode.gridLocation.y].CanBeUsedInMazeAndNormalPathfinding()) {
							
					directions.Add (new Vector2(-1, 0)); //left
				
				}

				if((int)currentRoomNode.gridLocation.x + 1 < roomNodesInGrid.GetLength(0) &&
		   			!closedNodes.Contains(roomNodesInGrid[(int)currentRoomNode.gridLocation.x + 1, (int)currentRoomNode.gridLocation.y]) &&
					roomNodesInGrid[(int)currentRoomNode.gridLocation.x + 1, (int)currentRoomNode.gridLocation.y].CanBeUsedInMazeAndNormalPathfinding()) {
						
					directions.Add (new Vector2(1, 0)); //right
				}

				if((int)currentRoomNode.gridLocation.y - 1 > -1 &&
				   !closedNodes.Contains(roomNodesInGrid[(int)currentRoomNode.gridLocation.x, (int)currentRoomNode.gridLocation.y - 1]) &&
					roomNodesInGrid[(int)currentRoomNode.gridLocation.x, (int)currentRoomNode.gridLocation.y - 1].CanBeUsedInMazeAndNormalPathfinding()) {

					directions.Add (new Vector2(0, -1)); //down
				}
				
				if((int)currentRoomNode.gridLocation.y + 1 < roomNodesInGrid.GetLength(1) &&
				   !closedNodes.Contains(roomNodesInGrid[(int)currentRoomNode.gridLocation.x, (int)currentRoomNode.gridLocation.y + 1]) &&
					roomNodesInGrid[(int)currentRoomNode.gridLocation.x, (int)currentRoomNode.gridLocation.y + 1].CanBeUsedInMazeAndNormalPathfinding()) {

					if(roomNodesInGrid[(int)currentRoomNode.gridLocation.x, (int)currentRoomNode.gridLocation.y + 1] != startNode) {
						directions.Add (new Vector2(0, 1)); //up
					}
				}
			}

			if(directions.Count > 0) {

				int directionIndex = Random.Range (0, directions.Count);
				Vector2 chosenDirection = directions[directionIndex];

				RoomNode roomNodeInGrid = roomNodesInGrid[(int)(currentRoomNode.gridLocation.x + chosenDirection.x),(int)(currentRoomNode.gridLocation.y + chosenDirection.y)];

				Direction direction = MathUtils.Vector2AsDirection(chosenDirection);
				currentRoomNode.AddConnectionDirection(direction);
				roomNodeInGrid.AddConnectionDirection(MathUtils.GetInverse(direction));

				roomNodeInGrid.SetParent(currentRoomNode);
				roomNodeInGrid.SetPartOfPath();

				closedNodes.Add (roomNodeInGrid);

				currentRoomNode = roomNodeInGrid;

			} else {

				if(currentRoomNode.GetParentNode() == null) {
					Logger.Log ("stuck?");
					return null;
				}
				Logger.Log ("backtracking");
				currentRoomNode = currentRoomNode.GetParentNode();
			}
		}

		return closedNodes;
	}
}
