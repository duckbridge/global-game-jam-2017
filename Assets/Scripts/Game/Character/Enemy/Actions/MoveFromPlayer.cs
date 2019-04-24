using UnityEngine;
using System.Collections;

public class MoveFromPlayer : EnemyAction {

	public string actionOnTimeout = "Idle";
	public float actionTimeout = 0f;
	
	public float moveFromPlayerSpeed = 16f;
	
	private BodyControl bodyControl;
	
	protected override void OnActionStarted () {
		
		bodyControl = controllingEnemy.GetComponent<BodyControl>();
		controllingEnemy.PlayAnimationByName("Walking", true);
		bodyControl.SetMoveSpeed(moveFromPlayerSpeed);
		
		if(actionTimeout > 0) {
			Invoke ("OnActionTimedOut", actionTimeout);
		}
		
		base.OnActionStarted ();
	}
	
	protected override void OnUpdate () {
		Vector3 direction = MathUtils.CalculateDirection(controllingEnemy.transform.position, player.transform.position);
		bodyControl.DoMove(direction.x, direction.z, false);
	}
	
	protected override void OnActionFinished () {
		if(bodyControl) {
			bodyControl.ResetMoveSpeed();
			bodyControl.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		
		base.OnActionFinished ();
	}
	
	private void OnActionTimedOut() {
		DeActivate(actionOnTimeout);
	}
}
