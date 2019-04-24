using UnityEngine;
using System.Collections;
using InControl;

public class OnScreenKeyboardKeyRow : Menu {

    public override void Start () {
        menuButtons.ForEach(menuButton => menuButton.AddEventListener(this.gameObject));
    }

    public void SetPlayerInputActions(PlayerInputActions playerInputActions) {
        this.playerInputActions = playerInputActions;
    }

    public override void OnMenuButtonPressed(MenuButton menuButton) {
        if(!this.isActive) {
            return;
        }
    }

    public override void Update () {

        if(!isActive) {
            return;
        }

        if(canPressNavigationButton && playerInputActions.right.IsPressed && playerInputActions.right.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveToNextButton();
        }
        
        if(canPressNavigationButton && playerInputActions.left.IsPressed && playerInputActions.left.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveToPreviousButton();
        }

        if(!canPressNavigationButton) {
            if(playerInputActions.left.Value == 0 && playerInputActions.right.Value == 0) {
                canPressNavigationButton = true;  
            }
        }
        
		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			menuButtons[currentIndex].OnPressed();
		}
    }
}
