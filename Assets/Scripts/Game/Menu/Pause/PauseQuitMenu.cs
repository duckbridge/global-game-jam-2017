using UnityEngine;
using System.Collections;
using System;

public class PauseQuitMenu : PauseSubMenu {

    public TextMesh textOutput;

    protected override void OnActivated() {

        SerializablePlayerDataSummary lastSave = SceneUtils.FindObject<PlayerSaveComponent>().LoadPlayerData();

        if(!lastSave.isCorrupt) {

            TimeSpan span = DateTime.Now.Subtract (lastSave.lastSaveDate);

            textOutput.text = "Your last save was " + span.Minutes + (span.Minutes == 1 ? " minute " : " minutes ") + "ago";
        }    else {
            textOutput.text = "You have no saved data";
        }

        base.OnActivated();
    }

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

		if(!canPressNavigationButton) {
		  if(playerInputActions.right.Value == 0 && playerInputActions.left.Value == 0 && playerInputActions.up.Value == 0 && playerInputActions.down.Value == 0) {
		      canPressNavigationButton = true;
		  }
		}

		if(playerInputActions.menuSelect.LastValue == 0 && playerInputActions.menuSelect.IsPressed) {
			menuButtons[currentIndex].OnPressed();
		}
    }

    public override void OnMenuButtonPressed (MenuButton menuButton) {

        if(!this.isActive) {
            return;
        }

        if(onPressedSound) {
            onPressedSound.Play();
        }

        if(menuButton.menuButtonType != MenuButtonType.EXIT) {

            SetInactive();

        } else {
			SetInactive ();
			Time.timeScale = 1f;
			SceneUtils.FindObject<Player> ().GetComponent<Collider> ().enabled = false;
			SceneUtils.FindObject<OnDieEffect> ().StartEffect ();
			Invoke ("LoadMainScene", 1f);
        }
     }

	private void LoadMainScene() {
		PlayerInputHelper.ResetInputHelper ();
		Loader.LoadScene(Scene.MenuScene, LoadingScreenType.menu_default);
	}
}
