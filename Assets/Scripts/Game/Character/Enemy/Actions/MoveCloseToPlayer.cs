using UnityEngine;
using System.Collections;

public class MoveCloseToPlayer : MoveToPlayer {

	public bool usesCollision = false;

	public float closeToPlayerDistance = 2f;

	protected override void OnUpdate () {
		base.OnUpdate();

		if(Vector3.Distance(player.transform.position, controllingEnemy.transform.position) < closeToPlayerDistance && !usesCollision) {
			DeActivate(actionOnTimeout);
		}
	}

	public void OnTriggerEnter(Collider coll) {

		if(usesCollision) {
			Player player = coll.gameObject.GetComponent<Player>();
			if(player) {
				DeActivate(actionOnTimeout);
			}
		}
	}
}
