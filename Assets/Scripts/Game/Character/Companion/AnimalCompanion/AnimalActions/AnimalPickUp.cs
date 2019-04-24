using UnityEngine;
using System.Collections;

public class AnimalPickUp : AnimalAction {

	public float closeToTargetDistance = 1f;
	public float maximumDistanceFromTarget = 40f;
	
	private Player player;
	
	protected override void OnUpdate () {
	
		if(runTarget == null || Vector3.Distance(player.transform.position, this.transform.position) > maximumDistanceFromTarget) {
			FinishAction(AnimalActionType.FOLLOWING_PLAYER);
		}

		if(runTarget != null && Vector3.Distance(runTarget.transform.position, this.transform.position) > closeToTargetDistance) {

			Vector3 directionToMoveIn = (runTarget.transform.position - this.transform.position);
			directionToMoveIn.Normalize();
			
			animalBodycontrol.DoMove(directionToMoveIn.x, directionToMoveIn.z, false);
		
		}
	}
	
	protected override void OnStarted () {
		player = SceneUtils.FindObject<Player>();
	}
}