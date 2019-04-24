using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseSubMenu : Menu {

	public SoundObject onExittedMenuSound;

	protected override void SelectFirstButton () {
	}

	protected override void OnStartNewGame() {

	}

	protected override void OnLoadGame() {
	}

	public override void Update () {

		if(!isActive) {
			return;
		}

		if(canPressNavigationButton && playerInputActions.left.IsPressed && playerInputActions.left.Value > 0.4f) {
      		canPressNavigationButton = false;
			OnMoveToNextButton();
		}

		if(canPressNavigationButton && playerInputActions.right.IsPressed && playerInputActions.right.Value > 0.4f) {
     		canPressNavigationButton = false;
			OnMoveToPreviousButton();
		}

	    if(canPressNavigationButton && playerInputActions.up.IsPressed && playerInputActions.up.Value > 0.4f) {
			SetInactive();
			DispatchMessage("OnSubMenuDone", this);
		}

	    if(!canPressNavigationButton) {
	        if(playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0 && playerInputActions.up.Value == 0) {
	            canPressNavigationButton = true;
	        }
	    }
	}

	protected override void OnActivated () {
		base.OnActivated ();

		currentIndex = 0;
		currentMenuButton = menuButtons[0];
		currentMenuButton.OnSelected();
	}

	protected override void OnDeactivated () {
		base.OnDeactivated ();

		foreach(MenuButton menuButton in menuButtons) {
			menuButton.OnUnSelected();
		}
		currentIndex = 0;

		DispatchMessage("HidePauseSubScreen", null);
	}

	public override void OnPauseGame() {
	}

	public override void OnResumeGame() {
	}

	public override void SetInactive () {
		if (onExittedMenuSound) {
			onExittedMenuSound.Play ();
		}
		base.SetInactive ();
	}
}
