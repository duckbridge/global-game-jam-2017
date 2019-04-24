using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : Menu {

	private PauseSubMenu currentSubMenu;

	protected override void OnStartNewGame() {
		SceneUtils.FindObject<PauseScreen>().RequestUnpause(true, true, false);
	}

	protected override void OnLoadGame() {
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

		if(canPressNavigationButton && playerInputActions.down.IsPressed && playerInputActions.down.Value > 0.4f) {

            canPressNavigationButton = false;

			if(menuButtons[currentIndex].GetComponent<MenuButtonWithSubmenu>()) {

				this.SetInactive();

				MenuButtonWithSubmenu buttonWithSubmenu = (MenuButtonWithSubmenu) menuButtons[currentIndex];

				buttonWithSubmenu.OnUnSelected();
				buttonWithSubmenu.ShowSubmenu();

				buttonWithSubmenu.GetMenu().AddEventListener(this.gameObject);
				buttonWithSubmenu.GetMenu().SetActive();

			}
		}

        if(!canPressNavigationButton) {
            if(playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0 && playerInputActions.down.Value == 0) {
                canPressNavigationButton = true;  
            }
        }

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			if(menuButtons[currentIndex].GetComponent<MenuButtonWhichShowsScreen>()) {
				MenuButtonWhichShowsScreen showScreenButton = menuButtons[currentIndex].GetComponent<MenuButtonWhichShowsScreen>();
				if(showScreenButton) {
					
					currentSubMenu = showScreenButton.pauseSubScreen;
					DispatchMessage("OnSubMenuEntered", null);
				}
			}
			if(onPressedSound) {
				onPressedSound.Play();
			}
			menuButtons[currentIndex].OnPressed();
		}
	}
	
	public void OnSubMenuDone(PauseSubMenu pauseSubMenu) {
		pauseSubMenu.RemoveEventListener(this.gameObject);
		menuButtons[currentIndex].OnSelected();

		this.SetActive();
	}

	public override void OnPauseGame() {
	}
	
	public override void OnResumeGame() {
	}

	public void DeactivateSubMenuIfActive() {
		if(currentSubMenu && currentSubMenu.isActive) {
			currentSubMenu.SetInactive();
		}
	}

    public bool IsSubMenuActive() {
        return currentSubMenu == null ? false : currentSubMenu.isActive;
    }

    protected override void OnExitPressed() {

    }
}

