using UnityEngine;
using System.Collections;

public class TextBoxManagerWaitsForInteract : TextBoxManager {

	private bool isWaitingOnInteract = false;

	public override void Update () {
		base.Update ();

		if(isWaitingOnInteract && !isPaused && !isBusy && isActivated) {
			if(playerInputActions.interact.WasPressed) {
				OnTextBoxManagerDone();
			}
		}
	}

	protected override void OnBeforeTextBoxManagerDone() {
		isWaitingOnInteract = true;
	}
}
