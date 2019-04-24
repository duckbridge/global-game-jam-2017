using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMusicScreen : PauseSubMenu {

	private int currentTrackIndex = 0;
	private MusicManager musicManager;
	private string musicType;

	protected override void OnActivated () {
		base.OnActivated ();

		Player player = SceneUtils.FindObject<Player>();
		musicManager = player.GetMusicManager();

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
            if(playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0) {
                canPressNavigationButton = true;  
            }
        }

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			menuButtons[currentIndex].OnPressed();
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
