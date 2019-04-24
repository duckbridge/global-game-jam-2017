using UnityEngine;
using System.Collections;

public class SuicideEnemy : Enemy {
	
	public DoExplodeAction explosionAction;

	protected override void OnDie () {
		if(!explosionAction.HasExploded()) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			explosionAction.StartActionForced();

			Invoke ("OnReallyDied", 1f);

		} else {
			base.OnDie ();
		}
	}

	private void OnReallyDied() {
		DispatchMessage("OnEnemyDied", this);
		Destroy (this.gameObject);
	}
}
