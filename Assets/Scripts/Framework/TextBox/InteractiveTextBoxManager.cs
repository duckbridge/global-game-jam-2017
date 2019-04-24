using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveTextBoxManager : TextBoxManager {

	public PressActionDisplay yesActionDisplay, noActionDisplay;
	private bool interactionCanBeDone = false;
	private bool interactionOptionsShown = false;

	public override void Update (){
	
		if(interactionCanBeDone) {
			if(Input.GetButtonDown("Shoot")) { //yes
			
				DispatchMessage("OnInteractionAccepted", null);
				OnInteractionDone();

			} else if(Input.GetButtonDown("Reload")) { //no
				DispatchMessage("OnInteractionCancelled", null);
				OnInteractionDone();
			}
		} else {
			base.Update ();
		}
	}

	private void OnInteractionDone() {

		yesActionDisplay.Hide ();
		noActionDisplay.Hide ();

		yesActionDisplay.gameObject.SetActive(false);
		noActionDisplay.gameObject.SetActive(false);

		interactionCanBeDone = false;
		interactionOptionsShown = false;

		Hide ();
	}

	protected override void OnTextBoxManagerDone() {

		if(!interactionOptionsShown) {
			interactionOptionsShown  = true;

			yesActionDisplay.gameObject.SetActive(true);
			noActionDisplay.gameObject.SetActive(true);

			yesActionDisplay.Show("Yes", PressActionDisplay.ActionNames.YES);
			noActionDisplay.Show("No", PressActionDisplay.ActionNames.NO);
			interactionCanBeDone = true;
		}
	}
}
