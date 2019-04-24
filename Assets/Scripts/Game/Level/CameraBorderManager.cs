using UnityEngine;
using System.Collections;
using System;

public class CameraBorderManager : MonoBehaviour {

    public float areaNameShowTime = 3f;
	public iTween.EaseType transitionEaseType = iTween.EaseType.easeOutExpo;

	public float cameraTransitionTime = .5f;

	private LevelBuilder levelBuilder;

	private Player player;
	private Camera gameCamera;

	private bool canSwitchRooms = true;
	private bool isTransitioning = false;

	private AreaNameDisplay areaNameOutput;

	private TileType oldTileBlockTileType = TileType.none, newTileBlockTileType = TileType.none;

	// Use this for initialization
	void Start () {

	}

	public void Initialize() {
		player = SceneUtils.FindObject<Player>();
		player.AddEventListener(this.gameObject);
		
		gameCamera = this.transform.Find("GameCamera").GetComponent<Camera>();
		levelBuilder = SceneUtils.FindObject<LevelBuilder>();

		areaNameOutput = SceneUtils.FindObject<AreaNameDisplay>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(player) {

			Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);

			if(player.transform.position.x < (this.transform.position.x - cameraBounds.extents.x)) {
				OnLeftRoom(new Vector2(-1, 0));
			} else if(player.transform.position.x > (this.transform.position.x + cameraBounds.extents.x)) {
				OnLeftRoom(new Vector2(1, 0));
			} else if(player.transform.position.z < (this.transform.position.z -  cameraBounds.extents.z)) {
				OnLeftRoom(new Vector2(0, -1));
			} else if(player.transform.position.z > (this.transform.position.z + cameraBounds.extents.z)) {
				OnLeftRoom(new Vector2(0, 1));
			}
		}
	}

	private void OnLeftRoom(Vector2 newGridLocation) {
		if(player) {

			RoomTransferData roomTransferData = new RoomTransferData();

			if(!levelBuilder) {
				ClampPlayer(player);
				return;
			}

			RoomNode roomToTranfserTo = RoomNodeHelper.GetRoomNodeAt(player.GetCurrentTileBlock(), player.GetGridLocation() + newGridLocation);
			RoomNode oldRoomNode = levelBuilder.GetCurrentRoomNode();

			if(canSwitchRooms) {
				
				object[] roomToTransferToAndIsInNewTileBlock = SetRoomToTransferToAsRoomInNewTileBlockIfEntered(levelBuilder.GetCurrentRoomNode(), roomToTranfserTo, newGridLocation);
				roomToTranfserTo = (RoomNode)roomToTransferToAndIsInNewTileBlock.GetValue (0);
				bool isInNewTileBlock = (bool) roomToTransferToAndIsInNewTileBlock.GetValue(1);
	
				if (isInNewTileBlock) {
					RoomNodeSpawnerUtil.SpawnRoom (roomToTranfserTo); 
				}
			}

			if(roomToTranfserTo && roomToTranfserTo.GetRoom() && canSwitchRooms && player.CanSwitchRooms(oldRoomNode, roomToTranfserTo) && roomToTranfserTo.CanBeVisited() && !roomToTranfserTo.GetRoom().IsDefaultRoom()) {

                if(GetComponent<FollowCamera2D>()) {
                    GetComponent<FollowCamera2D>().enabled = false;
                }

				if(oldRoomNode.GetTileBlock() != roomToTranfserTo.GetTileBlock()) {
					levelBuilder.OnTileBlockEntered(roomToTranfserTo.GetTileBlock(), oldRoomNode.GetTileBlock());

					oldTileBlockTileType = oldRoomNode.GetTileBlock().GetTileType();
					newTileBlockTileType = roomToTranfserTo.GetTileBlock().GetTileType();

					MusicManager musicManager = SceneUtils.FindObject<MusicManager>();

					if(musicManager && musicManager.GetCurrentMusicTileType() != newTileBlockTileType) {
						musicManager.PlayMusicByTileType(newTileBlockTileType);
					}
				}

				canSwitchRooms = false;
				isTransitioning = true;

				iTween.MoveTo(this.gameObject, new ITweenBuilder()
				              .SetPosition(roomToTranfserTo.GetRoom().GetCameraPosition())
				              .SetTime(cameraTransitionTime)
				              .SetEaseType(transitionEaseType).Build()
				              );

				Invoke ("ResetSwitchingOfRooms", cameraTransitionTime);

				roomTransferData.roomNode = roomToTranfserTo;
				player.OnExittedRoom(oldRoomNode);

				oldRoomNode.GetRoom ().OnExitted();

				if(HasEnteredFirstRoomOfNewTileBlock(oldRoomNode, roomToTranfserTo)) {
					oldTileBlockTileType = newTileBlockTileType;

					MusicManager musicManager = SceneUtils.FindObject<MusicManager>();
					if(musicManager && musicManager.GetCurrentMusicTileType() != newTileBlockTileType) {
						musicManager.PlayMusicByTileType(newTileBlockTileType);
					}
				}

				if(HasExitedVillage(oldRoomNode, roomToTranfserTo)) {
					player.OnVillageExited(oldRoomNode, roomToTranfserTo);
					areaNameOutput.ShowNameForTime(roomToTranfserTo.GetRoom().areaCode, areaNameShowTime);
				}

				if(HasEnteredVillageFromOutside(oldRoomNode, roomToTranfserTo)) {
					player.OnVillageEnteredFromOutside(roomToTranfserTo);
					areaNameOutput.ShowNameForTime(roomToTranfserTo.GetRoom().areaCode, areaNameShowTime);

                    GetComponent<FollowCamera2D>().enabled = true;
                    GetComponent<FollowCamera2D>().Initialize(roomToTranfserTo.transform.parent);
				}

				if(HasEnteredVillageExitFromVillage(oldRoomNode, roomToTranfserTo)) {
					player.OnVillageExitEnteredFromVillage(roomToTranfserTo);
				}

				if(HasEnteredTransitionRoomFromRegularRoom(oldRoomNode, roomToTranfserTo)) {
					areaNameOutput.ShowNameForTime(roomToTranfserTo.GetRoom().areaCode, areaNameShowTime);
				}

				if(HasEnteredRegularRoomFromTransitionRoom(oldRoomNode, roomToTranfserTo)) {
					areaNameOutput.ShowNameForTime(roomToTranfserTo.GetRoom().areaCode, areaNameShowTime);
				}

				levelBuilder.OnRoomEntered(roomTransferData, cameraTransitionTime * .5f);

			} else {

				ClampPlayer(player);

			}
		}
	}

	private bool HasEnteredFirstRoomOfNewTileBlock(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return oldTileBlockTileType != newTileBlockTileType && oldRoomNode.tileType == newRoomNode.tileType;
	}

	private void ClampPlayer(Player player) {
		Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);
		
		float clampedPositionX = Mathf.Clamp(player.transform.position.x, this.transform.position.x - cameraBounds.extents.x, this.transform.position.x + cameraBounds.extents.x);
		float clampedPositionZ = Mathf.Clamp(player.transform.position.z, this.transform.position.z - cameraBounds.extents.z, this.transform.position.z + cameraBounds.extents.z);
		
		player.transform.position = new Vector3(clampedPositionX, player.transform.position.y, clampedPositionZ);
	}

	private void ResetSwitchingOfRooms() {
		CancelInvoke("ResetSwitchingOfRooms");
		isTransitioning = false;
		canSwitchRooms = true;
	}

	public bool IsTransitioning() {
		return isTransitioning;
	}

	private bool HasEnteredVillageFromOutside(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return oldRoomNode.GetRoomNodeType() == RoomNodeType.Normal && newRoomNode.GetRoomNodeType() == RoomNodeType.VillageExit;
	}

	private bool HasEnteredVillageExitFromVillage(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return (oldRoomNode.GetRoomNodeType() == RoomNodeType.Village || oldRoomNode.GetRoomNodeType() == RoomNodeType.VillageCenter) && newRoomNode.GetRoomNodeType() == RoomNodeType.VillageExit;
	}

	private bool HasEnteredTransitionRoomFromRegularRoom(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return (oldRoomNode.GetRoomNodeType() == RoomNodeType.Normal && newRoomNode.GetRoomNodeType() == RoomNodeType.Transition);
	}

	private bool HasEnteredRegularRoomFromTransitionRoom(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return (oldRoomNode.GetRoomNodeType() == RoomNodeType.Transition && newRoomNode.GetRoomNodeType() == RoomNodeType.Normal);
	}


	private bool HasExitedVillage(RoomNode oldRoomNode, RoomNode newRoomNode) {
		return (oldRoomNode.GetRoomNodeType() == RoomNodeType.Village || oldRoomNode.GetRoomNodeType() == RoomNodeType.VillageExit) && newRoomNode.GetRoomNodeType() == RoomNodeType.Normal;
	}

	private object[] SetRoomToTransferToAsRoomInNewTileBlockIfEntered(RoomNode currentRoomNode, RoomNode roomToTranfserTo, Vector2 newGridLocation) {

		RoomNode newRoomNode = roomToTranfserTo;
		bool isInNewTileBlock = false;

		if(currentRoomNode.gridLocation.x + newGridLocation.x >= player.GetCurrentTileBlock().roomNodes.GetLength(0)) {

			TileBlock nextTileBlock = ListUtils.NextTo(levelBuilder.GetTileBlocks(), player.GetCurrentTileBlock());
			newRoomNode = levelBuilder.GetRoomNodeAtWorldLocation(nextTileBlock, new Vector2(nextTileBlock.minimumWorldGridSize, 0)); 
			isInNewTileBlock = true;
		}
		
		if(currentRoomNode.gridLocation.y + newGridLocation.y >= player.GetCurrentTileBlock().roomNodes.GetLength(1)) {

			TileBlock nextTileBlock = ListUtils.NextTo(levelBuilder.GetTileBlocks(), player.GetCurrentTileBlock());
			newRoomNode = levelBuilder.GetRoomNodeAtWorldLocation(nextTileBlock, new Vector2(0, nextTileBlock.minimumWorldGridSize)); 
			isInNewTileBlock = true;
		}
		
		
		if(currentRoomNode.gridLocation.x + newGridLocation.x < 0f) {

			TileBlock nextTileBlock = ListUtils.PreviousOf(levelBuilder.GetTileBlocks(), player.GetCurrentTileBlock());
			newRoomNode = levelBuilder.GetRoomNodeAtWorldLocation(nextTileBlock, new Vector2(nextTileBlock.maximumWorldGridSize - 1, 0)); 
			isInNewTileBlock = true;	
		}

		if(currentRoomNode.gridLocation.y + newGridLocation.y < 0f) {

			TileBlock nextTileBlock = ListUtils.PreviousOf(levelBuilder.GetTileBlocks(), player.GetCurrentTileBlock());
			newRoomNode = levelBuilder.GetRoomNodeAtWorldLocation(nextTileBlock, new Vector2(0, nextTileBlock.maximumWorldGridSize - 1)); 
			isInNewTileBlock = true;
		}

		return new object[] { newRoomNode, isInNewTileBlock };
	}

	public void DisableSwitchingOfRooms() {
		canSwitchRooms = false;
	}

	public void EnableSwitchingOfRooms() {
		canSwitchRooms = true;
	}
}

