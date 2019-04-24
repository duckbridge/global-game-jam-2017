using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MazeBuilder : LevelBuilder {

	public int tileBlockSize = 10;

	public TileType tileType;

	private RoomNode[,] totalGrid;

	void Update () {
	
	}

	protected override void PreparePlayer () {

		RoomNode startRoom = totalGrid[0, totalGrid.GetLength(1)-1];

		player.GetComponent<PlayerInputComponent> ().enabled = false;

		currentRoomNode = startRoom;
		player.SetStartRoomNode(startRoom);

		startRoom.GetRoom().OnEntered(0f, ref player);

		player.transform.position = new Vector3(startRoom.GetRoom().transform.Find("SpawnPosition").position.x, player.transform.position.y, startRoom.GetRoom().transform.Find("SpawnPosition").position.z);
		cameraContainer.transform.position = new Vector3(startRoom.GetRoom().GetCameraPosition().x, cameraContainer.transform.position.y, startRoom.GetRoom().GetCameraPosition().z);

		minimapBuilder.PrepareSmallMinimap(startRoom.gridLocation);
		minimapBuilder.SetCurrentGrid(player, startRoom.gridLocation);

		player.SetInTown(false);
		player.SetInside(false);

		player.GetAnimationControl().SwapAnimationGroup(AnimationGroup.Normal);

		player.OnSpawned();

		SceneUtils.FindObject<OnStartEffect>().ShowEffect(this.gameObject);
	}

	private void OnStartEffectDone() {
        SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.Show ());
		player.GetComponent<PlayerInputComponent> ().enabled = true;
    }

	protected override void BuildLevel() {

		UpdateLoadingText("Building dungeon", 30);

		List<RoomNode> allRoomNodes = new List<RoomNode>();
				
		TileBlockBuilder tileBlockBuilder = CreateTileBlockBuilder();
		TileBlock tileBlock = tileBlockBuilder.GetTileBlock();

		tileBlockBuilder.SetDungeonBuilder();

		player.SetCurrentTileBlock(tileBlock);

		tileBlockBuilder.PrepareForBuilding(new Vector2(0, 0));
		tileBlockBuilder.transform.position = new Vector3(0, 0);
		tileBlockBuilder.transform.parent = this.transform;

		minimapGridSize += new Vector2(tileBlock.roomNodes.GetLength(0), tileBlock.roomNodes.GetLength(1));

		tileBlock.worldGridLocation = new Vector2(0, 0);
		tileBlock.localGridLocation = new Vector2(0, 0);

		allRoomNodes = BuildAllRoomNodesInBlock(ref tileBlockBuilder);

		totalGrid = new RoomNode[(int)minimapGridSize.x, (int)minimapGridSize.y];

		foreach(RoomNode roomNode in allRoomNodes) {
			roomNode.isVisited = false;
			totalGrid[(int)roomNode.gridLocation.x, (int)roomNode.gridLocation.y] = roomNode;
		}

		PathFindBetweenExitPointsInTileBlocks(ref tileBlockBuilder, ref totalGrid);

		SpawnAllRoomsForTileBlock(ref tileBlock);

		minimapBuilder.CreateMiniMapForGrid(ref tileBlock, new Vector2(0f, 0f));

		UpdateLoadingText("Done", 100);
	
	}

	private TileBlockBuilder CreateTileBlockBuilder() {

		TileBlockBuilder builder = (TileBlockBuilder) GameObject.Instantiate(tileBlockBuilderPrefab, this.transform.position, Quaternion.identity);

		builder.Initialize(tileBlockSize);
		builder.SpawnTileBlock(tileType, roomSize, 0);

		return builder;
	}

	private List<RoomNode> BuildAllRoomNodesInBlock(ref TileBlockBuilder blockBuilder) {
	
		List<RoomNode> allRoomNodes = new List<RoomNode>();

		blockBuilder.SpawnNodes();
		allRoomNodes.AddRange(blockBuilder.GetRoomNodesAsList());

		return allRoomNodes;
	}


	private void SpawnAllRoomsForTileBlock(ref TileBlock tileBlock) {
		foreach(RoomNode roomNode in tileBlock.GetRoomNodesAsList()) {
			roomNode.PrepareRoomForSpawning(ref player, true);
			totalGrid[(int)roomNode.gridLocation.x, (int)roomNode.gridLocation.y] = roomNode;
		}
		tileBlock.SetSpawnedRooms();
	}

	private void PathFindBetweenExitPointsInTileBlocks(ref TileBlockBuilder tileBlockBuilderUsed, ref RoomNode[,] roomNodeGrid) {
		RoomNode roomNode = roomNodeGrid[0, roomNodeGrid.GetLength(1)-1];
		RoomNode targetNode = roomNodeGrid[roomNodeGrid.GetLength(0)-1, 0];

		roomNode.SetPartOfPath();
		targetNode.SetPartOfPath();

		tileBlockBuilderUsed.CreateMazeFromPointAToB(ref roomNode, ref targetNode, ref roomNodeGrid);
	}
}

