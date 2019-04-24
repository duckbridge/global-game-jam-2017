using UnityEngine;
using System.Collections;

public class DoExplodeAction : EnemyAction {

	public Explosion[] explosions;
	public float explosionTimeout = 1f;

	private Blink2D[] allBlinks;
	private bool hasExploded = false;

	protected override void OnActionStarted () {

		hasExploded = true;

		allBlinks = controllingEnemy.GetComponentsInChildren<Blink2D>();

		//Invoke ("StartBlinking", explosionTimeout * .1f);
		//Invoke ("BlinkFaster", explosionTimeout * .5f);
		//Invoke ("BlinkFastest", explosionTimeout * .9f);

		Invoke ("DoExplode", explosionTimeout);

		base.OnActionStarted ();
	}

	public void StartActionForced() {
		OnActionStarted();
	}

	protected override void OnUpdate () {

	}

	private void DoExplode() {
		explosions[0].AddEventListener(this.gameObject);

		controllingEnemy.GetAnimationManager().StopHideAllAnimations();

		foreach(Explosion explosion in explosions) {
			explosion.DoExplode();
		}
	}

	private void StartBlinking() {
		foreach(Blink2D blink2D in allBlinks) {
			blink2D.BlinkWithTimeout(99, 0.2f);
		}
	}
	
	private void BlinkFaster() {
		foreach(Blink2D blink2D in allBlinks) {
			blink2D.BlinkWithTimeout(99, 0.1f);
		}
	}
	
	private void BlinkFastest() {
		foreach(Blink2D blink2D in allBlinks) {
			blink2D.BlinkWithTimeout(99, 0.05f);
		}
	}

	public void OnExplosionDone() {
		controllingEnemy.Kill();
	}

	protected override void OnActionFinished () {

		CancelInvoke("DoExplode");
		CancelInvoke("StartBlinking");
		CancelInvoke("BlinkFaster");
		CancelInvoke("BlinkFastest");

		base.OnActionFinished ();
	}

	public bool HasExploded() {
		return hasExploded;
	}
}
