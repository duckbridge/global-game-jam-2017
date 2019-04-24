using UnityEngine;
using System.Collections;

public class MoveAligned : EnemyAction {

	public enum AlignDirections { ONLYLEFTRIGHT, ONLYUPDOWN }
	public AlignDirections alignDirections;

	public float moveToPlayerSpeed = 16f;
	public string actionOnAlignedWithPlayer = "ShootPlayer";

	private BodyControl bodyControl;

	protected override void OnActionStarted () {

		bodyControl = controllingEnemy.GetComponent<BodyControl>();

		if(controllingEnemy.GetAnimationControl()) {
			controllingEnemy.GetAnimationControl().PlayAnimationByName("Walking", true);
		} else {
			controllingEnemy.PlayAnimationByName("Walking", true);
		}

		bodyControl.SetMoveSpeed(moveToPlayerSpeed);

		base.OnActionStarted ();
	}

	protected override void OnUpdate () {

		if(alignDirections == AlignDirections.ONLYLEFTRIGHT) {

			float directionX = player.transform.position.x - controllingEnemy.transform.position.x;
			
			if(directionX > 0) {
				directionX = 1f;
			} else if(directionX < 0) {
				directionX = -1f;
			}
			
			bodyControl.DoMove(directionX, 0f, false);
			
			if(Mathf.Abs(controllingEnemy.transform.position.x - player.transform.position.x) < .5f) {
				DeActivate(actionOnAlignedWithPlayer);
			}
		}

		if(alignDirections == AlignDirections.ONLYUPDOWN) {
			float directionZ = player.transform.position.z - controllingEnemy.transform.position.z;

			if(directionZ > 0) {
				directionZ = 1f;
			} else if(directionZ < 0) {
				directionZ = -1f;
			}

			bodyControl.DoMove(0f, directionZ, false);

			if(Mathf.Abs(controllingEnemy.transform.position.z - player.transform.position.z) < .5f) {
				DeActivate(actionOnAlignedWithPlayer);
			}
		}
	}

	protected override void OnActionFinished () {
		if(bodyControl) {
			bodyControl.ResetMoveSpeed();
			bodyControl.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		base.OnActionFinished ();
	}
}
