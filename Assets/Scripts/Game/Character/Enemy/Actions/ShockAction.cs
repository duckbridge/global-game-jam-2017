using UnityEngine;
using System.Collections;

public class ShockAction : EnemyAction {

	public SoundObject shockSound;

	public string actionNameOnDone = "PatrolAction";

	public float actionDuration = 1f;
	public string shockAnimationName;
	private Collider shockCollider;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		controllingEnemy.PlayAnimationByName(shockAnimationName, true);
		shockCollider = this.transform.Find("ShockCollider").GetComponent<Collider>();
		shockCollider.enabled = true;
		shockSound.Play();

		Invoke ("OnShockDone", actionDuration); 
	}

	private void OnShockDone() {
		shockCollider.enabled = false;
		DeActivate(actionNameOnDone);
	}

	protected override void OnActionFinished () {
		shockCollider.enabled = false;
		base.OnActionFinished ();
	}
}
