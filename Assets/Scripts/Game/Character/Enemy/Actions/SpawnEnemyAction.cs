using UnityEngine;
using System.Collections;

public class SpawnEnemyAction : EnemyAction {

	public string enemyPrefabLocation = "Enemies/";
	public string[] spawnableEnemies;
	
	public float spawnTimeout = 1.5f;

	public string actionOnDone = "MoveAligned";

	protected override void OnActionStarted () {

		BeforeSpawnEnemy();

	}

	private void BeforeSpawnEnemy() {
	
		controllingEnemy.PlayAnimationByName("SpawnEnemy", true);
		Invoke ("SpawnEnemy", .5f);
		Invoke ("OnActionDone", spawnTimeout);
	}

	private void SpawnEnemy() {

        controllingEnemy.SetCanDie(false);

		int chosenIndex = Random.Range (0, spawnableEnemies.Length);

		GameObject enemy = (GameObject)
			GameObject.Instantiate(Resources.Load(enemyPrefabLocation + spawnableEnemies[chosenIndex], typeof(GameObject)), controllingEnemy.transform.Find("SpawnPosition").position, Quaternion.identity) as GameObject;
		
		enemy.transform.parent = controllingEnemy.transform.parent;
		SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, enemy.gameObject);

		Enemy enemyObject = enemy.GetComponentInChildren<Enemy>();
		
		if(enemyObject) {
			enemyObject.OnActivate();
			DispatchMessage("OnSpawnedEnemy", enemyObject);
		}

        controllingEnemy.SetCanDie(true);
	}

	private void OnActionDone() {
		DeActivate(actionOnDone);
	}

	protected override void OnActionFinished () {
        CancelInvoke("BeforeSpawnEnemy");
        CancelInvoke("SpawnEnemy");
		
		base.OnActionFinished ();
	}
}
