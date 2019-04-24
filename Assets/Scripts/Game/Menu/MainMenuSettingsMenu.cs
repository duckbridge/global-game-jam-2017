using UnityEngine;
using System.Collections;

public class MainMenuSettingsMenu : Menu {
	public MenuButton cancelButton;

    public override void Update () {
        
        if(!isActive) {
            return;
        }
        
        if(playerInputActions.down.WasPressed) {
            OnMoveToNextButton();
        }
        
        if(playerInputActions.up.WasPressed) {
            OnMoveToPreviousButton();
        }

        if(playerInputActions.left.WasPressed && menuButtons[currentIndex].GetComponent<VolumeMenuButton>()) {
            menuButtons[currentIndex].GetComponent<VolumeMenuButton>().IncrementVolumeBy(-.1f);
        }
        
        if(playerInputActions.right.WasPressed && menuButtons[currentIndex].GetComponent<VolumeMenuButton>()) {
            menuButtons[currentIndex].GetComponent<VolumeMenuButton>().IncrementVolumeBy(.1f);
        }

        if((playerInputActions.left.WasPressed || playerInputActions.right.WasPressed) && menuButtons[currentIndex].GetComponent<ToggleMenuButton>()) {
            menuButtons[currentIndex].GetComponent<ToggleMenuButton>().DoToggle();
        }

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
            menuButtons[currentIndex].OnPressed();
        }

		if(playerInputActions.back.WasPressed) {
			cancelButton.OnPressed();
		}
    }

    public override void OnMenuButtonPressed(MenuButton menuButton) {
        if(!this.isActive) {
            return;
        }

        if(onPressedSound) {
            onPressedSound.Play();
        }
        
        if(menuButton.menuButtonType == MenuButtonType.EXIT) {
            DispatchMessage("OnQuittingSoundsMenu", null);
        }

    }
}
