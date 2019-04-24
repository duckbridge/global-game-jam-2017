using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	public LevelBuilder levelBuilder;
	public Player player;

	private CandyContainer candyContainer;
	private HeartContainer heartContainer;

	private List<GameObject> objectsToRewriteOnPlayerSwap = new List<GameObject>();
	private Vector3 playerPosition;
	private CameraBorderManager cameraBorderManager;

	private PlayerSaveComponent playerSaveComponent;

	void Awake() {

		cameraBorderManager = SceneUtils.FindObject<CameraBorderManager>();
		candyContainer = SceneUtils.FindObject<CandyContainer>();
		heartContainer = SceneUtils.FindObject<HeartContainer>();

		playerSaveComponent = GetComponent<PlayerSaveComponent>();
		playerSaveComponent.Initialize();

		if(playerSaveComponent.HasNonOriginalPlayer()) {
			SwapPlayer(playerSaveComponent.currentPlayerName);
		} else {
			heartContainer.Initialize();
			candyContainer.Initialize();

			cameraBorderManager.Initialize();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwapPlayer(PlayerCharacterName newCharacterName) {
	
		playerPosition = player.transform.position;
		bool playerIsAtBoss = player.isAtBoss;
		bool isInTown = player.IsInTown();

		TileBlock playerTileBlock = player.GetCurrentTileBlock();
		RoomNode currentRoomNode = player.GetCurrentRoomNode();

		float musicTime = player.GetMusicManager().GetCurrentMusic().GetSound().time;
		player.GetMusicManager().GetCurrentMusic().Stop();

		Destroy(player.gameObject);

		player = (Player) GameObject.Instantiate(Resources.Load ("Players/"+newCharacterName.ToString().ToLower(), typeof(Player)), playerPosition, Quaternion.identity);
        if(GetComponent<SpecialPlayerSettings>()) {
            GetComponent<SpecialPlayerSettings>().ApplySettings(player);
        }

		if(levelBuilder) {
			levelBuilder.player = player;
		}

		player.SetInTown(isInTown);
		player.isAtBoss = playerIsAtBoss;

		player.transform.parent = this.transform;

		if(playerTileBlock != null) {
			player.SetCurrentTileBlock(playerTileBlock);
		}

		if(currentRoomNode != null) {
			player.SetCurrentRoomNode(currentRoomNode);
		}

		player.Start ();
		player.FindMusicComponents();
		player.OnStart();

		playerSaveComponent.Initialize();
		playerSaveComponent.LoadData();

		heartContainer.Initialize();
		candyContainer.Initialize();

		cameraBorderManager.Initialize();

		player.GetMusicManager().GetCurrentMusic().GetSound().time = musicTime;

		if(currentRoomNode != null) {
			currentRoomNode.GetRoom().FindBeatListenerForBeatObjects();
			currentRoomNode.GetRoom().OnEntered(0f, ref player);
		}

	}
}
