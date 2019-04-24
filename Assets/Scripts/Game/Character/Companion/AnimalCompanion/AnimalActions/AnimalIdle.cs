using UnityEngine;
using System.Collections;

public class AnimalIdle : AnimalAction {

	protected override void OnStarted() {
		animalBodycontrol.StopMoving();
		animalCompanion.GetAnimationControl().PlayAnimationByName("Idle");
		animalCompanion.AddEventListener(this.gameObject);
	}

	public void OnAnimalFullHealth() {
		Player player = SceneUtils.FindObject<Player>();
		
		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(player.transform);
		player.GetComponent<PlayerCircleGrowComponent>().OnAnimalAdded(animalCompanion);
		
		FinishAction(AnimalActionType.FOLLOWING_PLAYER);
	}
}
