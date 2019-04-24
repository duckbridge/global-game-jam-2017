using UnityEngine;
using System.Collections;

public class BoomboxIdle : BoomboxAction {

	protected override void OnStarted () {

		if(boomboxCompanion.currentRoom.GetComponent<CassetteRoom>() || boomboxCompanion.currentRoom.GetComponent<GameRoom>()) {
			boomboxCompanion.GetAnimationManager().PlayAnimationByName("Idle");
		} else {
			boomboxCompanion.GetAnimationManager().PlayAnimationByName("Idle-Sleeping");
		}

		boomboxCompanion.SetTextBoxBasedOnContext();

		boomboxCompanion.GetComponent<Collider>().enabled = true;
	}
}