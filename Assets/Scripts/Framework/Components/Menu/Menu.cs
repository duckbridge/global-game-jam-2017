using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class Menu : DispatchBehaviour {

	public List<MenuButton> menuButtons;

    public bool canCycle = true;
	public bool isActive = true;
	public Scene sceneAfterStartPressed;
	public float menuStateResetTime = .5f;

	public SoundObject onMoveDownSound, onMoveUpSound, onPressedSound;

	protected MenuButton currentMenuButton;
	protected int currentIndex = 0;

	protected PlayerInputActions playerInputActions;
    protected bool canPressNavigationButton = true;    

	// Use this for initialization
	public virtual void Start () {
		playerInputActions = PlayerInputHelper.LoadData ();
		menuButtons.ForEach(menuButton => menuButton.AddEventListener(this.gameObject));

		SelectFirstButton();
	}

	protected virtual void SelectFirstButton() {
		if(menuButtons.Count > 0) {
			currentMenuButton = menuButtons[0];
			currentMenuButton.OnSelected();
		}
	}

	protected virtual void OnMoveToPreviousButton() {
		if(onMoveUpSound) {
			onMoveUpSound.Play();
		}

		menuButtons[currentIndex].OnUnSelected();
		--currentIndex;
		if(currentIndex < 0) {
			currentIndex = canCycle ? menuButtons.Count - 1 : 0;
		}

		if(!menuButtons[currentIndex].IsMovable()) {
			OnMoveToPreviousButton();
		}

		menuButtons[currentIndex].OnSelected();
	}

	protected virtual void OnMoveToNextButton() {
		if(onMoveDownSound) {
			onMoveDownSound.Play();
		}

		menuButtons[currentIndex].OnUnSelected();
		++currentIndex;

		if(currentIndex >= menuButtons.Count) {
			currentIndex = canCycle ? 0 : menuButtons.Count - 1;
		}

		if(!menuButtons[currentIndex].IsMovable()) {
			OnMoveToNextButton();
		}

		menuButtons[currentIndex].OnSelected();
	}

	public virtual void OnMenuButtonPressed(MenuButton menuButton) {
		if(!this.isActive) {
			return;
		}

		if(onPressedSound) {
			onPressedSound.Play();
		}

		switch(menuButton.menuButtonType) {
			case MenuButtonType.EXIT:
				OnExitPressed();
			break;
			case MenuButtonType.STARTGAME:
				OnStartNewGame();
			break;
			case MenuButtonType.LOADGAME:
				OnLoadGame();
			break;

			case MenuButtonType.CONTROLS:
				OnControlsPressed();
			break;

            case MenuButtonType.SETTINGS:
                OnSettingsPressed();
            break;
		}
		this.isActive = false;
	}
	
	protected virtual void OnStartNewGame() {}
	protected virtual void OnLoadGame() {}
	protected virtual void OnControlsPressed() {}
    protected virtual void OnSettingsPressed() {}
	protected virtual void OnActivated() {}
	protected virtual void OnDeactivated() {}

	protected virtual void OnExitPressed() {
		Application.Quit();
	}

	public virtual void SetActive() {

		if (playerInputActions == null) {
			playerInputActions = PlayerInputHelper.LoadData ();
			ActivateMenu ();
		} else {
			ActivateMenu ();
		}
	}

	public void OnDoneWaiting() {
		ActivateMenu ();
	}

	protected void ActivateMenu() {
		if (!isActive) {
			this.isActive = true;

			OnActivated ();
		}
	}

	public virtual void SetInactive() {
		this.isActive = false;
		
		OnDeactivated();
	}

	public virtual void Update () {

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

        if(!canPressNavigationButton) {
            if(playerInputActions.up.Value == 0 && playerInputActions.down.Value == 0) {
                canPressNavigationButton = true;  
            }
        }

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
            if(currentIndex < menuButtons.Count) {
                menuButtons[currentIndex].OnPressed();
            }
        }
	}

    public int GetCurrentIndex() {
        return currentIndex;
    }

    public void SetCurrentIndex(int currentIndex) {
        this.currentIndex = currentIndex;
    }
}
