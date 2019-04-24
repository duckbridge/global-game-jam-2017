using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

public class PlayerSaveComponent : MonoBehaviour {

	public bool isInsideDungeon = true;
	public PlayerCharacterName currentPlayerName;

	private PlayerCharacterName originalPlayerName = PlayerCharacterName.Rick;

	private VillageTeleporterComponent teleporterComponent;
	private WeaponManager weaponManager;
	private MusicManager musicManager;
	private CollectionManager collectionManager;
	private PowerUpComponent powerupComponent;

	private List<string> savedKiddoNames;
	private List<string> savedAnimalNames;

	private List<int> finishedMinigameIds;
	private List<TileType> deadBorderWalls;

	private int lastBossBeatenId = 0;
	private int lastVillageVisited = 0;
    private int lastEmailId = 0;

	private Dictionary<int, int> tileBlockVersionById = new Dictionary<int, int>();
    private Dictionary<string, string> savedKeyboardInputs = new Dictionary<string, string>();

	private List<TileType> unlockedTileTypeTracks = new List<TileType>();
	private List<SerializableEmail> emails = new List<SerializableEmail>();

    private bool hasFreeBatteryInDetroit = true;
    private bool hasTalkedToBotInCosmoCrater = false;
    private bool hasInsertedCorrectLAPassword = false;
	private List<int> brokenGateIds = new List<int>();
	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize() {
		powerupComponent = SceneUtils.FindObject<PowerUpComponent>();
		collectionManager = SceneUtils.FindObject<CollectionManager>();
		teleporterComponent = SceneUtils.FindObject<VillageTeleporterComponent>();
		weaponManager = SceneUtils.FindObject<WeaponManager>();
		musicManager = SceneUtils.FindObject<MusicManager>();
		savedKiddoNames = new List<string>();
		savedAnimalNames = new List<string>();
		finishedMinigameIds = new List<int>();
		deadBorderWalls = new List<TileType>();
        emails = new List<SerializableEmail>();

		teleporterComponent.Initialize();

		LoadData ();
	}

	public void AddSavedAnimal(string animalName) {
		if(animalName != null && !savedAnimalNames.Contains(animalName)) {
			savedAnimalNames.Add (animalName);
		}
	}

    public void AddEmail(SerializableEmail email) {
        email.emailID = lastEmailId;
        if(email.text.Length > 0 && email.subject.Length > 0) {
            emails.Add(email);
        }
        lastEmailId = emails.Count;
    }

    public void MarkEmailAsRead(int emailID) {
        foreach(SerializableEmail email in emails) {
            if(email.emailID == emailID) email.isRead = true;
        }
    }

    public void AddKeyboardInputToSave(string key, string value) {
        savedKeyboardInputs[key] = value;
    }

	private string DecidePlayerSpawnRoomNodeName(Player player, SpawnType spawnType) {
		string roomNodeName = "playerroom";

		if (player.GetCurrentTileBlock () != null) {
			if (!player.GetCurrentTileBlock ().IsPlayerBlock ()) {
				roomNodeName = "villagecenter";
			}
		}

		if (spawnType == SpawnType.ATGAMECONSOLE) {
			roomNodeName = "villagepart_bottom_left";
		}

		if (spawnType == SpawnType.ATMINIDUNGEON) {
			//roomNodeName = "minidungeon"; TODO: implement later
		}

		return roomNodeName;

	}

	public void SaveData(SpawnType spawnType, bool spawnInJunkyard = false) {
		XmlSerializer serializer = new 
			XmlSerializer(typeof(SerializablePlayerDataSummary));

		SerializablePlayerDataSummary serializablePlayerDataSummary = new SerializablePlayerDataSummary();

		List<int> teleporters = teleporterComponent.GetTileBlockIdsUnlocked();
		for(int i = 0 ; i < teleporters.Count; i++) {
			serializablePlayerDataSummary.villageTeleportersUnlocked.Add (teleporters[i]);
		}

		Player player = SceneUtils.FindObject<Player> ();

		serializablePlayerDataSummary.candyAmount = SceneUtils.FindObject<CandyContainer>().candyAmount;

		serializablePlayerDataSummary.lastBossBeatenId = lastBossBeatenId;

		for(int i = 0 ; i < unlockedTileTypeTracks.Count ; i++) {
			serializablePlayerDataSummary.unlockedTileTypeTracks.Add (unlockedTileTypeTracks[i]);
		}

		serializablePlayerDataSummary.hasFreeBatteryInDetroit = hasFreeBatteryInDetroit;
        serializablePlayerDataSummary.hasTalkedToBotInCosmoCrater = hasTalkedToBotInCosmoCrater;
        serializablePlayerDataSummary.hasInsertedCorrectLAPassword = hasInsertedCorrectLAPassword; 

		serializablePlayerDataSummary.lastSongPlayedTileType = musicManager.GetCurrentMusicTileType();
		serializablePlayerDataSummary.playerName = currentPlayerName;
		serializablePlayerDataSummary.lastVillageVisited = lastVillageVisited;

        serializablePlayerDataSummary.pauseScreenType = SceneUtils.FindObject<PauseScreenManager>().GetPauseScreenType();

		serializablePlayerDataSummary.animalInfo = collectionManager.GetAllAnimalInfo();
		serializablePlayerDataSummary.availableSongs = collectionManager.GetAllMusicInfo();
		serializablePlayerDataSummary.unlockedMinigames = collectionManager.GetAllGameInfo();

		serializablePlayerDataSummary.animationGroup = player.GetAnimationControl().GetCurrentAnimationGroup();

		serializablePlayerDataSummary.finishedMinigameIds = finishedMinigameIds;
		serializablePlayerDataSummary.brokenGateIds = brokenGateIds;

        serializablePlayerDataSummary.rollUnlocked = powerupComponent.HasUnlockedRolling();
		serializablePlayerDataSummary.hasSecondAttackUnlocked = powerupComponent.HasUnlockedSecondAttack ();
		serializablePlayerDataSummary.hasThirdAttackUnlocked = powerupComponent.HasUnlockedThirdAttack ();

		serializablePlayerDataSummary.deadBorderWalls = deadBorderWalls;

        serializablePlayerDataSummary.emails = emails;

        serializablePlayerDataSummary.skipBossCutscene = false;

		serializablePlayerDataSummary.savedKiddoNames = savedKiddoNames;
		serializablePlayerDataSummary.savedAnimalNames = savedAnimalNames;

		serializablePlayerDataSummary.spawnType = spawnType;
        serializablePlayerDataSummary.spawnInJunkyard = spawnInJunkyard;

		serializablePlayerDataSummary.lastScene = (Scene) Enum.Parse(typeof(Scene), Application.loadedLevelName);

		serializablePlayerDataSummary.canThrowWeapon = powerupComponent.CanThrowWeapon();
		serializablePlayerDataSummary.musicAuraType = powerupComponent.GetCurrentAuraType();
        
		serializablePlayerDataSummary.lastSaveDate = DateTime.Now;

		serializablePlayerDataSummary.playerSpawnRoomNode = DecidePlayerSpawnRoomNodeName (player, spawnType);

		List<string> tileBlockVersionsData = new List<string>();

		foreach(KeyValuePair<int, int> entry in tileBlockVersionById) {
			tileBlockVersionsData.Add (entry.Key + "," + entry.Value);
		}

        List<string> keyboardInputData = new List<string>();

        foreach(KeyValuePair<string, string> keyboardInput in savedKeyboardInputs) {
            keyboardInputData.Add(keyboardInput.Key + "," + keyboardInput.Value);
        }

		serializablePlayerDataSummary.tileBlockVersionById = tileBlockVersionsData;
        serializablePlayerDataSummary.savedKeyboardInput = keyboardInputData;

		StreamWriter myWriter = new StreamWriter(GameSettings.GetPlayerDataSaveName());
		serializer.Serialize(myWriter, serializablePlayerDataSummary);
		
		myWriter.Close();
		
		Logger.Log ("done saving player data");
	}

	public void UpdateSpawnInfo(SpawnType spawnType, bool? spawnInJunkyard = null) {
		SerializablePlayerDataSummary savedData = LoadPlayerData();

		if(!savedData.isCorrupt) {

			XmlSerializer serializer = new 
				XmlSerializer(typeof(SerializablePlayerDataSummary));

			savedData.spawnType = spawnType;

			if (spawnInJunkyard.HasValue) {
				savedData.spawnInJunkyard = spawnInJunkyard.Value;
			}

			StreamWriter myWriter = new StreamWriter(GameSettings.GetPlayerDataSaveName());
			serializer.Serialize(myWriter, savedData);
			
			myWriter.Close();
			
			Logger.Log ("done saving only 'usesTeleporter' data");

		}
	}


    public void UpdateSkipBossCutscene(bool skipBossCutscene) {
        SerializablePlayerDataSummary savedData = LoadPlayerData();

        if(!savedData.isCorrupt) {

            XmlSerializer serializer = new 
                XmlSerializer(typeof(SerializablePlayerDataSummary));

            savedData.skipBossCutscene = skipBossCutscene;

            StreamWriter myWriter = new StreamWriter(GameSettings.GetPlayerDataSaveName());
            serializer.Serialize(myWriter, savedData);
            
            myWriter.Close();
            
            Logger.Log ("done saving only 'skipBossCutscene' data");

        }
    }

	public void LoadData() {

			SerializablePlayerDataSummary serializablePlayerDataSummary = LoadPlayerData();

			if(!serializablePlayerDataSummary.isCorrupt) {
				if(serializablePlayerDataSummary.villageTeleportersUnlocked != null) {
					for(int i = 0 ; i < serializablePlayerDataSummary.villageTeleportersUnlocked.Count ; i++) {
						teleporterComponent.AddUnlockedTeleporter (serializablePlayerDataSummary.villageTeleportersUnlocked[i]);
					}
				}

				if(serializablePlayerDataSummary.playerName != currentPlayerName) {
					GetComponent<PlayerManager>().SwapPlayer(serializablePlayerDataSummary.playerName);
				}

				collectionManager.AddAnimalInfoRange(serializablePlayerDataSummary.animalInfo);

				collectionManager.AddMusicInfoRange(serializablePlayerDataSummary.availableSongs);
				collectionManager.AddGameInfoRange (serializablePlayerDataSummary.unlockedMinigames);	

				musicManager.Initialize(serializablePlayerDataSummary.lastSongPlayedTileType, serializablePlayerDataSummary.availableSongs);

				deadBorderWalls = serializablePlayerDataSummary.deadBorderWalls;

				lastVillageVisited = serializablePlayerDataSummary.lastVillageVisited;
				lastBossBeatenId = serializablePlayerDataSummary.lastBossBeatenId;
				currentPlayerName = serializablePlayerDataSummary.playerName;

				finishedMinigameIds = serializablePlayerDataSummary.finishedMinigameIds;
				hasFreeBatteryInDetroit = serializablePlayerDataSummary.hasFreeBatteryInDetroit;
                hasTalkedToBotInCosmoCrater = serializablePlayerDataSummary.hasTalkedToBotInCosmoCrater;
                hasInsertedCorrectLAPassword = serializablePlayerDataSummary.hasInsertedCorrectLAPassword;            

				brokenGateIds = serializablePlayerDataSummary.brokenGateIds;

                emails = serializablePlayerDataSummary.emails;
                lastEmailId = emails.Count;

				savedKiddoNames = serializablePlayerDataSummary.savedKiddoNames;
				savedAnimalNames = serializablePlayerDataSummary.savedAnimalNames;
				
                if(serializablePlayerDataSummary.rollUnlocked) {
                    powerupComponent.UnlockRolling();
                }

				if(serializablePlayerDataSummary.hasSecondAttackUnlocked) {
					powerupComponent.UnlockSecondAttack ();
				}

				if(serializablePlayerDataSummary.hasThirdAttackUnlocked) {
					powerupComponent.UnlockThirdAttack ();
				}

                SceneUtils.FindObject<PauseScreenManager>().SwitchPauseScreen(serializablePlayerDataSummary.pauseScreenType);
                
				CandyContainer candyContainer = SceneUtils.FindObject<CandyContainer>();
				candyContainer.candyAmount = serializablePlayerDataSummary.candyAmount;
				candyContainer.UpdateCandyOutputText();

				if(serializablePlayerDataSummary.canThrowWeapon) {
					powerupComponent.EnableWeaponThrowing();
				}

				powerupComponent.SwapMusicAuraType(serializablePlayerDataSummary.musicAuraType);
                
                for(int i = 0 ; i < serializablePlayerDataSummary.savedKeyboardInput.Count ;i++) {
                    
                    string savedInputKey = serializablePlayerDataSummary.savedKeyboardInput[i].Split(',')[0];
                    string savedInputValue = serializablePlayerDataSummary.savedKeyboardInput[i].Split(',')[1];    
                    
                    AddKeyboardInputToSave(savedInputKey, savedInputValue);
                }
			

				Player player = SceneUtils.FindObject<Player>();
				player.SetNameOfPlayerSpawnRoomNode (serializablePlayerDataSummary.playerSpawnRoomNode);

				if(!isInsideDungeon) {
					player.GetAnimationControl().SwapAnimationGroup(serializablePlayerDataSummary.animationGroup);
				} else {
					player.GetAnimationControl().SwapAnimationGroup(AnimationGroup.Normal);
				}

				Logger.Log ("loaded player data deserializing");
			
		} else {

			tileBlockVersionById = new Dictionary<int, int>();
            savedKeyboardInputs = new Dictionary<string, string>();

			musicManager.Initialize();

		    musicManager.SetCurrentMusicTileType(TileType.none);

			powerupComponent.SwapMusicAuraType(MusicAuraTypes.Sphere);

            SceneUtils.FindObject<PauseScreenManager>().SwitchPauseScreen(PauseScreenTypes.Regular);

		}
	}

	public void AddBrokenGate(int brokenGateId) {
		if (!brokenGateIds.Contains (brokenGateId)) {
			brokenGateIds.Add (brokenGateId);
		}
	}

	public void AddUnlockedTileTypeTrack(TileType tileType) {
		unlockedTileTypeTracks.Add(tileType);
	}

	public List<TileType> GetUnlockedTileTypeTracks() {
		return unlockedTileTypeTracks;
	}

	public List<int> GetBrokenGateIds() {
		return brokenGateIds;
	}

	public List<TileType> LoadUnlockedTileTypeTracksOfAllSlots() {
		List<TileType> tileTypesOfAllSlots = new List<TileType>();

		int originalChosenSLot = GameSettings.CHOSEN_SAVE_SLOT;

		for(int i = 0 ; i < GameSettings.MAX_SAVE_SLOTS ; i++) {
			GameSettings.CHOSEN_SAVE_SLOT = i+1;
			tileTypesOfAllSlots.AddRange(LoadUnlockedTileTypeTracks());
		}

		GameSettings.CHOSEN_SAVE_SLOT = originalChosenSLot;

		return tileTypesOfAllSlots;
	}

	public List<TileType> LoadUnlockedTileTypeTracks() {
		unlockedTileTypeTracks = new List<TileType>();

		XmlSerializer serializer = new XmlSerializer(typeof(SerializablePlayerDataSummary));
		
		if(File.Exists(GameSettings.GetPlayerDataSaveName())) {
			SerializablePlayerDataSummary serializablePlayerDataSummary = new SerializablePlayerDataSummary();
			
			FileStream myFileStream = 
				new FileStream(GameSettings.GetPlayerDataSaveName(), FileMode.Open);
			
			serializablePlayerDataSummary = (SerializablePlayerDataSummary) serializer.Deserialize(myFileStream);

			for(int i = 0 ; i < serializablePlayerDataSummary.unlockedTileTypeTracks.Count ; i++) {
				unlockedTileTypeTracks.Add(serializablePlayerDataSummary.unlockedTileTypeTracks[i]);
			}

			myFileStream.Close();
		}

		return unlockedTileTypeTracks;
	}	

    public bool HasSavedData() {
        bool hasSavedData = false;
    
        for(int i = 1 ; i < GameSettings.MAX_SAVE_SLOTS + 1 ; i++) {
            if(File.Exists(GameSettings.GetPlayerDataSaveNameForSlot(i))) {
                hasSavedData = true;
            }
        }

        return hasSavedData;
    }

	public void LoadTileBlockVersions() {

		XmlSerializer serializer = new XmlSerializer(typeof(SerializablePlayerDataSummary));
		
		if(File.Exists(GameSettings.GetPlayerDataSaveName())) {
			tileBlockVersionById = new Dictionary<int, int>();

			SerializablePlayerDataSummary serializablePlayerDataSummary = new SerializablePlayerDataSummary();

			FileStream myFileStream = 
				new FileStream(GameSettings.GetPlayerDataSaveName(), FileMode.Open);
			
			serializablePlayerDataSummary = (SerializablePlayerDataSummary) serializer.Deserialize(myFileStream);

			for(int i = 0 ; i < serializablePlayerDataSummary.tileBlockVersionById.Count ;i++) {
				
				int tileBlockId = Convert.ToInt32(serializablePlayerDataSummary.tileBlockVersionById[i].Split(',')[0]);
				int tileBlockVersion = Convert.ToInt32(serializablePlayerDataSummary.tileBlockVersionById[i].Split(',')[1]);	
				
				AddVersionForTileBlock(tileBlockId, tileBlockVersion);
			}

			myFileStream.Close();
		}
	}

	public SerializablePlayerDataSummary LoadPlayerData() {
		
		XmlSerializer serializer = new XmlSerializer(typeof(SerializablePlayerDataSummary));
		
		SerializablePlayerDataSummary serializablePlayerDataSummary = new SerializablePlayerDataSummary();

		serializablePlayerDataSummary.isCorrupt = true;
		serializablePlayerDataSummary.lastScene = Scene.MainScene;
		serializablePlayerDataSummary.animationGroup = AnimationGroup.Naked;

		if(File.Exists(GameSettings.GetPlayerDataSaveName())) {
			
			FileStream myFileStream = 
				new FileStream(GameSettings.GetPlayerDataSaveName(), FileMode.Open);
			
			serializablePlayerDataSummary = (SerializablePlayerDataSummary) serializer.Deserialize(myFileStream);
			serializablePlayerDataSummary.isCorrupt = false;

			myFileStream.Close();
		}

		return serializablePlayerDataSummary;

	}

	public bool HasNonOriginalPlayer() {
		return currentPlayerName != originalPlayerName;
	}

	public void SetLastBossBeatenId(int id) {
		this.lastBossBeatenId = id;
	}

	public int GetLastBossBeatenId() {
		return lastBossBeatenId;
	}

	public void AddSavedKiddo(string kiddoName) {
		if(!savedKiddoNames.Contains(kiddoName)) {
			this.savedKiddoNames.Add (kiddoName);
		}
	}

	public List<string> GetSavedAnimals() {
		return this.savedAnimalNames;
	}

	public List<string> GetSavedKiddoNames() {
		return this.savedKiddoNames;
	}

	public void AddDeadBorderWall(TileType tileType) {
		deadBorderWalls.Add (tileType);
	}

	public void AddVersionForTileBlock(int tileBlockId, int version) {
		tileBlockVersionById.Add(tileBlockId, version);	
	}

	public int GetVersion(int idOfTileBlock) {
		int foundVersion = 0;

		tileBlockVersionById.TryGetValue(idOfTileBlock, out foundVersion);

		Logger.Log ("found version " + foundVersion + " for ID " + idOfTileBlock);
		return foundVersion;
	}

	public void SetLastVillageVisited(int lastVillageVisited) {
		if(lastVillageVisited > this.lastVillageVisited) {
			this.lastVillageVisited = lastVillageVisited;
		}
	}

	public void OnMiniGameFinished(int miniGameId) {
		if(!finishedMinigameIds.Contains(miniGameId)) {
			finishedMinigameIds.Add (miniGameId);	
		}
	}

	public bool HasFinishedMiniGame(int miniGameId) {
		return finishedMinigameIds.Contains(miniGameId);
	}

	public int GetLastVisitedVillage() {
		return this.lastVillageVisited;
	}

	public List<TileType> GetDeadBorderWalls() {
		return deadBorderWalls;
	}

    public void OnFreeBatteryInDetroitPickedUp() {
        hasFreeBatteryInDetroit = false;
    }

    public bool HasFreeBatteryInDetroit() {
        return hasFreeBatteryInDetroit;
    }

    public void OnTakedToBotInCosmoCrater() {
        hasTalkedToBotInCosmoCrater = true;
    }

    public bool HasTalkedToBotInCosmoCrater() {
        return hasTalkedToBotInCosmoCrater;
    }
    
    public void OnLaCorrectPasswordInserted() {
        hasInsertedCorrectLAPassword = true;
    }

    public bool HasLaCorrectPasswordInserted() {
        return hasInsertedCorrectLAPassword;
    }

    public string GetSavedInputByName(string key) {
        string value = "";

        if(savedKeyboardInputs.ContainsKey(key)) {    
            value = savedKeyboardInputs[key];
        }

        return value;
    }

    public List<SerializableEmail> GetEmails() {
        return emails;
    }
}
