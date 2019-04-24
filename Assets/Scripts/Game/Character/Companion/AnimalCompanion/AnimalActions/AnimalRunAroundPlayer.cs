using UnityEngine;
using System.Collections;

public class AnimalRunAroundPlayer : AnimalAction {

	public float closeToTargetDistance = .3f;
	public float maximumDistanceFromTarget = 3f;

	private RunPosition currentRunPosition;
	private int currentRunIndex = -1;

	private Player player;
	
	protected override void OnUpdate () {
		if(currentRunPosition) {

			if(MathUtils.GetDistance2D(player.transform.position, animalCompanion.transform.position) > maximumDistanceFromTarget) {
				FinishAction(AnimalActionType.FOLLOWING_PLAYER);
			}

			if(MathUtils.GetDistance2D(currentRunPosition.transform.position, animalCompanion.transform.position) > closeToTargetDistance) {

				animalBodycontrol.DoMoveWithMinimumSpeed(currentRunPosition.transform.position, .05f);
			
			} else {
				FinishAction(AnimalActionType.RUN_AROUND_PLAYER);
			}
		}
	}
	
	protected override void OnStarted () {
		player = SceneUtils.FindObject<Player>();
		currentRunIndex = player.GetCircleRunPositions().GetNextRunPositionIndex(currentRunIndex);
		currentRunPosition = player.GetCircleRunPositions().runPositions[currentRunIndex];

		animalCompanion.GetAnimationControl().PlayAnimationByName("Walking");

	}
}