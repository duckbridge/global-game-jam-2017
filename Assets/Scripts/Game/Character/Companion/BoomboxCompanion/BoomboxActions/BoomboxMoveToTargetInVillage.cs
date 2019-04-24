using UnityEngine;
using System.Collections;

public class BoomboxMoveToTargetInVillage : BoomboxAction {

	public SoundObject soundToPlayOnMovingDone;

	public float closeToTargetDistance = 1f;
	
	private bool isMoving = false;

	protected override void OnUpdate () {

		if(MathUtils.GetDistance2D(runTarget.transform.position, boomboxCompanion.transform.position) > closeToTargetDistance) {
			if(!isMoving) {
				boomboxCompanion.GetAnimationManager().PlayAnimationByName("Walking");
			}

			isMoving = true;

			boomboxCompanion.GetComponent<BodyControl>().MoveKinematic(runTarget.transform.position);
		
		} else {
			boomboxCompanion.transform.position = new Vector3(runTarget.transform.position.x, boomboxCompanion.transform.position.y, runTarget.transform.position.z);
			soundToPlayOnMovingDone.Play();
			FinishAction(BoomboxActionType.IDLE);
		}

	}
	
	protected override void OnStarted () {
		boomboxCompanion.GetComponent<Collider>().enabled = false;
		boomboxCompanion.GetAnimationManager().PlayAnimationByName("Walking");
	}
}