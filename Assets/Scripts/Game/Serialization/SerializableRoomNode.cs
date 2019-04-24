using UnityEngine;
using System.Collections;

public class SerializableRoomNode {
	public int roomNodeID;

	public Vector3 localPosition;

	public string roomPrefix;
	public TileType tileType;
	public Vector2 worldGridLocation;
	public Vector2 gridLocation;
	public bool isPartOfPath;

	public bool isVisited;
	public bool isPlayerNode;
	public bool isPlayerSpawnNode;
	public bool canBeVisited;

	public PointOfInterest pointOfInterest = PointOfInterest.Dungeon_UP;

	public bool hasPointOfInterest = false;
	public bool hasShop = false;

	public RoomNodeType roomNodeType = RoomNodeType.Normal;
}
