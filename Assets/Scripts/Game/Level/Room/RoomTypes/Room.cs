using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : DispatchBehaviour {
    
	public List<int> spawnEnemyChances = new List<int>{75, 25};
    public bool alwaysSpawnEnemies = false;
	public float chanceThatRoomIsTrapRoom = .5f;

	public string areaCode = "";

	public Color roomColor = Color.white;

	protected RoomNode roomNode;
	protected Transform chestDropPosition;

	protected bool hasSpawnedDecor = false;
	protected bool hasSpawnedTraps = false;

	protected bool isDefaultRoom = false;
	protected int amountOfEnemies = 0;

	protected bool spawningDisabled = false;

	// Use this for initialization
	public virtual void Awake () {
		chestDropPosition = this.transform.Find("ChestDropPosition");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Initialize(RoomNode roomNode) {
		this.roomNode = roomNode;
	}
		
	public Vector3 GetCameraPosition() {
		return this.transform.Find("CameraLocation").position;
	}

	protected virtual void ActivateEnemiesDelayed() {
		foreach(Enemy enemy in GetComponentsInChildren<Enemy>()) {
			enemy.OnActivate();
		}
	}

	public virtual void OnEntered(float enemyActivationDelay, ref Player playerEntered) {

		roomNode.isVisited = true;

		SpawnItemsAndEnemies();
		
		Invoke ("ActivateEnemiesDelayed", enemyActivationDelay);

		foreach(BeatObject beatObject in GetComponentsInChildren<BeatObject>()) {
			beatObject.Activate();
		}    

		foreach(ObjectThatActivatesOnRoomEnter objectActivatesOnRoomEnter in GetComponentsInChildren<ObjectThatActivatesOnRoomEnter>()) {
			objectActivatesOnRoomEnter.Activate ();
		}   

        Transform decorationTransform = this.transform.Find("Decorations");
        if(decorationTransform && decorationTransform.gameObject.activeInHierarchy) {
            foreach(RoomDecoration roomDecoration in decorationTransform.GetComponentsInChildren<RoomDecoration>()) {
                roomDecoration.Show();
            }
        }
	}

	public void FindBeatListenerForBeatObjects() {
		foreach(BeatObject beatObject in GetComponentsInChildren<BeatObject>()) {
			beatObject.FindBeatListener();
		}
	}

	public virtual void OnExitted() {
		foreach(Enemy enemy in GetComponentsInChildren<Enemy>()) {
			enemy.OnDeActivate();
		}

        Transform decorationTransform = this.transform.Find("Decorations");
        if(decorationTransform && decorationTransform.gameObject.activeInHierarchy) {
            foreach(RoomDecoration roomDecoration in decorationTransform.GetComponentsInChildren<RoomDecoration>()) {
                roomDecoration.Hide();
            }
        }

		foreach(BeatObject beatObject in GetComponentsInChildren<BeatObject>()) {
			beatObject.Deactivate();
		}

		foreach(ObjectThatActivatesOnRoomEnter objectActivatesOnRoomEnter in GetComponentsInChildren<ObjectThatActivatesOnRoomEnter>()) {
			objectActivatesOnRoomEnter.DeActivate ();
		}
	}

	protected void SpawnItemsAndEnemies() {

		if (spawningDisabled) {
			return;
		}

		ItemSpawner[] itemSpawners = GetComponentsInChildren<ItemSpawner>();
		EnemySpawner[] enemySpawners = GetComponentsInChildren<EnemySpawner>();
		DecorSpawner[] decorSpawners = GetComponentsInChildren<DecorSpawner> ();
			
		bool spawnEnemies = true;
        bool spawnTraps = false;

		if(!hasSpawnedTraps && itemSpawners.Length > 0) {
			float calculatedChance = Random.Range(0f, 1f);
			if(chanceThatRoomIsTrapRoom > calculatedChance || enemySpawners.Length == 0) {
                spawnTraps = true;
				spawnEnemies = false;
			}
		}

		if (alwaysSpawnEnemies) {
			SpawnEnemies (enemySpawners);
		} else if(spawnEnemies && spawnEnemyChances.Count > 0) {
			if (Random.Range (0, 100) < spawnEnemyChances [0]) {
				if (spawnEnemyChances.Count > 1) {
					spawnEnemyChances.RemoveAt (0);
				}
				SpawnEnemies (enemySpawners);
			}
		}
        
        if(spawnTraps) {
			SpawnTraps (itemSpawners);
		}

		if (!hasSpawnedDecor) {
			SpawnDecor (decorSpawners);
		}
	}

	private void SpawnEnemies(EnemySpawner[] enemySpawners) {
		foreach(EnemySpawner enemySpawner in enemySpawners) {

			EnemySpawnSummary[] enemySummaries = enemySpawner.GetRandomEnemiesToSpawn();

			if(!enemySummaries[0].name.ToLower().Contains("none")) {

				for(int i = 0 ; i < enemySummaries.Length ; i++) {

					if(enemySummaries[i].name.Length > 0) {
						GameObject enemy = (GameObject)
							GameObject.Instantiate(Resources.Load(enemySummaries[i].name, typeof(GameObject)), enemySpawner.transform.position + enemySummaries[i].spawnPositionOffset, Quaternion.identity) as GameObject;

						enemy.transform.parent = this.transform;

						Enemy enemyObject = enemy.GetComponentInChildren<Enemy>();

						if(enemyObject) {
							enemyObject.OnSpawned(this);
							enemyObject.AddEventListener(this.gameObject);
						}

						SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, enemy.gameObject);

						++amountOfEnemies;
					}

				}
			}
		}
	}

	private void SpawnTraps(ItemSpawner[] itemSpawners) {
		hasSpawnedTraps = true;

		foreach(ItemSpawner itemSpawner in itemSpawners) {
			EnemySpawnSummary[] enemySummaries = itemSpawner.GetRandomItemsToSpawn();

			if(!enemySummaries[0].name.ToLower().Contains("none")) {

				for(int i = 0 ; i < enemySummaries.Length ; i++) {
					GameObject trap = (GameObject)
						GameObject.Instantiate(Resources.Load(enemySummaries[i].name, typeof(GameObject)), itemSpawner.transform.position + enemySummaries[i].spawnPositionOffset, Quaternion.identity) as GameObject;

					trap.transform.parent = this.transform;
				}
			}

		}
	}

	private void SpawnDecor(DecorSpawner[] decorSpawners) {
		hasSpawnedDecor = true;

		foreach (DecorSpawner decorSpawner in decorSpawners) {
			EnemySpawnSummary[] enemySummaries = decorSpawner.GetRandomItemsToSpawn ();

			if (!enemySummaries [0].name.ToLower ().Contains ("none")) {

				for (int i = 0; i < enemySummaries.Length; i++) {
					GameObject decor = (GameObject)
						GameObject.Instantiate (Resources.Load (enemySummaries [i].name, typeof(GameObject)), decorSpawner.transform.position + enemySummaries [i].spawnPositionOffset, Quaternion.identity) as GameObject;

					decor.transform.parent = this.transform;
				}
			}

		}
	}

	public virtual void OnEnemyDied(Enemy enemy) {
		--amountOfEnemies;

		if(amountOfEnemies <= 0) {
			DispatchMessage("OnAllEnemiesDiedInRoom", this);
		}
	}

	public virtual void SpawnCorrectGroundTile() {}

	public TileType GetTileType() {
		return roomNode.GetTileType();
	}

	public RoomNode GetRoomNode() {
		return roomNode;
	}

	public bool CanSpawnEnemy() {
		return GetComponentsInChildren<EnemySpawner>().Length > 0;
	}

	public bool HasEnemies() {

		int amountOfEnemies = GetComponentsInChildren<Enemy>().Length;
		int amountOfVillagers =  GetComponentsInChildren<Villager>().Length;

		int realEnemyCount = amountOfEnemies - amountOfVillagers;

		return realEnemyCount > 0;
	}

	public bool IsDefaultRoom() {
		return isDefaultRoom;
	}

	public void AddEnemySpawned(Enemy enemySpawned) {
		++amountOfEnemies;
		enemySpawned.AddEventListener(this.gameObject);
	}

	protected void DisableSpawning() {
		hasSpawnedDecor = true;
		hasSpawnedTraps = true;
		spawnEnemyChances.Clear ();
		alwaysSpawnEnemies = false;

		spawningDisabled = true;
	}
}
