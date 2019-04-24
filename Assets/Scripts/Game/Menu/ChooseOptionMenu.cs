using UnityEngine;
using System.Collections;

public class ChooseOptionMenu : Menu {

	public MenuButton cancelButton;
    public bool navigateLeftRight = true;

	public override void OnMenuButtonPressed(MenuButton menuButton) {
		MenuButtonWithId menuButtonWithId = (MenuButtonWithId) menuButton;

		if(!this.isActive) {
			return;
		}
        
		if(onPressedSound) {
			onPressedSound.Play();
		}

		DispatchMessage("OnOptionChosen", menuButtonWithId.id);

        this.isActive = false;

	}

	public override void Update () {
		
		if(!isActive) {
			return;
		}

        if(navigateLeftRight) {
		
            if(canPressNavigationButton && playerInputActions.right.IsPressed && playerInputActions.right.Value > 0.4f) {
                canPressNavigationButton = false;
    			OnMoveToNextButton();
    		}
    		
            if(canPressNavigationButton && playerInputActions.left.IsPressed && playerInputActions.left.Value > 0.4f) {
                canPressNavigationButton = false;
    			OnMoveToPreviousButton();
	    	}
        } else {
            if(canPressNavigationButton && playerInputActions.down.IsPressed && playerInputActions.down.Value > 0.4f) {
                canPressNavigationButton = false;
                OnMoveToNextButton();
            }
            
            if(canPressNavigationButton && playerInputActions.up.IsPressed && playerInputActions.up.Value > 0.4f) {
                canPressNavigationButton = false;
                OnMoveToPreviousButton();
            }
        }

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			menuButtons[currentIndex].OnPressed();
		}

		if(playerInputActions.back.WasPressed) {
			cancelButton.OnPressed();
		}

        if(!canPressNavigationButton) {
            if(playerInputActions.up.Value == 0 && playerInputActions.down.Value == 0 && playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0) {
                canPressNavigationButton = true;  
            }
        }
	}
}
