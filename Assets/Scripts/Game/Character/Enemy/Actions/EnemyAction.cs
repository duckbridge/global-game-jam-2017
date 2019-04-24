using UnityEngine;
using System.Collections;

public class EnemyAction : PlatformerAIAction {

	protected Enemy controllingEnemy;
	protected Player player;

	public void Initialize(Enemy enemy) {
		player = SceneUtils.FindObject<Player>();
		this.controllingEnemy = enemy;
	}
}
