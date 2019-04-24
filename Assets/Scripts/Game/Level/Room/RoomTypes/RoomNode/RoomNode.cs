using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomNode : MonoBehaviour {

	public bool isVisited {get; set;}
	public int version {get; set;}
	public string roomPrefix {get; set;}
	public TileType tileType {get; set;}
	public Vector2 worldGridLocation {get; set;}
	public Vector2 gridLocation {get; set;}
	public bool isPartOfPath{ get; set; }

	private bool isPlayerNode = false;
	private bool isPlayerSpawnNode = false;
	private bool canBeVisited {get; set;}
	
	private Direction endPointDirection = Direction.NONE;

	private string villageSidePostFix = "";

	private Room room;
	private RoomNodeType roomNodeType = RoomNodeType.Normal;

	private List<Direction> connectionDirections;
	private bool canBeUsedInPathfinding = true;
	private bool canBeUsedInMazePathfinding = true;

	private RoomNode parentNode;

	private int totalMoveCost = 0;
	private int moveCost = 0;

	private PointOfInterest pointOfInterest = PointOfInterest.Dungeon_UP;

	private bool hasPointOfInterest = false;
	private bool isDungeonEntrance = false;
	private bool isDungeonBoss = false;

	private bool isOccupied = false;
	private TileBlock tileBlock;
	private int roomNodeID = -1;
	private TileType connectedTileType = TileType.none;

	private bool canConnectToEntrance = true;
	private string villageName = "village";

	public void PrepareRoomForSpawning(ref Player player, bool isMaze = false) {

		if(room != null) {
			Logger.Log ("already has room!");
			return;
		}

		roomPrefix = "Rooms/outside/" + tileType.ToString() + "/default";

		if(connectionDirections != null) {
			string roomName = "";

			if(endPointDirection != Direction.NONE) {
				roomName += endPointDirection.ToString().ToLower();
			}

			DecideRoomName(ref roomName);

			if(connectedTileType != TileType.none && connectedTileType != tileType) {
				roomPrefix = "Rooms/outside/" + tileType.ToString() + "/"  + connectedTileType.ToString() + "/" + roomName;
			} else {
				roomPrefix = "Rooms/outside/" + tileType.ToString() + "/" + roomName;
			}
		
		} else if (endPointDirection != Direction.NONE) {

			string roomName = DecideRoomNameBasedOnEndPoint();

			if(connectedTileType != TileType.none && connectedTileType != tileType) {
				roomPrefix = "Rooms/outside/" + tileType.ToString() + "/"  + connectedTileType.ToString() + "/" + roomName;
			} else {
				roomPrefix = "Rooms/outside/" + tileType.ToString() + "/" + roomName;
			}
		}

		if(!canBeVisited) {

			roomPrefix = "Rooms/empty";

		} else if(isPlayerSpawnNode) {

			roomPrefix = "Rooms/villages/playerroom/version"+version+"/playerroom";

		} else if(roomNodeType == RoomNodeType.VillageCenter) {

			roomPrefix = "Rooms/villages/" + villageName + "/version" + version + "/villagecenter";
		
		} else if(roomNodeType == RoomNodeType.Village || roomNodeType == RoomNodeType.VillageExit) {

			roomPrefix = "Rooms/villages/" + villageName + "/version" + version + "/villagepart" + villageSidePostFix;
		}

		else if(hasPointOfInterest) {
			string poiName = pointOfInterest.ToString().Split('_')[0];
			roomPrefix = "Rooms/outside/" + tileType.ToString() + "/poi/" + poiName.ToLower();
		} else if(isDungeonBoss) {
			roomPrefix = "Rooms/outside/" + tileType.ToString() + "/boss";
		} else if(isDungeonEntrance) {
			roomPrefix = "Rooms/outside/" + tileType.ToString() + "/entrance" + GetEntrancePostFix();
		}
	
		Logger.Log (roomPrefix);

		string[] splittedRoomPrefix = this.roomPrefix.Split ('/');
		string roomNodeName = splittedRoomPrefix[splittedRoomPrefix.Length - 1];

		this.name = roomNodeName;

		if (!isMaze) {
			if (this.name.Equals (player.GetNameOfPlayerSpawnRoomNode ())) {
				RoomNodeSpawnerUtil.SpawnRoom (this);
				player.SetStartRoomNode(this);
			}
		} else {
			RoomNodeSpawnerUtil.SpawnRoom (this);
		}
	}
	
	private string DecideRoomNameBasedOnEndPoint() {

		string roomName =  "default";
	
		roomName = endPointDirection.ToString().ToLower();
	
		if(roomName.Contains("left") || roomName.Contains("right")) {
			roomName = "horizontal";
		}
		
		if(roomName.Contains("up") || roomName.Contains("down")) {
			roomName = "vertical";	
		}

		Logger.Log ("decided on name " + roomName + " based on endpoint");

		return roomName;
	}

	private void DecideRoomName(ref string roomName) {

		Vector4 allDirections = Vector4.zero;

		bool containsLeft = connectionDirections.Contains(Direction.LEFT);
		bool containsRight = connectionDirections.Contains(Direction.RIGHT);
		bool containsUp = connectionDirections.Contains(Direction.UP);
		bool containsDown = connectionDirections.Contains(Direction.DOWN);

		if(containsLeft) {
			allDirections += new Vector4(1, 0, 0, 0);
			AppendToString(ref roomName, "left", true);
		}

		if(containsRight) {
			allDirections += new Vector4(0, 1, 0, 0);
			AppendToString(ref roomName, "right", true);
		}
		
		if(containsUp) {
			allDirections += new Vector4(0, 0, 1, 0);
			AppendToString(ref roomName, "up", false);
		}

		if(containsDown) {
			allDirections += new Vector4(0, 0, 0, 1);
			AppendToString(ref roomName, "down", false);
		}

		roomName = CorrectRoomName(roomName);

		if((allDirections.x + allDirections.y + allDirections.z + allDirections.w == 4) ||
		   (allDirections.x + allDirections.y + allDirections.z == 3 && endPointDirection == Direction.DOWN) ||
		   (allDirections.y + allDirections.z + allDirections.w == 3 && endPointDirection == Direction.LEFT) ||
		   (allDirections.x + allDirections.z + allDirections.w == 3 && endPointDirection == Direction.RIGHT) ||
		   (allDirections.x + allDirections.y + allDirections.w == 3 && endPointDirection == Direction.UP)) {

			roomName = "cross";
		}
	}

	public void AddConnectionDirection(Direction direction) {
		if(connectionDirections == null) {
			connectionDirections = new List<Direction>();
		}

		connectionDirections.Add (direction);
	}

	public List<Direction> GetConnectedDirections() {
		if(connectionDirections == null) {
			connectionDirections = new List<Direction>();
		}
		return connectionDirections;
	}

	public void SetTileType(TileType tileType) {
		this.tileType = tileType;
	}

	public void SetPlayerNode() {
		this.isPlayerNode = true;
	}

	public void SetEndPointDirectionAndTileType(Direction endPointDirection, TileType connectedTileType) {
		this.connectedTileType = connectedTileType;
		this.endPointDirection = endPointDirection;
	}

	public Direction GetEndpointDirection() {
		return this.endPointDirection;
	}

	public RoomNodeType GetRoomNodeType() {
		return roomNodeType;
	}

	public void SetPlayerSpawnNode() {
		this.isPlayerSpawnNode = true;
	}

	
	public void SetDungeonEntranceNode() {
		this.isDungeonEntrance = true;
	}

	
	public void SetDungeonBossNode() {
		this.isDungeonBoss = true;
	}

	public void SetRoomNodeType(RoomNodeType roomNodeType) {
		this.roomNodeType = roomNodeType;
	}

	public void SetVillageSidePostFix(string villageSidePostFix) {
		this.villageSidePostFix = villageSidePostFix;
	}

	public void SetUnusableForPathfinding() {
		this.canBeUsedInPathfinding = false;
	}

	public void SetUnusableForMazePathfinding() {
		this.canBeUsedInMazePathfinding = false;
	}

	public void DisableVisiting() {
		canBeVisited = false;
	}

	public void EnableVisiting() {
		canBeVisited = true;
	}

	public bool CanBeVisited() {
		return canBeVisited;
	}

	public Room GetRoom() {
		return room;
	}

	public TileType GetTileType() {
		return tileType;
	}

	public void SetPartOfPath() {
		isPartOfPath = true;
	}

	private void AppendToString(ref string source, string stringToAppend, bool insertBeforeStart) {

		if(source.Contains(stringToAppend)) {
			Logger.Log ("string already contains strToAppend " + source + "," + stringToAppend);
			return;
		}

		if(insertBeforeStart) {
			source = stringToAppend + source;	
		} else {
			source += stringToAppend;	
		}
	}

	public bool CanBeUsedInPathfinding() {
		return canBeUsedInPathfinding;
	}

	public bool CanBeUsedInMazePathfinding() {
		return canBeUsedInMazePathfinding;
	}

	public bool CanBeUsedInMazeAndNormalPathfinding() {
		return canBeUsedInMazePathfinding && canBeUsedInPathfinding;
	}

	public void SetParent(RoomNode parentNode) {
		this.parentNode = parentNode;
	}

	public RoomNode GetParentNode() {
		return this.parentNode;
	}

	public int GetTotalMoveCost() {
		return totalMoveCost;
	}

	public int GetMoveCost() {
		return moveCost;
	}

	public void SetTotalMoveCost(int newTotalMoveCost) { 
		this.totalMoveCost = totalMoveCost;
	}

	public void SetMoveCost(int newMoveCost) {
		this.moveCost = newMoveCost;
	}

	public void SetPointOfInterest(PointOfInterest pointOfInterest) {
		hasPointOfInterest = true;
		this.pointOfInterest = pointOfInterest;
	}

	public PointOfInterest GetPointOfInterest() {
		return pointOfInterest;
	}

	protected virtual string CorrectRoomName(string roomName) {

		if(roomName == "leftright" || roomName == "rightleft") {
			roomName = "horizontal";
		}

		if(roomName == "updown" || roomName == "downup") {
			roomName = "vertical";
		}
		
		if(roomName == "leftdownup") {
			roomName = "leftupdown";
		}
		
		if(roomName == "rightdownup") {
			roomName = "rightupdown";
		}

		if(roomName == "leftrightdown") {
			roomName = "rightleftdown";
		}

		if(roomName == "leftrightup") {
			roomName = "rightleftup";
		}

		return roomName;
	}

	private string GetEntrancePostFix() {
		string connectedDirString = "";

		if(GetConnectedDirections().Contains(Direction.RIGHT)) {
			connectedDirString += "_right";
		}

		if(GetConnectedDirections().Contains(Direction.DOWN)) {
			connectedDirString += "_down";
		}

		return connectedDirString;
	}

	public void SetTileBlock(TileBlock tileBlock) {
		this.tileBlock = tileBlock;
	}

	public TileBlock GetTileBlock() {
		return tileBlock;
	}

	public bool HasPointOfInterest() {
		return hasPointOfInterest;
	}

	public bool IsPlayerNode() {
		return isPlayerNode;
	}

	public bool IsPlayerSpawnNode() {
		return isPlayerSpawnNode;
	}

	public void SetRoom(Room room) {
		this.room = room;
	}

	public void SetOccupied() {
		this.isOccupied = true;
	}

	public bool IsOccupied() {
		return isOccupied;
	}

	public void SetRoomNodeID(int newRoomNodeID) {
		this.roomNodeID = newRoomNodeID;
	}

	public int GetRoomNodeID() {
		return this.roomNodeID;
	}

	public void SetConnectsToExit() {
		canConnectToEntrance = false;
	}

	public bool CanConnectToEntrance() {
		return canConnectToEntrance;
	}

	public void SetVillageName(string villageName) {
		this.villageName = villageName;
	}
}
