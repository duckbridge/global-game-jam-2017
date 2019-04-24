using UnityEngine;
using System.Collections;

public class TextBoxManagerWaitsForInteractOrCancel : TextBoxManager {

	public bool quitOnCancel = true;
	public GameObject stuffToShow;
	private bool isWaitingOnInteract = false;

	public override void Update () {
		base.Update ();

		if(isWaitingOnInteract && !isPaused && !isBusy && isActivated) {
			if(playerInputActions.interact.WasPressed) {
				stuffToShow.SetActive (false);
				OnTextBoxManagerDone();
			}

			if (playerInputActions.back.WasPressed) {
				if(quitOnCancel) {
					Logger.Log ("TJO");
					Application.Quit (); //TJO
				}
			}
		}
	}

	protected override void OnBeforeTextBoxManagerDone() {
		isWaitingOnInteract = true;
		if (stuffToShow) {
			stuffToShow.SetActive (true);	
		}
	}
}
