using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerializableTileBlock {

	public int version;
	public int id;
	public Vector3 position;

	public int minimumWorldGridSize;
	public int maximumWorldGridSize;

	public TileType tileType;
	public List<List<SerializableRoomNode>> roomNodes;
	public Vector2 localGridLocation;
	public Vector2 worldGridLocation;
	public bool isPartOfPath;

	public BlockType blockType = BlockType.NormalBlock;
	public bool isPlayerCurrentTileBlock;

}
