using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TileBlockBuilder : MonoBehaviour {

	public TileBlock tileBlockPrefab;

	public Vector2 villageLocation = new Vector2(3, 3);

	private int mazePathMaxWidth = 3;
	private int blockSize = 5;
	private int villageBounds = 1;

	private Vector2 roomSize;
	private Vector2 gridLocationToStartBuilding;

	private Player player;
	private TileBlock tileBlock;
	
	private List<RoomNode> endPoints;
	private string villageName = "village";

	private bool isDungeonBuilder = false;

	void Awake () {
	}
	
	void Update () {
	}

	public void Initialize(int newBlockSize) {
		this.blockSize = newBlockSize;
	}

	public void SpawnTileBlock(TileType tileType, Vector2 roomSize, int id) {
		this.roomSize = roomSize;

		this.tileBlock = (TileBlock) GameObject.Instantiate(tileBlockPrefab, this.transform.position, Quaternion.identity);
		this.tileBlock.transform.parent = this.transform;
		tileBlock.id = id;

		tileBlock.Initialize(blockSize, roomSize);
		tileBlock.SetTileType(tileType);
	}

	public void PrepareForBuilding(Vector2 gridLocationToStartBuilding) {
		this.gridLocationToStartBuilding = gridLocationToStartBuilding;

		player = SceneUtils.FindObject<Player>();

	}

	public void SpawnEmptyRoomNodes() {
		
		int minimumBlockSize = (int) (-Math.Floor((float)blockSize/2));
		int maximumBlockSize = (int) Math.Ceiling((float)blockSize/2);
		
		int xIndex = 0;
		int yIndex = 0;
		
		for(int x = minimumBlockSize; x < maximumBlockSize; x++) {
			for(int y = minimumBlockSize ; y < maximumBlockSize; y++) {
				
				RoomNode roomNode = SpawnEmptyRoomNode(new Vector2(x, y), new Vector2(xIndex, yIndex));
				
				roomNode.DisableVisiting();
				roomNode.SetUnusableForPathfinding();

				roomNode.gridLocation = (new Vector2(gridLocationToStartBuilding.x + xIndex, gridLocationToStartBuilding.y + yIndex));
				tileBlock.roomNodes[xIndex, yIndex] = roomNode;
				
				yIndex++;
				
			}
			
			xIndex++;
			yIndex = 0;
			
		}
	}

	public void SpawnNodes() {
		
		int minimumBlockSize = (int) (-Math.Floor((float)blockSize/2));
		int maximumBlockSize = (int) Math.Ceiling((float)blockSize/2);
		
		int xIndex = 0;
		int yIndex = 0;

		tileBlock.minimumWorldGridSize = minimumBlockSize;
		tileBlock.maximumWorldGridSize = maximumBlockSize;

		for(int x = minimumBlockSize; x < maximumBlockSize; x++) {
			for(int y = minimumBlockSize ; y < maximumBlockSize; y++) {
				
				RoomNode roomNode = SpawnRoomNode(new Vector2(x, y), new Vector2(xIndex, yIndex));

				if(tileBlock.IsPlayerBlock()) {
					roomNode.SetPlayerNode();
				}

				if(tileBlock.GetConnectedDirections() != null) {

					TileType tileTypeToConnectTo = tileBlock.tileType;

					if(x == minimumBlockSize && y == 0 && tileBlock.GetConnectedDirections().ContainsKey(Direction.LEFT)) {

						tileBlock.GetConnectedDirections().TryGetValue(Direction.LEFT, out tileTypeToConnectTo);
						AddRoomNodeAsEndPoint(roomNode, false, Direction.LEFT, tileTypeToConnectTo);

					} else if(x == (maximumBlockSize - 1) && y == 0 && tileBlock.GetConnectedDirections().ContainsKey(Direction.RIGHT)) {

						roomNode.SetConnectsToExit();
						tileBlock.GetConnectedDirections().TryGetValue(Direction.RIGHT, out tileTypeToConnectTo);
						AddRoomNodeAsEndPoint(roomNode, false, Direction.RIGHT, tileTypeToConnectTo);
					
					} else if(x == 0 && y == minimumBlockSize && tileBlock.GetConnectedDirections().ContainsKey(Direction.DOWN)) {

						tileBlock.GetConnectedDirections().TryGetValue(Direction.DOWN, out tileTypeToConnectTo);
						AddRoomNodeAsEndPoint(roomNode, false, Direction.DOWN, tileTypeToConnectTo);
					
					} else if(x == 0 && y == (maximumBlockSize - 1) && tileBlock.GetConnectedDirections().ContainsKey(Direction.UP)) {

						roomNode.SetConnectsToExit();
						tileBlock.GetConnectedDirections().TryGetValue(Direction.UP, out tileTypeToConnectTo);
						AddRoomNodeAsEndPoint(roomNode, false, Direction.UP, tileTypeToConnectTo);
					
					}
				}

				if(!isDungeonBuilder) {
					if(IsVillageExitDown(new Vector2(xIndex, yIndex))) {

						Logger.Log ("added village exit down");
						AddRoomNodeAsEndPoint(roomNode, true, Direction.UP, tileBlock.tileType);
					}

					if(IsVillageExitUp(new Vector2(xIndex, yIndex)) && !tileBlock.IsPlayerBlock()) {

						Logger.Log ("added village exit up");
						AddRoomNodeAsEndPoint(roomNode, true, Direction.DOWN, tileBlock.tileType);
					}
				}

				roomNode.gridLocation = (new Vector2(gridLocationToStartBuilding.x + xIndex, gridLocationToStartBuilding.y + yIndex));
				roomNode.worldGridLocation = new Vector2(x, y);

				if(IsPlayerSpawnRoom(new Vector2(xIndex, yIndex))) {

					roomNode.SetPlayerSpawnNode();
					roomNode.SetOccupied();

					player.SetInTown(true);
					player.SetStartRoomNode(roomNode);
	
				} else if(roomNode.GetRoomNodeType() == RoomNodeType.Village || 
				          roomNode.GetRoomNodeType() == RoomNodeType.VillageCenter ||
				          roomNode.GetRoomNodeType() == RoomNodeType.VillageExit) {

						roomNode.SetOccupied();
				
				} 

				tileBlock.roomNodes[xIndex, yIndex] = roomNode;
				
				yIndex++;
				
			}
			
			xIndex++;
			yIndex = 0;
			
		}
	}

	public void SpawnPOI(POISpawnTypes poiSpawnType, ref List<Vector2> possibleSpawnPositions, bool connectsWithExit) {

		int randomIndex = UnityEngine.Random.Range (0, possibleSpawnPositions.Count);
		Vector2 randomRoomNodePosition = possibleSpawnPositions[randomIndex];

		RoomNode randomRoomNode = tileBlock.roomNodes[(int)randomRoomNodePosition.x, (int)randomRoomNodePosition.y];

		possibleSpawnPositions.RemoveAt (randomIndex);

		Direction direction = Direction.NONE;
		PointOfInterest chosenPOI = PointOfInterest.School_LEFT;

		switch(poiSpawnType) {
		
			case POISpawnTypes.RandomPOI:

				PointOfInterest[] pois = (PointOfInterest[]) System.Enum.GetValues(typeof(PointOfInterest));
				int randomPOIindex = UnityEngine.Random.Range (0, pois.Length);

				chosenPOI = pois[randomPOIindex];
				direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);

				randomRoomNode.SetPointOfInterest(chosenPOI);
				randomRoomNode.SetUnusableForPathfinding();
			break;
				
			case POISpawnTypes.DungeonPOI:
				
				chosenPOI = PointOfInterest.Dungeon_UP;
				direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);
				
				randomRoomNode.SetPointOfInterest(chosenPOI);
				randomRoomNode.SetUnusableForPathfinding();
				
			break;

            case POISpawnTypes.FishingPOI:
                
                chosenPOI = PointOfInterest.Fishing_UP;
                direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);
                
                randomRoomNode.SetPointOfInterest(chosenPOI);
                randomRoomNode.SetUnusableForPathfinding();
                
            break;
				
			case POISpawnTypes.GameRoomPOI:

				chosenPOI = PointOfInterest.Gameroom_RIGHT;
				direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);
				
				randomRoomNode.SetPointOfInterest(chosenPOI);
				randomRoomNode.SetUnusableForPathfinding();
			break;

			case POISpawnTypes.CassettePOI:
			
				chosenPOI = PointOfInterest.CassetteRoom_RIGHT;
				direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);
				
				randomRoomNode.SetPointOfInterest(chosenPOI);
				randomRoomNode.SetUnusableForPathfinding();
			break;
				
				
			case POISpawnTypes.AnimalRoomPOI:
				
				chosenPOI = PointOfInterest.AnimalRoom_UP;
				direction = (Direction) System.Enum.Parse(typeof(Direction), chosenPOI.ToString().Split('_')[1]);

				randomRoomNode.SetPointOfInterest(chosenPOI);
				randomRoomNode.SetUnusableForPathfinding();
				
			break;
		}

		Direction inverseDirection = MathUtils.GetInverse(direction);
		
		Vector2 newRoomNodeIndex = randomRoomNodePosition + MathUtils.GetDirectionAsVector2(inverseDirection);
		possibleSpawnPositions.Remove (newRoomNodeIndex);

		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(1f, 0f));
		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(-1f, 0f));

		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(0f, -1f));
		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(0f, 1f));

		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(1f, 1f));
		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(1, -1f));

		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(-1f, -1f));
		possibleSpawnPositions.Remove(randomRoomNodePosition + new Vector2(-1f, 1f));

		RoomNode endpointRoomNode = tileBlock.roomNodes[(int)newRoomNodeIndex.x, (int)newRoomNodeIndex.y];
		endpointRoomNode.SetOccupied();

		if(connectsWithExit) {
			endpointRoomNode.SetConnectsToExit();
		}

		Logger.Log ("set neighbour as occupied");

		AddRoomNodeAsEndPoint(endpointRoomNode, false, direction, tileBlock.tileType);

	}

	public void SetAllEndPointNeighboursAsOccupied() {
		foreach(RoomNode endPoint in endPoints) {

			Vector2 neighbourPosition = Vector2.zero;

			switch(endPoint.GetEndpointDirection()) {
				case Direction.UP: //down
					neighbourPosition = endPoint.gridLocation + new Vector2(0, -1);
				break;

				case Direction.DOWN: //up
					neighbourPosition = endPoint.gridLocation + new Vector2(0, 1);
				break;

				case Direction.LEFT: //left
					neighbourPosition = endPoint.gridLocation + new Vector2(-1, 0);
				break;

				case Direction.RIGHT: //right
					neighbourPosition = endPoint.gridLocation + new Vector2(1, 0);
				break;

			}

			if(neighbourPosition.x > 0 && neighbourPosition.y > 0 
			   && neighbourPosition.x < GetRoomNodes().GetLength(0) && neighbourPosition.y < GetRoomNodes().GetLength(1)) {
				GetRoomNodes()[(int)neighbourPosition.x, (int)neighbourPosition.y].SetOccupied();
				Logger.Log ("Set old node as occupied");
			}
		}
	}

	private void AddRoomNodeAsEndPoint(RoomNode roomNode, bool insert, Direction endPointDirection, TileType tileTypeToConnectTo) {
		if(endPoints == null) {
			endPoints = new List<RoomNode>();
		}

		if(insert) {
			endPoints.Insert(0, roomNode);
		} else {
			endPoints.Add(roomNode);
		}

		roomNode.SetOccupied();
		roomNode.SetEndPointDirectionAndTileType(endPointDirection, tileTypeToConnectTo);
	}

	private RoomNode SpawnEmptyRoomNode(Vector2 gridLocation, Vector2 gridLocationInBlock) {
		
		string roomNodePrefix = "Rooms/RoomNode";
		
		Vector3 newPosition = new Vector3(roomSize.x * gridLocation.x, 0f, roomSize.y * gridLocation.y);
		
		RoomNode newRoomNode = (RoomNode) GameObject.Instantiate(Resources.Load(roomNodePrefix, typeof(RoomNode)), new Vector3(-9999, 0f ,9999), Quaternion.identity);
		
		newRoomNode.transform.parent = tileBlock.transform;
		newRoomNode.transform.localPosition = new Vector3(newPosition.x, newRoomNode.transform.localPosition.y, newPosition.z);
		
		return newRoomNode;
	}

	private RoomNode SpawnRoomNode(Vector2 gridLocation, Vector2 gridLocationInBlock) {

		string roomNodePrefix = "Rooms/RoomNode";

		if(isDungeonBuilder) {
			roomNodePrefix = "Rooms/DungeonRoomNode";
		}
		
		Vector3 newPosition = new Vector3(roomSize.x * gridLocation.x, 0f, roomSize.y * gridLocation.y);
		
		RoomNode newRoomNode = (RoomNode) GameObject.Instantiate(Resources.Load(roomNodePrefix, typeof(RoomNode)), new Vector3(-9999, 0f ,9999), Quaternion.identity);

		if(!isDungeonBuilder) {
			if(IsPlayerSpawnRoom(gridLocationInBlock)) {

				newRoomNode.SetVillageName(villageName);
				newRoomNode.SetRoomNodeType(RoomNodeType.VillageCenter);

				if(tileBlock.IsPlayerBlock()) {
					newRoomNode.SetPlayerSpawnNode();
				}

				newRoomNode.SetUnusableForPathfinding();
				
			} else if(IsVillageCenterSpawnRoom(gridLocationInBlock)) {

				newRoomNode.SetVillageName(villageName);
				newRoomNode.SetRoomNodeType(RoomNodeType.VillageCenter);
				newRoomNode.SetVillageSidePostFix(GetVillageSidePostFix(gridLocationInBlock));
				newRoomNode.SetUnusableForPathfinding();

			} else if(IsVillageSpawnRoom(gridLocationInBlock)) {

				newRoomNode.SetVillageName(villageName);

				if(IsVillageExit(gridLocationInBlock)) {
					Logger.Log ("village exit");
					newRoomNode.SetRoomNodeType(RoomNodeType.VillageExit);
				} else {
					newRoomNode.SetRoomNodeType(RoomNodeType.Village);
				}

				newRoomNode.SetVillageSidePostFix(GetVillageSidePostFix(gridLocationInBlock));
				newRoomNode.SetUnusableForPathfinding();
				
			} else if(IsAroundVillage(gridLocationInBlock)) {
				newRoomNode.SetOccupied();
			} 
		}

		newRoomNode.SetTileBlock(tileBlock);
		newRoomNode.SetTileType(tileBlock.GetTileType());
		newRoomNode.transform.parent = tileBlock.transform;
		newRoomNode.transform.localPosition = new Vector3(newPosition.x, newRoomNode.transform.localPosition.y, newPosition.z);
		newRoomNode.EnableVisiting();

		return newRoomNode;
	}

	public void CreateMazeFromPointAToB(ref RoomNode startNode, ref RoomNode endNode, ref RoomNode[,] roomNodeGrid) {
		startNode.SetDungeonEntranceNode();
		endNode.SetDungeonBossNode();

		MazeBuilderHelper.CreateMaze(startNode, endNode, ref roomNodeGrid, new Vector2(1, 0), new Vector2(-1, 0));
	}

	public void PathFindBetweenExitPoints() {

		bool stuckWhileFinding = false;

		if(tileBlock.IsPlayerBlock()) { //first block where player spawns
		
			if(endPoints != null && endPoints.Count > 0) {
				endPoints[0].SetPartOfPath();
				for(int i = 1 ; i < endPoints.Count ;i++) {
					List<RoomNode> foundPath = TileBlockPathFinder.FindPathBetween(endPoints[0], endPoints[i], ref tileBlock);

					if(foundPath == null) {
						stuckWhileFinding = true;
						break;
					}
				}
			}
		} else {

			if(endPoints != null && endPoints.Count > 0) {
				endPoints[0].SetPartOfPath(); //exit
				endPoints[1].SetPartOfPath(); //entrance

				for(int i = 2 ; i < endPoints.Count ;i++) {

					List<RoomNode> foundPath;

					if(endPoints[i].CanConnectToEntrance()) {
						//hier de Maze pathfinding gebruiken.. denk ik!
						RoomNode[,] roomNodesToUse = tileBlock.GetRoomNodesUnderYAsMazeUnPathfindable(
							(int)villageLocation.x - mazePathMaxWidth, 
							(int)villageLocation.x + mazePathMaxWidth,
							(int)villageLocation.y - 1);
						foundPath = MazeBuilderHelper.CreateMaze(endPoints[1], endPoints[i], ref roomNodesToUse, new Vector2(0, -1), new Vector2(0, 1));
						//foundPath = TileBlockPathFinder.FindPathBetween(endPoints[1], endPoints[i], ref tileBlock);
					} else {
						foundPath = TileBlockPathFinder.FindPathBetween(endPoints[0], endPoints[i], ref tileBlock);
					}
					
					if(foundPath == null) {
						stuckWhileFinding = true;
						break;
					}
				}
			}
		}

		if(stuckWhileFinding) {
			Logger.Log("stuck?...reloading scene");
			SceneUtils.FindObject<MapBuilder>().OnMapLoadingError();
		}
	}

	public TileBlock GetTileBlock() {
		return tileBlock;
	}

	public Vector2 GetBlockSize() {
		return new Vector2(tileBlock.roomNodes.GetLength(0) * roomSize.x, 
		                   tileBlock.roomNodes.GetLength(1) * roomSize.y);
	}

	public List<RoomNode> GetRoomNodesAsList() {
		return tileBlock.GetRoomNodesAsList();
	}

	public RoomNode[,] GetRoomNodes() {
		return tileBlock.roomNodes;
	}

	private string GetVillageSidePostFix(Vector2 gridLocationInBlock) {
		string postFix = "";

		int xMin = (int) (villageLocation.x - villageBounds);
		int xMax = (int) (villageLocation.x + villageBounds);

		int yMin = (int) (villageLocation.y - villageBounds);
		int yMax = (int) (villageLocation.y + villageBounds);

		if(gridLocationInBlock.y == yMin) {
			postFix += "_bottom";
		} else if(gridLocationInBlock.y == yMax) {
			postFix = "_top";
		}

		if(gridLocationInBlock.x == xMin) {
			postFix += "_left";
		} else if(gridLocationInBlock.x == xMax) {
			postFix += "_right";
		}

		return postFix;
	}

	public bool IsAroundVillage(Vector2 gridLocationInBlock) {
		bool isAroundVillage = false;
		
		if(gridLocationInBlock.x >= villageLocation.x - (villageBounds + 1)
		   && gridLocationInBlock.x <= villageLocation.x + (villageBounds + 1)
		   && gridLocationInBlock.y >= villageLocation.y - (villageBounds + 1)
		   && gridLocationInBlock.y <= villageLocation.y + (villageBounds + 1)) {
			
			isAroundVillage = true;
		}

		return isAroundVillage;
	}

	private bool IsVillageSpawnRoom(Vector2 gridLocationInBlock) {

		bool isVillageSpawnRoom = false;

		if(gridLocationInBlock.x >= villageLocation.x - villageBounds
		   && gridLocationInBlock.x <= villageLocation.x + villageBounds
		   && gridLocationInBlock.y >= villageLocation.y - villageBounds
		   && gridLocationInBlock.y <= villageLocation.y + villageBounds
		   && !IsPlayerSpawnRoom(gridLocationInBlock)) {

			isVillageSpawnRoom = true;
		}

		return isVillageSpawnRoom;
	}

	private bool IsVillageExit(Vector2 gridLocationInBlock) {
		bool isVillageExit = false;
		
		if(gridLocationInBlock.x == villageLocation.x
		   && ((gridLocationInBlock.y == villageLocation.y + villageBounds) || (gridLocationInBlock.y == villageLocation.y - villageBounds))) {
			
			isVillageExit = true;
		}
		
		return isVillageExit;
	}

	private bool IsVillageCenterSpawnRoom(Vector2 gridLocationInBlock) {
		return !tileBlock.IsPlayerBlock() && gridLocationInBlock == villageLocation;
	}

	private bool IsCheckPointSpawnRoom(Vector2 gridLocationInBlock) {
		return gridLocationInBlock == villageLocation;
	}

	private bool IsVillageExitDown(Vector2 gridLocationInBlock) {
		bool isVillageExit = false;
		
		if(gridLocationInBlock.x == villageLocation.x
		   && gridLocationInBlock.y == villageLocation.y - villageBounds - 1) {

			isVillageExit = true;
		}
		
		return isVillageExit;
	}

	private bool IsVillageExitUp(Vector2 gridLocationInBlock) {
		bool isVillageExit = false;
		
		if(gridLocationInBlock.x == villageLocation.x
		   && gridLocationInBlock.y == villageLocation.y + villageBounds + 1) {
			
			isVillageExit = true;
		}
		
		return isVillageExit;
	}

	public bool IsAroundCheckpoint(Vector2 gridLocationInBlock) {
		bool isAroundVillage = false;
		
		if(gridLocationInBlock.x >= villageLocation.x - 1
		   && gridLocationInBlock.x <= villageLocation.x + 1
		   && gridLocationInBlock.y >= villageLocation.y - 1
		   && gridLocationInBlock.y <= villageLocation.y + 1) {
			
			isAroundVillage = true;
		}
		
		return isAroundVillage;
	}

	private bool IsCheckpointExit(Vector2 gridLocationInBlock) {
		bool isVillageExit = false;
		
		if(gridLocationInBlock.x == villageLocation.x
		   && gridLocationInBlock.y == villageLocation.y + 1) {
			
			isVillageExit = true;
		}
		
		return isVillageExit;
	}

	private bool IsPlayerSpawnRoom(Vector2 gridLocationInBlock) {
		return tileBlock.IsPlayerBlock() && gridLocationInBlock == villageLocation;
	}

	public void SetVillageName(string villageName) {
		this.villageName = villageName;
	}

	public void SetDungeonBuilder() {
		isDungeonBuilder = true;
	}
}
