using UnityEngine;
using System.Collections;

public class MenuButtonWhichShowsScreen : MenuButtonWithColors {

	public PauseSubMenu pauseSubScreen;

	public override void OnPressed () {
		DispatchMessage("SetInactive", null);

		pauseSubScreen.AddEventListener(this.gameObject);
		pauseSubScreen.gameObject.SetActive(true);
		pauseSubScreen.SetActive();

	}

	public void HidePauseSubScreen() {
		DispatchMessage("SetActive", null);

		pauseSubScreen.RemoveEventListener(this.gameObject);
		pauseSubScreen.gameObject.SetActive(false);

	}
}
