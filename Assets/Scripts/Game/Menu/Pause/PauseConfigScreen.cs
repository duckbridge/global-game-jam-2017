using UnityEngine;
using System.Collections;

public class PauseConfigScreen : PauseSubMenu {

	public override void Update () {
		
		if(!isActive) {
			return;
		}
		
        if(canPressNavigationButton && playerInputActions.down.IsPressed && playerInputActions.down.Value > 0.4f) { 
            canPressNavigationButton = false;
			OnMoveToNextButton();
		}
		
        if(canPressNavigationButton && playerInputActions.up.IsPressed && playerInputActions.up.Value > 0.4f) {
            canPressNavigationButton = false;
			OnMoveToPreviousButton();
		}

		if(canPressNavigationButton && playerInputActions.left.IsPressed && playerInputActions.left.Value > 0.4f && menuButtons[currentIndex].GetComponent<VolumeMenuButton>()) {
            canPressNavigationButton = false;
			menuButtons[currentIndex].GetComponent<VolumeMenuButton>().IncrementVolumeBy(-.1f);
		}
		
		if(canPressNavigationButton && playerInputActions.right.IsPressed && playerInputActions.right.Value > 0.4f && menuButtons[currentIndex].GetComponent<VolumeMenuButton>()) {
            canPressNavigationButton = false;
			menuButtons[currentIndex].GetComponent<VolumeMenuButton>().IncrementVolumeBy(.1f);
		}

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			menuButtons[currentIndex].OnPressed();
		}

        if(!canPressNavigationButton) {
            if(playerInputActions.down.Value == 0 && playerInputActions.up.Value == 0 && playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0) {
                canPressNavigationButton = true;  
            }
        }
	}

	public override void OnMenuButtonPressed (MenuButton menuButton) {

		if(onPressedSound) {
			onPressedSound.Play();
		}

		switch(menuButton.menuButtonType) {
			case MenuButtonType.EXIT:
				SetInactive();
			break;

		}
	}
}
