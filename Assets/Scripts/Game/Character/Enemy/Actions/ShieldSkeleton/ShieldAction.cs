using UnityEngine;
using System.Collections;

public class ShieldAction : EnemyAction {

	public string actionOnDone = "Charge";
	public float minimumShieldTimeout = 3f;
	public float maximumShieldTimeout = 3f;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		this.AddEventListener(controllingEnemy.gameObject);

		Invoke ("EnableShield", .5f);

		controllingEnemy.PlayAnimationByName("UseShield", true);
		Invoke ("RemoveShield", Random.Range (minimumShieldTimeout, maximumShieldTimeout));
	}

	private void EnableShield() {
		DispatchMessage("OnShieldUsed", null);
	}

	private void RemoveShield() {
		controllingEnemy.PlayAnimationByName("RemoveShield", true);
		Invoke ("OnDoneWithShielding", .5f);
	}

	private void OnDoneWithShielding() {
		DispatchMessage("OnShieldDone", null);
		DeActivate(actionOnDone);
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();

		CancelInvoke("EnableShield");
		CancelInvoke("RemoveShield");
		CancelInvoke("OnDoneWithShielding");
	}
}
