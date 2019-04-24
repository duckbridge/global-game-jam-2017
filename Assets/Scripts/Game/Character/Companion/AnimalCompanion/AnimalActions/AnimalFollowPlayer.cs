using UnityEngine;
using System.Collections;

public class AnimalFollowPlayer : AnimalAction {

	public float closeToTargetDistance = 1f;
	public float maximumDistanceFromTarget = 15f;
	
	private Player player;
	private bool isMoving = false;

	protected override void OnUpdate () {
	
		if(MathUtils.GetDistance2D(player.transform.position, animalCompanion.transform.position) > maximumDistanceFromTarget) {
			FinishAction(AnimalActionType.TELEPORT_TO_RANDOM_POS);
		}

		if(MathUtils.GetDistance2D(player.transform.position, animalCompanion.transform.position) > closeToTargetDistance) {
			if(!isMoving) {
				animalCompanion.GetAnimationControl().PlayAnimationByName("Walking");
			}

			isMoving = true;
			animalBodycontrol.DoMoveWithAcceleration(player.transform.position);
		
		} else {

			isMoving = false;

			animalBodycontrol.StopMoving();
			animalCompanion.GetAnimationControl().PlayAnimationByName("Idle");
			//FinishAction(AnimalActionType.RUN_AROUND_PLAYER);
		}

	}

	public void OnAnimalNoHealth() {
		RunPosition[] runPositions = animalCompanion.GetCurrentRoom().transform.Find ("AnimalRunPositions").GetComponentsInChildren<RunPosition>();
		
		int randomIndex = Random.Range (0, runPositions.Length);
		
		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(runPositions[randomIndex].transform);
		animalCompanion.SetTarget(runPositions[randomIndex].transform);
		
        animalCompanion.StopHeartParticles();

		FinishAction(AnimalActionType.RUNOUTOFSCREEN);
	}
	
	protected override void OnStarted () {
		isMoving = false;
		animalCompanion.GetAnimationControl().PlayAnimationByName("Walking");
		
        animalCompanion.StartHeartParticles();

		player = SceneUtils.FindObject<Player>();
		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(player.transform);
	}
}