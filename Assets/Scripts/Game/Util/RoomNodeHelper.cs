using UnityEngine;
using System.Collections;

public class RoomNodeHelper {

	public static RoomNode GetRoomNodeAt(TileBlock tileBlock, Vector2 localGridLocation) {
		RoomNode roomNodeToReturn = null;
		
		if(localGridLocation.x < tileBlock.roomNodes.GetLength(0) && localGridLocation.y < tileBlock.roomNodes.GetLength(1)
		   && localGridLocation.x > -1 && localGridLocation.y > -1) {
			
			roomNodeToReturn = tileBlock.roomNodes[(int)localGridLocation.x, (int)localGridLocation.y];
		}
		
		return roomNodeToReturn; 
	}
}
