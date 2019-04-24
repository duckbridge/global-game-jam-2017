using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SerializablePlayerDataSummary {
	
	public int candyAmount = 0;
	public bool isCorrupt = false;

	public Scene lastScene;

	public List<int> villageTeleportersUnlocked = new List<int>();
	public List<int> finishedMinigameIds = new List<int>();
	public List<TileType> deadBorderWalls = new List<TileType>();
    public List<SerializableEmail> emails = new List<SerializableEmail>();
    
	public PlayerCharacterName playerName;
    public PauseScreenTypes pauseScreenType = PauseScreenTypes.Regular;
	public TileType lastSongPlayedTileType = TileType.none;

	public List<MusicInfo> availableSongs = new List<MusicInfo>();
	public List<AnimalInfo> animalInfo = new List<AnimalInfo>();
	public List<TileType> unlockedTileTypeTracks = new List<TileType>();
	public List<GameInfo> unlockedMinigames = new List<GameInfo>();

	public int lastBossBeatenId = 0;
	public int lastVillageVisited = 0;

	public AnimationGroup animationGroup;
	public List<string> savedKiddoNames = new List<string>();
	public List<string> savedAnimalNames = new List<string>();
	public List<int> brokenGateIds = new List<int>();

	public SpawnType spawnType = SpawnType.NORMAL;
    public bool spawnInJunkyard = false;
	public bool canThrowWeapon = false;
	public MusicAuraTypes musicAuraType = MusicAuraTypes.Sphere;

	public List<string> tileBlockVersionById = new List<string>();
    public List<string> savedKeyboardInput = new List<string>();

	public DateTime lastSaveDate;

    public bool rollUnlocked = false;
	public bool hasSecondAttackUnlocked = false;
	public bool hasThirdAttackUnlocked = false;

    public bool hasFreeBatteryInDetroit = true;
    public bool hasTalkedToBotInCosmoCrater = false;
    public bool skipBossCutscene = false;
    public bool hasInsertedCorrectLAPassword = false;
	public string playerSpawnRoomNode = "playerroom";
}
