using UnityEngine;
using System.Collections;

public class SpawnerEnemy : Enemy {

	public void OnSpawnedEnemy(Enemy enemy) {
		this.currentRoom.AddEnemySpawned(enemy);
	}

}
