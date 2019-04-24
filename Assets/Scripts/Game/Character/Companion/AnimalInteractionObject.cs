using UnityEngine;
using System.Collections;

public class AnimalInteractionObject : InteractionObject {

	public AnimalCompanion animal;

	public override void OnInteract (Player player) {
		if(canInteract) {
			base.OnInteract (player);
			animal.SwitchAnimalAction(AnimalActionType.EATING);
		}
	}

}
