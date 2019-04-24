using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LevelBuilder : GameManager {

	public RoomNodeSpawnerUtil roomNodeSpawnerUtil;

	public BattleBorders battleBorders;

	public TileBlockBuilder tileBlockBuilderPrefab;
	
	public GameObject cameraContainer;
	public Camera gameCamera;
	
	public Player player;
	
	protected RoomNode currentRoomNode;
	
	protected MinimapBuilder minimapBuilder;
	
	protected Vector2 roomSize;
	protected Vector2 minimapGridSize;

	protected List<TileBlock> allTileBlocks;

	protected bool isNewGame = false;

	public virtual void Awake() {
		gameCamera.enabled = false;
		
		roomSize = new Vector2(71.2f, 40f);
		minimapBuilder = SceneUtils.FindObject<MinimapBuilder>();

		if(!Loader.IS_USING_LOADER) {
			PrepareLevel();
		}
	}

	public override void OnStart () {
		Physics.IgnoreLayerCollision(GameSettings.ANIMALS_LAYER, GameSettings.ANIMALS_LAYER);
		player.FindMusicComponents();

		if(!isNewGame) {
			player.OnStart();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void PrepareLevel() {
		BuildLevel();
		
		PreparePlayer();

        SceneUtils.FindObject<SettingsSaveComponent>().LoadSettingsData();
		SoundUtils.SetSoundVolumeToSavedValue();

		gameCamera.enabled = true;
	}

	protected abstract void BuildLevel();
	protected abstract void PreparePlayer();
	
	public RoomNode GetRoomNodeAtWorldLocation(TileBlock tileBlock, Vector2 worldGridLocation) {
		RoomNode roomNodeToReturn = null;

		roomNodeToReturn = tileBlock.GetRoomNodesAsList().Find(roomNode => roomNode.worldGridLocation == worldGridLocation);
		
		return roomNodeToReturn; 
	}

	public virtual void OnRoomEntered(RoomTransferData roomTransferData, float cameraTransitionTime) {
		roomTransferData.roomNode.isVisited = true;

		player.SetGridLocation(roomTransferData.roomNode.gridLocation);

		if (this.GetComponent<MapBuilder> ()) { //only spawn rooms in overworld
			player.SetCurrentRoomNode(roomTransferData.roomNode);
			roomNodeSpawnerUtil.SpawnRoomsAroundPlayer (ref player, 0.3f);
		}

		minimapBuilder.SetCurrentGrid(player, player.GetGridLocation());

		roomTransferData.roomNode.GetRoom().OnEntered(cameraTransitionTime, ref player);

		if(roomTransferData.roomNode.GetRoom().HasEnemies()) {
			roomTransferData.roomNode.GetRoom().AddEventListener(this.gameObject);
			battleBorders.TurnOnBattleBorders();
		}
		
		player.OnRoomEntered(roomTransferData.roomNode);

		currentRoomNode = roomTransferData.roomNode;
	}

	public virtual void OnTileBlockEntered(TileBlock newTileBlock, TileBlock oldTileBlock) {
	}

	public void OnAllEnemiesDiedInRoom(Room room) {
		player.OnRoomCleared();

		room.RemoveEventListener(this.gameObject);
		battleBorders.TurnOffBattleBorders();
	}
	
	public RoomNode GetCurrentRoomNode() {
		return currentRoomNode;
	}

	public void UpdateLoadingText(string text, int process) {
		//DispatchMessage("OnSetLoadingText", new LoadingMessage(text, process));
	}

	public virtual void SaveData(SpawnType spawnType) {
        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        SceneUtils.FindObject<SaveDisplay>().ShowAnimationForCharacter(playerSaveComponent.currentPlayerName);
        
		playerSaveComponent.SaveData(spawnType);
    }

	public List<TileBlock> GetTileBlocks() {
		return allTileBlocks;
	}

	public bool IsNewGame() {
		return isNewGame;
	}
}
