using UnityEngine;
using System.Collections;

public class SpawnEnemiesAction : EnemyAction {

	public float spawnEnemyTimeout = .5f;

	public int stageTwoAmountOfEnemiesToSpawn = 2;
	public string[] stageTwoSpawnableEnemies;

	public SoundObject onSpawnSound;
	public float actionDoneAfterSpawningTimeout = 2f;
	public int amountOfEnemiesToSpawn = 5;

	public string enemyPrefabLocation = "Enemies/";
	public string[] spawnableEnemies;
	
	public float spawnTimeout = 0.5f;

	public string actionOnDone = "MoveAligned";

	private int amountOfEnemiesSpawned = 0;
	private int amountOfEnmiesAlive = 0;

	private BossEnemy bossEnemy;

	protected override void OnActionStarted () {
		
		amountOfEnemiesSpawned = 0;
		BeforeSpawnEnemy();

	}

	private void BeforeSpawnEnemy() {

		bossEnemy = controllingEnemy.GetComponent<BossEnemy>();
		controllingEnemy.PlayAnimationByName("SpawnEnemy", true);
		Invoke ("SpawnEnemy", spawnEnemyTimeout);
	}

	private void SpawnEnemy() {
		onSpawnSound.Play(true);

		int maxAmountOfEnemies = spawnableEnemies.Length;

		if(bossEnemy && bossEnemy.IsInStageTwo()) {
			maxAmountOfEnemies = stageTwoSpawnableEnemies.Length;
		}

		int chosenIndex = Random.Range (0, maxAmountOfEnemies);

		string enemyPrefabName = spawnableEnemies[chosenIndex];

		if(bossEnemy && bossEnemy.IsInStageTwo()) {
			enemyPrefabName = stageTwoSpawnableEnemies[chosenIndex];	
		}

		GameObject enemy = (GameObject)
			GameObject.Instantiate(Resources.Load(enemyPrefabLocation + enemyPrefabName, typeof(GameObject)), controllingEnemy.transform.Find("SpawnPosition").position, Quaternion.identity) as GameObject;
		
		enemy.transform.parent = controllingEnemy.transform.parent;
		
		Enemy enemyObject = enemy.GetComponentInChildren<Enemy>();

		if(enemyObject) {

			SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, enemyObject.gameObject);

			enemyObject.AddEventListener(this.gameObject);
			enemyObject.OnActivate();

			DispatchMessage("OnSpawnedEnemy", enemyObject);

			++amountOfEnemiesSpawned;
			++amountOfEnmiesAlive;

			int maxAmountOfEnemiesToSpawn = amountOfEnemiesToSpawn;

			if(bossEnemy && bossEnemy.IsInStageTwo()) {
				maxAmountOfEnemiesToSpawn = stageTwoAmountOfEnemiesToSpawn;
			}

			if(amountOfEnemiesSpawned < maxAmountOfEnemiesToSpawn) {
				Invoke ("BeforeSpawnEnemy", spawnTimeout);
			} else {
				Invoke ("OnActionDone", actionDoneAfterSpawningTimeout);
			}
		}
	}

	public void OnEnemyDied(Enemy enemy) {
		--amountOfEnmiesAlive;	
	}

	private void OnActionDone() {
		DeActivate(actionOnDone);
	}

	protected override void OnActionFinished () {
		CancelInvoke("SpawnEnemy");
		CancelInvoke("BeforeSpawnEnemy");

		base.OnActionFinished ();
	}
}
