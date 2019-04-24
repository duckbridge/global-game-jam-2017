using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {

	public string path = "Items/";
	public string[] itemsToSpawn;

	private SpawnPosition[] spawnPositions;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public EnemySpawnSummary[] GetRandomItemsToSpawn() {
		
		spawnPositions = this.GetComponentsInChildren<SpawnPosition>();
		
		int randomIndex = Random.Range (0, itemsToSpawn.Length);
		string chosenItemSummary = itemsToSpawn[randomIndex];
		
		EnemySpawnSummary[] itemSummariesToReturn = new EnemySpawnSummary[1];
		
		if(chosenItemSummary.Contains("|")) {
			
			itemSummariesToReturn = new EnemySpawnSummary[chosenItemSummary.Split('|').Length];
			string[] enemySummaryData = chosenItemSummary.Split('|');
			
			for(int i = 0 ; i < enemySummaryData.Length ; i++) {
				
				EnemySpawnSummary enemySpawnSummary = new EnemySpawnSummary();
				enemySpawnSummary.name = path + enemySummaryData[i];
				enemySpawnSummary.spawnPositionOffset = spawnPositions[i].transform.localPosition;
				
				itemSummariesToReturn[i] = enemySpawnSummary;
			}
			
		} else {

			EnemySpawnSummary itemSpawnSummary  = new EnemySpawnSummary();

			if(chosenItemSummary.Length > 1) {
				
				itemSpawnSummary.name = path + chosenItemSummary;
				itemSpawnSummary.spawnPositionOffset = Vector3.zero;
				
			} else {
				
				itemSpawnSummary.name = "none";
				itemSpawnSummary.spawnPositionOffset = Vector3.zero;
				
			}
			
			itemSummariesToReturn[0] = itemSpawnSummary;
			
		}
		
		return itemSummariesToReturn;
	}
}
