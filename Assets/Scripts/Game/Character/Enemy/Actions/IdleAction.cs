using UnityEngine;
using System.Collections;

public class IdleAction : EnemyAction {

	protected override void OnActionStarted () {
		controllingEnemy.PlayAnimationByName("Idle", true);
	}
}
