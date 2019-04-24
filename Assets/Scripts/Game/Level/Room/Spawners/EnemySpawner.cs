using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public string path = "Enemies/";
	public string[] enemiesToSpawn;

	private SpawnPosition[] spawnPositions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoSpawn() {
		
	}

	public EnemySpawnSummary[] GetRandomEnemiesToSpawn() {

		spawnPositions = this.GetComponentsInChildren<SpawnPosition>();

		int randomIndex = Random.Range (0, enemiesToSpawn.Length);
		string chosenEnemySummary = enemiesToSpawn[randomIndex];

		EnemySpawnSummary[] enemySummariesToReturn = new EnemySpawnSummary[1];

		if(chosenEnemySummary.Contains("|")) {

			enemySummariesToReturn = new EnemySpawnSummary[chosenEnemySummary.Split('|').Length];
			string[] enemySummaryData = chosenEnemySummary.Split('|');

			for(int i = 0 ; i < enemySummaryData.Length ; i++) {

				if(i < spawnPositions.Length) {
					EnemySpawnSummary enemySpawnSummary = new EnemySpawnSummary();
					enemySpawnSummary.name = path + enemySummaryData[i];
					enemySpawnSummary.spawnPositionOffset = spawnPositions[i].transform.localPosition;

					enemySummariesToReturn[i] = enemySpawnSummary;
				} else {
					EnemySpawnSummary enemySpawnSummary = new EnemySpawnSummary();
					enemySpawnSummary.name = "";
					enemySummariesToReturn[i] = enemySpawnSummary;
				}
			}

		} else {

			EnemySpawnSummary enemySpawnSummary  = new EnemySpawnSummary();

			if(chosenEnemySummary.Length > 1) {
			
				enemySpawnSummary.name = path + chosenEnemySummary;
				enemySpawnSummary.spawnPositionOffset = Vector3.zero;
			
			} else {

				enemySpawnSummary.name = "none";
				enemySpawnSummary.spawnPositionOffset = Vector3.zero;

			}

			enemySummariesToReturn[0] = enemySpawnSummary;

		}

		return enemySummariesToReturn;
	}
}
