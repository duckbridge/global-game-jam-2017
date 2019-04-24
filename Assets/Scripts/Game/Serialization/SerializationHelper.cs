using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerializationHelper {

	public static SerializableTileBlock SerializeTileBlock(TileBlock tileBlock) {

		SerializableTileBlock serializeTileBlock = new SerializableTileBlock();

		serializeTileBlock.id = tileBlock.id;
		serializeTileBlock.position = tileBlock.transform.position;

		serializeTileBlock.localGridLocation = tileBlock.localGridLocation;
		serializeTileBlock.worldGridLocation = tileBlock.worldGridLocation;
		serializeTileBlock.isPlayerCurrentTileBlock = tileBlock.isPlayerCurrentTileBlock;

		serializeTileBlock.blockType = tileBlock.GetBlockType();
		serializeTileBlock.isPartOfPath = tileBlock.isPartOfPath;
		serializeTileBlock.roomNodes = SerializeRoomNodes(tileBlock.roomNodes);
		serializeTileBlock.tileType = tileBlock.tileType;
		serializeTileBlock.maximumWorldGridSize = tileBlock.maximumWorldGridSize;
		serializeTileBlock.minimumWorldGridSize = tileBlock.minimumWorldGridSize;

		return serializeTileBlock;

	}

	public static List<List<SerializableRoomNode>> SerializeRoomNodes(RoomNode[, ] roomNodes) {
		List<List<SerializableRoomNode>> serializedRoomNodes = new List<List<SerializableRoomNode>>();

		for(int x = 0 ; x < roomNodes.GetLength(0); x++) {

			List<SerializableRoomNode> subList = new List<SerializableRoomNode>();

			for(int y = 0; y < roomNodes.GetLength(1); y++) {
				subList.Add(SerializeRoomNode (roomNodes[x, y]));

			}

			serializedRoomNodes.Add (subList);
		}

		return serializedRoomNodes;
	}

	public static SerializableRoomNode SerializeRoomNode(RoomNode roomNode) {

		SerializableRoomNode serializableRoomNode = new SerializableRoomNode();

		serializableRoomNode.gridLocation = roomNode.gridLocation;
		serializableRoomNode.worldGridLocation = roomNode.worldGridLocation;
		serializableRoomNode.hasPointOfInterest = roomNode.HasPointOfInterest();
		serializableRoomNode.roomNodeType = roomNode.GetRoomNodeType();
		serializableRoomNode.isPartOfPath = roomNode.isPartOfPath;

		serializableRoomNode.isPlayerNode = roomNode.IsPlayerNode();
		serializableRoomNode.isPlayerSpawnNode = roomNode.IsPlayerSpawnNode();
		serializableRoomNode.canBeVisited = roomNode.CanBeVisited();

		serializableRoomNode.localPosition = roomNode.transform.localPosition;
		serializableRoomNode.pointOfInterest = roomNode.GetPointOfInterest();
		serializableRoomNode.roomPrefix = roomNode.roomPrefix;
		serializableRoomNode.roomNodeID = roomNode.GetRoomNodeID();
		serializableRoomNode.tileType = roomNode.tileType;

		serializableRoomNode.isVisited = roomNode.isVisited;

		return serializableRoomNode;

	}

	public static List<TileBlock> DeSerializeTileBlockContainer(Transform parentTransform, SerializableTileBlockContainer serializableTileBlockContainer) {

		List<TileBlock> tileBlocks = new List<TileBlock>();

		for(int i = 0 ; i < serializableTileBlockContainer.serializableTileBlocks.Count; i++) {
			TileBlock tileBlock = SpawnTileBlock(serializableTileBlockContainer.serializableTileBlocks[i]);
			tileBlock.transform.parent = parentTransform;

			tileBlocks.Add (tileBlock);
		}

		return tileBlocks;
	}

	private static TileBlock SpawnTileBlock(SerializableTileBlock serializableTileBlock) {
		TileBlock spawnedTileBlock = (TileBlock) GameObject.Instantiate(Resources.Load("Rooms/TileBlock", typeof(TileBlock)), new Vector3(0f, 0f, 0f), Quaternion.identity);

		spawnedTileBlock.maximumWorldGridSize = serializableTileBlock.maximumWorldGridSize;
		spawnedTileBlock.minimumWorldGridSize = serializableTileBlock.minimumWorldGridSize;

		//set version here? based on the village name? or sth?
		int versionOfTileBlock = SceneUtils.FindObject<PlayerSaveComponent>().GetVersion(serializableTileBlock.id);
		spawnedTileBlock.version = versionOfTileBlock;

		spawnedTileBlock.id = serializableTileBlock.id;
		spawnedTileBlock.transform.position = serializableTileBlock.position;
		spawnedTileBlock.SetBlockType(serializableTileBlock.blockType);
		spawnedTileBlock.SetTileType(serializableTileBlock.tileType);
		spawnedTileBlock.SetLocalGridLocation(serializableTileBlock.localGridLocation);
		spawnedTileBlock.isPartOfPath = serializableTileBlock.isPartOfPath;
		spawnedTileBlock.worldGridLocation = serializableTileBlock.worldGridLocation;
		spawnedTileBlock.isPlayerCurrentTileBlock = serializableTileBlock.isPlayerCurrentTileBlock;

		spawnedTileBlock.roomNodes = SpawnRoomNodes(spawnedTileBlock, serializableTileBlock.roomNodes);

		return spawnedTileBlock;

	}

	private static RoomNode[,] SpawnRoomNodes(TileBlock tileBlock, List<List<SerializableRoomNode>> serializableRoomNodes) {
		RoomNode[, ] roomNodes = new RoomNode[serializableRoomNodes.Count,serializableRoomNodes[0].Count];

		for(int x = 0 ; x <serializableRoomNodes.Count ; x++) {
			for(int y = 0 ; y < serializableRoomNodes[x].Count ; y++) {
				roomNodes[x,y] = SpawnRoomNode(tileBlock, serializableRoomNodes[x][y]);
			}
		}

		return roomNodes;
	}

	private static RoomNode SpawnRoomNode(TileBlock tileBlock, SerializableRoomNode serializableRoomNode) {
		RoomNode spawnedRoomNode = (RoomNode) GameObject.Instantiate(Resources.Load("Rooms/RoomNode", typeof(RoomNode)), new Vector3(0f, 0f, 0f), Quaternion.identity);

		spawnedRoomNode.transform.parent = tileBlock.transform;
		spawnedRoomNode.transform.localPosition = serializableRoomNode.localPosition;
		spawnedRoomNode.roomPrefix = serializableRoomNode.roomPrefix;
		spawnedRoomNode.tileType = serializableRoomNode.tileType;
		spawnedRoomNode.gridLocation = serializableRoomNode.gridLocation;
		spawnedRoomNode.worldGridLocation = serializableRoomNode.worldGridLocation;
		spawnedRoomNode.isPartOfPath = serializableRoomNode.isPartOfPath;
		spawnedRoomNode.SetRoomNodeID(serializableRoomNode.roomNodeID);

		spawnedRoomNode.version = tileBlock.version;
		spawnedRoomNode.isVisited = serializableRoomNode.isVisited;

		spawnedRoomNode.SetTileBlock(tileBlock);

		if(serializableRoomNode.isPlayerNode) {
			spawnedRoomNode.SetPlayerNode();
		}

		if(serializableRoomNode.isPlayerSpawnNode) {
			spawnedRoomNode.SetPlayerSpawnNode();
		}

		if(serializableRoomNode.canBeVisited) {
			spawnedRoomNode.EnableVisiting();
		} else {
			spawnedRoomNode.DisableVisiting();
		}

		if(serializableRoomNode.hasPointOfInterest) {
			spawnedRoomNode.SetPointOfInterest(serializableRoomNode.pointOfInterest);
		}

		spawnedRoomNode.SetRoomNodeType(serializableRoomNode.roomNodeType);

		return spawnedRoomNode;
	}

	public static void PrepareRoomForSpawning(RoomNode roomNode, ref Player player) {

		if(roomNode.roomPrefix != null) {
			Logger.Log (roomNode.roomPrefix);

			string[] splittedRoomPrefix = roomNode.roomPrefix.Split ('/');
			string roomNodeName = splittedRoomPrefix [splittedRoomPrefix.Length - 1];

			roomNode.name = roomNodeName;

			if(roomNode.version > 0) {
				InsertRoomNodeVersion(roomNode);
			}

			if (roomNodeName.Equals(player.GetNameOfPlayerSpawnRoomNode())) {
				RoomNodeSpawnerUtil.SpawnRoom (roomNode);
				player.SetStartRoomNode (roomNode);
			}
		} else {
			Logger.Log ("RoomNode didnt have roomPrefix!");
		}
	}

	private static void InsertRoomNodeVersion(RoomNode roomNode) {
		if(roomNode.roomPrefix.Contains("version")) {

			int startIndex = roomNode.roomPrefix.IndexOf("version");
			char oldVersion = roomNode.roomPrefix[startIndex + 7];
			string newRoomNodePrefix = roomNode.roomPrefix.Replace("version"+oldVersion, "version"+roomNode.version);

			roomNode.roomPrefix = newRoomNodePrefix;
		}
	}
}
