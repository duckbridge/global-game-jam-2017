using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIActionManager : PlatformerAIActionManager {
	
	public override void Awake () {
		bossActions = 
			new List<PlatformerAIAction>(this.transform.Find("Actions").GetComponentsInChildren<PlatformerAIAction>());

		foreach(EnemyAction enemyAction in bossActions) {
			enemyAction.AddEventListener(this.gameObject);
			enemyAction.Initialize(GetComponent<Enemy>());
		}
	}
}
