using UnityEngine;
using System.Collections;

public class BlobEnemy : Enemy {

	public Transform[] miniBlobSpawnPositions;
	public int minimumAmountOfBlobsSpawned = 3;
	public int maximumAmountOfBlobsSpawned = 5;

	public GameObject babyBlobToSpawn;

	public void OnSpawnedEnemy(Enemy enemy) {
		this.currentRoom.AddEnemySpawned(enemy);
	}

	protected override void OnDie () {

		if(!isDead) {
			int amountOfBlobsSpawned = Random.Range (minimumAmountOfBlobsSpawned, maximumAmountOfBlobsSpawned+1);
			Player player = SceneUtils.FindObject<Player>();

			for(int i = 0 ; i < amountOfBlobsSpawned ;i++) {
				GameObject enemy = (GameObject)
					GameObject.Instantiate(babyBlobToSpawn, miniBlobSpawnPositions[i].transform.position, Quaternion.identity) as GameObject;
				
				enemy.transform.parent = this.transform.parent;
				
				Enemy enemyObject = enemy.GetComponentInChildren<Enemy>();
				
				if(enemyObject) {
					this.currentRoom.AddEnemySpawned(enemyObject);
					enemyObject.OnActivate();
					enemyObject.OnSpawned (this.currentRoom);
				}

				SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, enemy.gameObject);

			}

			base.OnDie ();
		}

	}
}
