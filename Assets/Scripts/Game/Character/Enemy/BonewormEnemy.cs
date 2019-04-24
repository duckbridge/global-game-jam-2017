using UnityEngine;
using System.Collections;

public class BonewormEnemy : Enemy {

	public Collider defaultCollider, alternativeCollider;

	public void OnSwapColliders(bool swapToAlternativeCollider) {
		if(swapToAlternativeCollider) {
		
			alternativeCollider.enabled = true;
			defaultCollider.enabled = false;
		
		} else {
			
			defaultCollider.enabled = true;
			alternativeCollider.enabled = false;
		}
	}

	protected override void OnDie () {
		base.OnDie ();
		defaultCollider.enabled = false;
		alternativeCollider.enabled = false;
	}

	public override Collider GetCollider () {
		return defaultCollider;
	}
}
