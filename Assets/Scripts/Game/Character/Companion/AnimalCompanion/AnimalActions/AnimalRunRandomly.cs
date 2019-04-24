using UnityEngine;
using System.Collections;

public class AnimalRunRandomly : AnimalAction {

	public float closeToTargetDistance = .3f;
	public float maximumDistanceFromTarget = 3f;

	private RunPosition randomRunPosition;
	private Player player;
	
	protected override void OnUpdate () {
		if(randomRunPosition) {

			if(Vector3.Distance(player.transform.position, this.transform.position) > maximumDistanceFromTarget) {
				FinishAction(AnimalActionType.FOLLOWING_PLAYER);
			}

			if(Vector3.Distance(randomRunPosition.transform.position, this.transform.position) > closeToTargetDistance) {

				Vector3 directionToMoveIn = (randomRunPosition.transform.position - this.transform.position);
				directionToMoveIn.Normalize();

				animalBodycontrol.DoMove(directionToMoveIn.x, directionToMoveIn.z, false);
			
			} else {
				FinishAction(AnimalActionType.RUNNING_RANDOMLY);
			}

		}
	}
	
	protected override void OnStarted () {
		player = SceneUtils.FindObject<Player>();
		randomRunPosition = player.GetRandomRunPositions();
	}
}