using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapBuilder : LevelBuilder {

	public List<TileBlockInfo> tileBlockInfo;

	public int tileBlockSpawnRange = 1;

	private WeatherManager weatherManager;
	
	private int currentTileBlockIndex = 0;
	
	private TileBlock playerTileBlock;

	private SpawnType spawnType = SpawnType.NORMAL;
	private float playerPositionYOnSpawn = -0.5f;

    public override void OnStart() {
        Physics.IgnoreLayerCollision(GameSettings.ANIMALS_LAYER, GameSettings.ANIMALS_LAYER);
        player.FindMusicComponents();

        if(!isNewGame) {
            player.OnStart(spawnType);
        }
    }

	protected override void PreparePlayer() {

		RoomNode startRoom = player.GetStartRoomNode();

		if (spawnType == SpawnType.ATGAMECONSOLE) {
			startRoom = player.GetCurrentTileBlock ().transform.Find ("villagepart_bottom_left").GetComponent<RoomNode> ();
		}

		currentRoomNode = startRoom;

		if(spawnType == SpawnType.TELEPORTED) {
			player.transform.position = new Vector3(startRoom.GetRoom().transform.Find("SpawnPosition").position.x, playerPositionYOnSpawn, startRoom.GetRoom().transform.Find("SpawnPosition").position.z);
            cameraContainer.transform.position = new Vector3(startRoom.GetRoom().GetCameraPosition().x, cameraContainer.transform.position.y, startRoom.GetRoom().GetCameraPosition().z);
        }

		if (spawnType == SpawnType.ATGAMECONSOLE) {
			player.transform.position = new Vector3 (startRoom.GetRoom ().transform.Find ("SpawnPosition").position.x, playerPositionYOnSpawn, startRoom.GetRoom ().transform.Find ("SpawnPosition").position.z);
			cameraContainer.transform.position = new Vector3 (startRoom.GetRoom ().GetCameraPosition ().x, cameraContainer.transform.position.y, startRoom.GetRoom ().GetCameraPosition ().z);
		}

		cameraContainer.GetComponent<FollowCamera2D>().enabled = true;
        cameraContainer.GetComponent<FollowCamera2D>().Initialize(player.GetCurrentTileBlock().transform);

		minimapBuilder.PrepareSmallMinimap(startRoom.gridLocation);

		minimapBuilder.SetCurrentGrid(player, startRoom.gridLocation);

		startRoom.GetRoom().OnEntered(0f, ref player);

		player.OnSpawned();
		player.GetComponent<PlayerInputComponent>().enabled = false;

		if(isNewGame) {

			if(!GameSettings.SKIP_START_CUTSCENE) {
				GameSettings.SKIP_START_CUTSCENE = true;

				PlayerRoom playerRoom = (PlayerRoom) startRoom.GetRoom();
				for(int i = 0 ; i < playerRoom.cutscenesInRoom.Length ; i++) {
					playerRoom.cutscenesInRoom[i].SetActive(true);
				}

				Invoke ("StartFirstCutscene", .5f);
			} else {

				PlayerRoom playerRoom = (PlayerRoom) startRoom.GetRoom();

				PrepareDBInRoom(playerRoom);

				player.GetAnimationControl().SwapAnimationGroup(AnimationGroup.NakedDrum);
				SceneUtils.FindObject<OnStartEffect>().ShowEffect(this.gameObject);
			}
		} else {
			SceneUtils.FindObject<OnStartEffect>().ShowEffect(this.gameObject);
		}
	}

	private void OnStartEffectDone() {
		SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.Show ());
		if (spawnType == SpawnType.TELEPORTED) {

			BoomboxCompanion boomboxCompanion = player.GetBoomboxCompanion ();

			boomboxCompanion.GetComponent<PlayerSpitter> ().SpitOutPlayer (player);
			player.transform.position = new Vector3 (boomboxCompanion.transform.position.x, playerPositionYOnSpawn, boomboxCompanion.transform.position.z);
			player.GetComponent<PlayerInputComponent> ().enabled = false;

		} else {
			player.GetComponent<PlayerInputComponent> ().enabled = true;
		}
	}

	private void StartFirstCutscene() {
		player.GetCurrentRoomNode().GetRoom().transform.Find("FirstCutsceneManager").GetComponent<CutSceneManager>().StartCutScene(true); //temp ofc!
	}

	public override void PrepareLevel() {

		spawnType = SpawnType.NORMAL;

        SceneUtils.FindObject<SettingsSaveComponent>().LoadSettingsData();
        
		if(!SaveUtils.HasMapSaveFile(GameSettings.CHOSEN_SAVE_SLOT)) {
			
			Logger.Log ("map save file not found, building new map");
			
			UpdateLoadingText("Building world..", 30);

			isNewGame = true;

			BuildLevel();
		
		} else {

			UpdateLoadingText("Loading world..", 30);
			Logger.Log ("map save file found, loading map");

			SceneUtils.FindObject<PlayerSaveComponent>().LoadTileBlockVersions();

			isNewGame = false;

            SerializablePlayerDataSummary savedPlayerData = SceneUtils.FindObject<PlayerSaveComponent>().LoadPlayerData();
			player.SetNameOfPlayerSpawnRoomNode (savedPlayerData.playerSpawnRoomNode);

            DeSerializeData(savedPlayerData.spawnInJunkyard);

			VillageCenterRoom villageCenter = playerTileBlock.GetComponentInChildren<VillageCenterRoom>();
			BoomboxCompanion boomboxCompanion = PrepareDBInRoom(villageCenter);

			spawnType = savedPlayerData.spawnType;

			if(savedPlayerData.spawnType == SpawnType.TELEPORTED) {
                
				player.transform.position = new Vector3 (boomboxCompanion.transform.position.x, playerPositionYOnSpawn, boomboxCompanion.transform.position.z);
				cameraContainer.transform.position = new Vector3(player.transform.position.x, cameraContainer.transform.position.y, player.transform.position.z);

				player.Initialize ();
				player.PrepareForSpitOut ();
			}

			if (savedPlayerData.spawnType == SpawnType.ATGAMECONSOLE) {
				cameraContainer.transform.position = new Vector3(player.transform.position.x, cameraContainer.transform.position.y, player.transform.position.z);

				player.Initialize ();
			}

            player.SetStartRoomNode(villageCenter.GetRoomNode());
			player.SetInTown(true);
		}

        ChangeWeatherBasedOnTileType(player.GetStartRoomNode().GetTileType());

		PreparePlayer();

        SoundUtils.SetSoundVolumeToSavedValue();

		gameCamera.enabled = true;
	}

	private BoomboxCompanion PrepareDBInRoom(Room room) {
		BoomboxCompanion boomboxCompanion = (BoomboxCompanion) GameObject.Instantiate(Resources.Load("Boombox/db", typeof(BoomboxCompanion)), room.transform.Find("dbSpawnPosition").position, Quaternion.Euler(new Vector3(90f, -90f, 0f)));
		boomboxCompanion.currentRoom = room;

		boomboxCompanion.transform.parent = room.transform;
		boomboxCompanion.Start ();
		boomboxCompanion.GetAnimationManager().Initialize();

		player.SetBoomBoxCompanion(boomboxCompanion);
		boomboxCompanion.GetComponent<BoomboxActionManager>().SwitchState(BoomboxActionType.IDLE);

        return boomboxCompanion;
	}

	private void PrepareWeaponOnLoadGame() {
		WeaponManager weaponManager = player.GetComponent<WeaponManager>();
		weaponManager.RetrieveWeapon(weaponManager.weaponToStartWith, false, false);

		player.GetComponent<CharacterControl>().Start ();
	}

	public void OnMapLoadingError() {
		SaveUtils.DeleteMapSaveFile(1);
		UpdateLoadingText("ERROR, Going back to menu!", -1);
		//Loader.ReloadLevelAndStopLoading();
		Loader.LoadScene(Scene.MenuScene, LoadingScreenType.menu_default);
	}

	// Update is called once per frame
	void Update () {
	
	}

	protected override void BuildLevel() {
		
		List<RoomNode> allRoomNodes = new List<RoomNode>();
		
		Direction[] directionsAvailable = new Direction[1]{ Direction.UP };
		
		List<TileBlockBuilder> allBlockBuilders = new List<TileBlockBuilder>();
		allTileBlocks = new List<TileBlock>();
		
		Direction previousDirection = Direction.NONE;
		TileBlockBuilder previousTileBlockBuilder = null;

		Vector2 gridPosition = new Vector2(0f, 0f);

		Logger.Log ("creating all tileblocks and stuff");
		while(currentTileBlockIndex < tileBlockInfo.Count) {
			
			TileBlockBuilder tileBlockBuilder = CreateTileBlockBuilder(tileBlockInfo[currentTileBlockIndex].tileBlockSize);
			TileBlock tileBlock = tileBlockBuilder.GetTileBlock();

			tileBlockBuilder.name += "_" + currentTileBlockIndex;

			allBlockBuilders.Add (tileBlockBuilder);
			allTileBlocks.Add (tileBlock);
			
			if(currentTileBlockIndex == 0) {
				tileBlock.SetPlayerBlock();
				player.SetCurrentTileBlock(tileBlock);
				playerTileBlock = tileBlock;
			}
			
			tileBlock.worldGridLocation = gridPosition;
			
			Direction nextTileblockDirection = Direction.NONE;
			
			if(currentTileBlockIndex < tileBlockInfo.Count - 1) {
				nextTileblockDirection = directionsAvailable[UnityEngine.Random.Range (0, directionsAvailable.Length)];
				
				if(nextTileblockDirection == Direction.RIGHT) {
					gridPosition += new Vector2(1, 0);
					tileBlock.AddConnectedDirection(Direction.RIGHT, tileBlockInfo[currentTileBlockIndex + 1].tileType);
					
				} else {
					gridPosition += new Vector2(0, 1);
					tileBlock.AddConnectedDirection(Direction.UP, tileBlockInfo[currentTileBlockIndex + 1].tileType);
				}
			}
			
			tileBlockBuilder.PrepareForBuilding(Vector2.zero);
			
			if(currentTileBlockIndex > 0) {
				
				if(previousDirection != Direction.NONE) {
					tileBlock.AddConnectedDirection(MathUtils.GetInverse(previousDirection), tileBlockInfo[currentTileBlockIndex - 1].tileType);
				}
				
				Vector3 spawnPosition = previousTileBlockBuilder.transform.position;
				
				if(previousDirection == Direction.RIGHT) {
					spawnPosition += new Vector3((previousTileBlockBuilder.GetTileBlock().GetWidth()/2 + tileBlock.GetWidth()/2), 0f, 0f);
				} else if(previousDirection == Direction.UP) {
					spawnPosition += new Vector3(0f, 0f, (previousTileBlockBuilder.GetTileBlock().GetHeight()/2 + tileBlock.GetHeight()/2));
				}

				tileBlockBuilder.transform.position = new Vector3(spawnPosition.x, this.transform.position.y, spawnPosition.z);
			} else {
				tileBlockBuilder.transform.position = new Vector3(0f, this.transform.position.y, 0f);
			}
			
			tileBlockBuilder.transform.parent = this.transform;

			previousTileBlockBuilder = tileBlockBuilder;
			previousDirection = nextTileblockDirection;
			
			++currentTileBlockIndex;
			
		}

		UpdateLoadingText("Loading world..", 50);

		Logger.Log ("building all roomnodes in tileblocks");

		allRoomNodes = BuildAllRoomNodesInBlocks(ref allBlockBuilders);

		Logger.Log ("pathfinding");

		PathFindBetweenExitPointsInTileBlocks(ref allBlockBuilders);

		Logger.Log ("spawning all rooms");

		SpawnAllRoomsForTileBlocksInRange(ref allTileBlocks, player.GetCurrentRoomNode().GetTileBlock(), tileBlockSpawnRange);

		Logger.Log ("spawning rooms around player");
		roomNodeSpawnerUtil.SpawnRoomsAroundPlayer (ref player, 0f);

		Logger.Log ("creating minimap");
		minimapBuilder.CreateMiniMapForGrid(ref playerTileBlock, player.GetStartRoomNode().gridLocation);

		Logger.Log ("done");
	}

	private TileBlockBuilder CreateTileBlockBuilder(int blockSize) {
		
		TileType tileType = tileBlockInfo[currentTileBlockIndex].tileType;

		TileBlockBuilder builder = (TileBlockBuilder) GameObject.Instantiate(tileBlockBuilderPrefab, this.transform.position, Quaternion.identity);

		Vector2 villageCenterLocation = GetRandomVillageCenterLocation(blockSize);
		builder.villageLocation = villageCenterLocation;

		builder.Initialize(blockSize);

		builder.SetVillageName(tileBlockInfo[currentTileBlockIndex].villageName);
		builder.SpawnTileBlock(tileType, roomSize, currentTileBlockIndex);
		SceneUtils.FindObject<PlayerSaveComponent>().AddVersionForTileBlock(currentTileBlockIndex, 0);

		return builder;
	}
	
	public override void OnTileBlockEntered(TileBlock newTileBlock, TileBlock oldTileBlock) {

		UnloadAllTileBlocksOutsideRange(ref allTileBlocks, newTileBlock, tileBlockSpawnRange);
		player.SetCurrentTileBlock(newTileBlock);
		minimapBuilder.CreateMiniMapForGrid(ref newTileBlock, player.GetCurrentRoomNode().gridLocation);

	}
	
	public override void OnRoomEntered(RoomTransferData roomTransferData, float cameraTransitionTime) {
		base.OnRoomEntered(roomTransferData, cameraTransitionTime);
		ChangeWeatherBasedOnTileType(roomTransferData.roomNode.GetTileType());
	}

	public void OnTeleportedToRoom(RoomNode teleportedRoomNode) {

		minimapBuilder.SetCurrentGrid(player, player.GetGridLocation());
		currentRoomNode = teleportedRoomNode;
		
		ChangeWeatherBasedOnTileType(teleportedRoomNode.GetTileType());
	}

	private void ChangeWeatherBasedOnTileType(TileType tileType) {

		if(!weatherManager) {
			weatherManager = SceneUtils.FindObject<WeatherManager>();
		}

		switch(tileType) {

			case TileType.one:
				weatherManager.EnableRain();
			break;

			case TileType.five:
				weatherManager.EnableSnow();
			break;
				
			case TileType.four:
			case TileType.three:
				weatherManager.DisableSnow();
				weatherManager.DisableRain();
			break;
				
		}
	}

	private Vector2 GetRandomVillageCenterLocation(float blockSize) {
		int maxPosition = (int) (blockSize * .5f); //.7f
		int minPosition = (int) (blockSize * .5f); //.3f

		return new Vector2(UnityEngine.Random.Range (minPosition, maxPosition), UnityEngine.Random.Range(minPosition, maxPosition));
	}

	private List<RoomNode> BuildAllRoomNodesInBlocks(ref List<TileBlockBuilder> blockBuildersToUse) {
	
		List<RoomNode> allRoomNodes = new List<RoomNode>();

		for(int i = 0 ; i < blockBuildersToUse.Count ; i++) {

			TileBlockBuilder blockBuilder = blockBuildersToUse[i];
			blockBuilder.SpawnNodes();

			blockBuilder.SetAllEndPointNeighboursAsOccupied();

			UnityEngine.Random.seed = System.Guid.NewGuid().GetHashCode();

			List<List<Vector2>> possibleSpawnPositions = GetAllPossibleSpawnPositions(blockBuilder, tileBlockInfo[i].tileBlockSize);

			List<Vector2> possibleSpawnPositionsForEntrance = possibleSpawnPositions[0];
			List<Vector2> possibleSpawnPositionsForExit = possibleSpawnPositions[1];

			POISpawnTypes[] pois = tileBlockInfo[i].pointOfInterestsToSpawn;

			for(int p = 0; p < pois.Length ; p++) {
				POISpawnTypes poi = pois[p];

				switch(poi) {
					case POISpawnTypes.DungeonPOI:
                    case POISpawnTypes.FishingPOI:
					case POISpawnTypes.GameRoomPOI:
						blockBuilder.SpawnPOI(poi, ref possibleSpawnPositionsForEntrance, false);
					break;

					case POISpawnTypes.AnimalRoomPOI:
					case POISpawnTypes.RandomPOI:
					case POISpawnTypes.CassettePOI:
						blockBuilder.SpawnPOI(poi, ref possibleSpawnPositionsForExit, true);
					break;

				}
			}

			allRoomNodes.AddRange(blockBuilder.GetRoomNodesAsList());
		}

		return allRoomNodes;
	}

	private void UnloadAllTileBlocksOutsideRange(ref List<TileBlock> allTileBlocks, TileBlock currentTileBlock, int range) {
		foreach(TileBlock tileBlock in allTileBlocks) {
			if(tileBlock.HasSpawnedRooms() &&  Mathf.Abs (tileBlock.id - currentTileBlock.id) > range) {
				tileBlock.gameObject.SetActive(false);
			}
		}
	}

	private void SpawnAllRoomsForTileBlocksInRange(ref List<TileBlock> allTileBlocks, TileBlock tileBlockToSpawnAround, int range) {

		foreach(TileBlock tileBlock in allTileBlocks) {
			SpawnRoomsForAllTileBlocksWithinRange(tileBlock, tileBlockToSpawnAround, range);
		}
	}

	private void SpawnRoomsForAllTileBlocksWithinRange(TileBlock tileBlock, TileBlock tileBlockToSpawnAround, int range, bool spawnFromSerializedBlock = false) {
		if(tileBlock != null &&  Mathf.Abs (tileBlock.id - tileBlockToSpawnAround.id) <= range) {
			SpawnOrDeSerializeAllRoomsForTileBlock (tileBlock, spawnFromSerializedBlock);
		}
	}

	//TODO: NOT DONE YET, need to pass 'spawnFromSerializedBlock' correctly somehow!
	public void SpawnOrDeSerializeAllRoomsForTileBlock(TileBlock tileBlock, bool spawnFromSerializedBlock = false) {
		if(!tileBlock.HasSpawnedRooms()) {
			if(!spawnFromSerializedBlock) {
				SpawnAllRoomsForTileBlock (tileBlock);
			} else {
				SpawnAllRoomForSerializedTileBlock(tileBlock);
			}
		} else {
			tileBlock.gameObject.SetActive(true);
		}
	}

	private void SpawnAllRoomsForTileBlock(TileBlock tileBlock) {
		foreach(RoomNode roomNode in tileBlock.GetRoomNodesAsList()) {
			roomNode.PrepareRoomForSpawning(ref player);
		}
		tileBlock.SetSpawnedRooms();
	}

	private void SpawnAllRoomForSerializedTileBlock(TileBlock tileBlock) {
		foreach(RoomNode roomNode in tileBlock.GetRoomNodesAsList()) {
			roomNode.version = tileBlock.version;
			SerializationHelper.PrepareRoomForSpawning(roomNode, ref player);
		}
		tileBlock.SetSpawnedRooms();
	}

	private void PathFindBetweenExitPointsInTileBlocks(ref List<TileBlockBuilder> tileBlockBuildersUsed) {

		for(int i = 0 ; i < tileBlockBuildersUsed.Count ; i++) {
			tileBlockBuildersUsed[i].PathFindBetweenExitPoints();
		}
	}

    private void DeSerializeData(bool spawnInJunkyard) {

		SerializableTileBlockContainer serializableTileBlockContainer = GetComponent<DataSerializer>().DeSerialize();
		allTileBlocks = SerializationHelper.DeSerializeTileBlockContainer(this.transform, serializableTileBlockContainer);

        FindPlayerTileBlock(spawnInJunkyard);

		foreach(TileBlock tileBlock in allTileBlocks){
			SpawnRoomsForAllTileBlocksWithinRange(tileBlock, playerTileBlock, tileBlockSpawnRange, true);
		}

		roomNodeSpawnerUtil.SpawnRoomsAroundPlayer (ref player, 0f);

		minimapBuilder.CreateMiniMapForGrid(ref playerTileBlock, player.GetGridLocation());
	}

    private void FindPlayerTileBlock(bool spawnInJunyard) {

        if(spawnInJunyard) {
        
            foreach(TileBlock tileBlock in allTileBlocks){
                if(tileBlock.tileType == TileType.one) {    
                    playerTileBlock = tileBlock;
                    player.SetCurrentTileBlock(playerTileBlock);
                    break;
                }
            }
        
        } else {
            foreach(TileBlock tileBlock in allTileBlocks){
                if(tileBlock.isPlayerCurrentTileBlock) {
                    playerTileBlock = tileBlock;
                    player.SetCurrentTileBlock(playerTileBlock);
                    break;
                }
            }
        }
    }

	private void SerializeData() {

		Logger.Log ("saving map...");

		GetComponent<DataSerializer>().Serialize(allTileBlocks);

	}

	
	public override void SaveData(SpawnType spawnType) {

        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        SceneUtils.FindObject<SaveDisplay>().ShowAnimationForCharacter(playerSaveComponent.currentPlayerName);
        
        SerializeData();
		playerSaveComponent.SaveData(spawnType);
	}

	private List<List<Vector2>> GetAllPossibleSpawnPositions(TileBlockBuilder blockBuilder, int blockSize) {
		List<Vector2> possibleSpawnPositionsForEntrance = new List<Vector2>();
		List<Vector2> possibleSpawnPositionsForExit = new List<Vector2>();

		int maximumYForEntranceSpawnPosition = (int) (blockSize * .4f);
		int minimumYForExitSpawnPosition = (int) (blockSize * .6f);

		for(int x = 2 ; x < blockSize - 1 ; x ++) {
			for(int y = 2 ; y < blockSize - 1 ; y ++) {
				
				Vector2 spawnPosition = new Vector2(x, y);
				RoomNode roomNode = blockBuilder.GetRoomNodes()[x, y];

				if(!blockBuilder.IsAroundVillage(spawnPosition) 
				   && roomNode.GetRoomNodeType() == RoomNodeType.Normal 
				   && !roomNode.IsOccupied()) {

					if(y < maximumYForEntranceSpawnPosition) {
						possibleSpawnPositionsForEntrance.Add (spawnPosition);
					} 

					if(y > minimumYForExitSpawnPosition) {
						possibleSpawnPositionsForExit.Add (spawnPosition);
					}
				}
			}
		}

		List<List<Vector2>> possibleSpawnPositions = new List<List<Vector2>>();

		possibleSpawnPositions.Add (possibleSpawnPositionsForEntrance);
		possibleSpawnPositions.Add (possibleSpawnPositionsForExit);

		return possibleSpawnPositions;
	}
}

