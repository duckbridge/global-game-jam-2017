using UnityEngine;
using System.Collections;

public class AnimalIdleVillage : AnimalAction {

	protected override void OnStarted() {

		SceneUtils.FindObject<PlayerSaveComponent>().AddSavedAnimal(animalCompanion.GetOriginalName());
		animalBodycontrol.StopMoving();
		animalCompanion.GetAnimationControl().PlayAnimationByName("Idle");

		this.AddEventListener(animalCompanion.gameObject);
		DispatchMessage("OnIdleInVillage", null);

	}
}
