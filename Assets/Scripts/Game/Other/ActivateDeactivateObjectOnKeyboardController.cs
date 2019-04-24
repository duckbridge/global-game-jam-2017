using UnityEngine;
using System.Collections;

public class ActivateDeactivateObjectOnKeyboardController : ObjectThatRepondsToKeyboardOrController {
	public GameObject onKeyboardPluggedIn;
	public GameObject onControllerPluggedIn;

	protected override void OnXboxControllerPluggedIn() {
		base.OnXboxControllerPluggedIn();

		onControllerPluggedIn.gameObject.SetActive (true);
		onKeyboardPluggedIn.gameObject.SetActive (false);
	}

	protected override void OnXboxControllerUnPlugged() {
		base.OnXboxControllerUnPlugged();

		onControllerPluggedIn.gameObject.SetActive (false);
		onKeyboardPluggedIn.gameObject.SetActive (true);
	}
}
