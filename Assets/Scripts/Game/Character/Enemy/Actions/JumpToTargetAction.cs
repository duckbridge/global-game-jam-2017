using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpToTargetAction : EnemyAction {

	public SoundObject jumpSound;

	public bool alsoJumpToPlayer = true;
	public Vector2 cameraShakeAmount = Vector2.zero;

	public float jumpTime = 1f;
	public float minimumJumpTimeout = .5f;
	public float maximumJumpTimeout = 1.5f;
	public float jumpSpeed = .5f;

	public List<Transform> jumpTargets;

	private List<Transform> availableJumpTargets;
	private Transform currentJumpTarget, previousJumpTarget;

	private bool isJumping = false;

	protected override void OnActionStarted () {

		Player player = SceneUtils.FindObject<Player>();
		if(alsoJumpToPlayer && !jumpTargets.Contains(player.transform)) {
			jumpTargets.Add (player.transform);
		}

		availableJumpTargets = new List<Transform>(jumpTargets);


		Invoke ("DoJump", Random.Range (minimumJumpTimeout, maximumJumpTimeout));
		base.OnActionStarted ();
	}

	protected override void OnUpdate () {
		if(isJumping) {

			Vector3 correctedJumpTarget = new Vector3(currentJumpTarget.position.x, controllingEnemy.transform.position.y, currentJumpTarget.position.z);

			Vector3 directionToJumpTarget = MathUtils.CalculateDirection(correctedJumpTarget, controllingEnemy.transform.position);
			directionToJumpTarget.Normalize();

			controllingEnemy.transform.position += directionToJumpTarget * jumpSpeed;
		}
	}

	private void StopJumping() {
		CancelInvoke("StopJumping");

		if(cameraShakeAmount != Vector2.zero) {
			SceneUtils.FindObject<CameraShaker>().ShakeCamera(cameraShakeAmount, true);
		}

		isJumping = false;
		controllingEnemy.PlayAnimationByName("Walking", true);

		Invoke ("DoJump", Random.Range (minimumJumpTimeout, maximumJumpTimeout));
	}

	protected override void OnActionFinished () {
		CancelInvoke("StopJumping");
		CancelInvoke("DoJump");

		base.OnActionFinished ();
	}

	private void DoJump() {

		jumpSound.Play();

		previousJumpTarget = currentJumpTarget;

		if(previousJumpTarget) {
			availableJumpTargets.Remove (previousJumpTarget);
		}

		int jumpTargetIndex = Random.Range (0, availableJumpTargets.Count);

		currentJumpTarget = availableJumpTargets[jumpTargetIndex];

		controllingEnemy.PlayAnimationByName("Jumping", true);
		isJumping = true;

		if(previousJumpTarget) {
			availableJumpTargets.Add(previousJumpTarget);
		}

		Invoke ("StopJumping", jumpTime);
	}
}
