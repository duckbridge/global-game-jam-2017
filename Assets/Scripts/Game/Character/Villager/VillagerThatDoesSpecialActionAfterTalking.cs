using UnityEngine;
using System.Collections;

public class VillagerThatDoesSpecialActionAfterTalking : Villager {

	public override void OnTextBoxDoneAndHidden() {
        player.OnTalkingDone();
        player.GetComponent<PlayerInputComponent>().enabled = true;

		if (GetComponent<VillagerSpecialAction> ()) {
			GetComponent<VillagerSpecialAction> ().DoAction (this);
		}
	}
}
