using UnityEngine;
using System.Collections;

public class BoomboxFollowPlayer : BoomboxAction {
	
	public float closeToTargetDistance = 1f;
	
	private Player player;
	private bool isMoving = false;

	protected override void OnUpdate () {

		if(MathUtils.GetDistance2D(player.transform.position, boomboxCompanion.transform.position) > closeToTargetDistance) {
			if(!isMoving) {
				boomboxCompanion.GetAnimationManager().PlayAnimationByName("Walking");
			}

			isMoving = true;

			boomboxCompanion.GetComponent<BodyControl>().MoveKinematic(player.transform.position);
		
		} else {
			if(!player.GetComponent<CharacterControl>().IsRolling()) {
				FinishAction(BoomboxActionType.EQUIP);
			}
		}

	}
	
	protected override void OnStarted () {
		boomboxCompanion.GetComponent<Collider>().enabled = false;
		boomboxCompanion.GetAnimationManager().PlayAnimationByName("Walking");
		player = SceneUtils.FindObject<Player>();
	}
}