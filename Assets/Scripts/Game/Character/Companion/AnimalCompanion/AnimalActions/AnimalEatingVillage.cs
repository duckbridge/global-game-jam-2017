using UnityEngine;
using System.Collections;

public class AnimalEatingVillage : AnimalAction {

	public SoundObject eatingSound;

	protected override void OnStarted() {
		animalBodycontrol.StopMoving();

		animalCompanion.GetAnimationManager().PlayAnimationByName("Eating", true);
		eatingSound.Play();
	}
}
