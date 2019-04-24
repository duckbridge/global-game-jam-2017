using UnityEngine;
using System.Collections;

public class AnimalRunOutOfScreen : AnimalAction {

	public float outOfScreenRunSpeed = 25f;
	private Vector3 moveDirection;

	protected override void OnUpdate () {
		animalBodycontrol.DoMove(moveDirection.x, moveDirection.z);
	}

	private void OnDoneMoving() {
		if(animalCompanion.GetCurrentRoom() && animalCompanion.GetCurrentRoom().transform.Find("AnimalSpawnPosition")) {
			animalCompanion.transform.position = animalCompanion.GetCurrentRoom().transform.Find("AnimalSpawnPosition").position;
		}

		FinishAction(AnimalActionType.IDLE);
	}

	protected override void OnStarted () {
		animalBodycontrol.moveSpeed = outOfScreenRunSpeed;
		moveDirection = MathUtils.CalculateDirection(animalCompanion.GetTarget().position, this.transform.position);

		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(animalCompanion.GetTarget());
		animalCompanion.GetComponent<CharacterToTargetTurner>().OnUpdate();
		animalCompanion.GetComponent<CharacterToTargetTurner>().enabled = false;

		Invoke ("OnDoneMoving", 5f);
		animalCompanion.GetAnimationControl().PlayAnimationByName("Walking", true);
	}
}