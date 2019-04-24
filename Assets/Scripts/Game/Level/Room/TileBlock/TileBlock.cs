using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileBlock : MonoBehaviour {

	public int version {get;set;}
	public int id {get; set;}
	public bool isPlayerCurrentTileBlock {get; set;}
	public int minimumWorldGridSize {get; set;}
	public int maximumWorldGridSize {get; set;}

	public TileType tileType { get; set;}
	public RoomNode[,] roomNodes { get; set;}
	public Vector2 localGridLocation { get; set;}
	public Vector2 worldGridLocation { get; set;}
	public bool isPartOfPath {get; set;}

	private BlockType blockType = BlockType.NormalBlock;

	private List<TileBlock> path;
	private Dictionary<Direction, TileType> connectedDirections;
	private bool hasSpawnedRooms = false;

	private Vector2 size;

	void Start () {}
	
	void Update () {}

	public void Initialize(int blockSize, Vector2 roomSize) {
		roomNodes = new RoomNode[blockSize, blockSize];
		size = new Vector2(((float)blockSize) * roomSize.x, ((float)blockSize) * roomSize.y);
	}

	public void SetTileType(TileType tileType) {
		this.tileType = tileType;
	}

	public TileType GetTileType() {
		return this.tileType;
	}

	public List<RoomNode> GetRoomNodesAsList() {
		List<RoomNode> nodesToReturn = new List<RoomNode>();
		
		foreach(RoomNode roomNode in roomNodes) {
			nodesToReturn.Add (roomNode);
		}
		
		return nodesToReturn;
	}

	public void AddPath(List<TileBlock> path) {
		if(this.path == null) {
			this.path = new List<TileBlock>();
		}

		this.path.AddRange(path);
	}

	public void AddToPath(TileBlock tileBlock) {
		if(this.path == null) {
			this.path = new List<TileBlock>();
		}
		
		this.path.Add(tileBlock);
	}


	public void AddConnectedDirection(Direction directionToAdd, TileType tileType) {
		if(connectedDirections == null) {
			connectedDirections = new Dictionary<Direction, TileType>();
		}
		
		if(!connectedDirections.ContainsKey(directionToAdd)) {
			connectedDirections.Add (directionToAdd, tileType);
		}
	}

	
	public Dictionary<Direction, TileType> GetConnectedDirections() {
		return connectedDirections;
	}

	public void SetPlayerBlock() {
		blockType = BlockType.PlayerBlock;
	}

	public void SetEnemyBlock() {
		blockType = BlockType.EnemyBlock;
	}

	public bool IsPlayerBlock() {
		return blockType == BlockType.PlayerBlock;
	}

	public bool IsEnemyBlock() {
		return blockType == BlockType.EnemyBlock;
	}

	public void SetLocalGridLocation(Vector2 gridLocation) {
		this.localGridLocation = gridLocation;
	}
	
	public Vector2 GetLocalGridLocation() {
		return localGridLocation;
	}

	public bool IsPartOfPath() {
		return isPartOfPath;
	}

	public void SetPartOfPath() {
		isPartOfPath = true;
	}

	public BlockType GetBlockType() {
		return blockType;
	}

	public void SetBlockType(BlockType blockType) {
		this.blockType = blockType;
	}

	public void SetSpawnedRooms() {
		hasSpawnedRooms = true;
	}

	public bool HasSpawnedRooms() {
		return hasSpawnedRooms;
	}

	public float GetWidth() {
		return size.x;
	}

	public float GetHeight() {
		return size.y;
	}

	public RoomNode[,] GetRoomNodesUnderYAsMazeUnPathfindable(int minimumX, int maximumX, int maximumY) {
		for (int x = 0; x < roomNodes.GetLength (0); x++) {
			for(int y = 0 ; y < roomNodes.GetLength(1); y++) {
				Vector2 gridLocation = roomNodes [x, y].gridLocation;
				if(gridLocation.y > maximumY || gridLocation.x < minimumX || gridLocation.x > maximumX) {
					roomNodes [x, y].SetUnusableForMazePathfinding ();
				}
			}
		}

		return roomNodes;
	}
}
