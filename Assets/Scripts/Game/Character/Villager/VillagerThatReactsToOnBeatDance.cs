using UnityEngine;
using System.Collections;

public class VillagerThatReactsToOnBeatDance : Villager {

	public AnimationOnBeatObject animationOnbeat;
	private bool hasReactedToDance = false;

	public override void OnDanceOnBeat (Player player) {
		base.OnDanceOnBeat (player);
		hasReactedToDance = true;

		if (GetComponent<VillagerSpecialAction> ()) {
			GetComponent<VillagerSpecialAction> ().DoAction (this);
		}
	}

	public override void OnInteract (Player player) {

		if (animationOnbeat) {
			animationOnbeat.Pause ();
		}

		base.OnInteract (player);

	}

	public override void OnTextBoxDoneAndHidden () {

		if (!hasReactedToDance) {
			base.OnTextBoxDoneAndHidden ();
		} else {
			player.OnTalkingDone();
			player.GetComponent<PlayerInputComponent>().enabled = true;
		}

		if (animationOnbeat) {
			animationOnbeat.Resume ();
		}
	}
}
