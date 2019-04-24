using UnityEngine;
using System.Collections;

public class AnimalWakeup : AnimalAction {
	
	protected override void OnUpdate () {}
	
	protected override void OnStarted () {
		animalCompanion.GetAnimationManager().PlayAnimationByName("Wakeup");
		animalCompanion.AddEventListener(this.gameObject);
	}

	public void OnAnimalFullHealth() {
		Player player = SceneUtils.FindObject<Player>();

		animalCompanion.GetComponent<CharacterToTargetTurner>().SetTarget(player.transform);
		player.GetComponent<PlayerCircleGrowComponent>().OnAnimalAdded(animalCompanion);
		
		FinishAction(AnimalActionType.FOLLOWING_PLAYER);
	}
}