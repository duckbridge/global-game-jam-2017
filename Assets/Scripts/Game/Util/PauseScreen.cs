using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	public float soundDecreaseOnpause = .5f;
	public PauseMenu pauseMenu;
	public float pauseScreenShowSpeed = 1f;
	private bool isGamePaused = false;

	private enum MoveType { UP, DOWN, LEFT, RIGHT, NONE }
	private MoveType moveType = MoveType.NONE;

	private Vector3 hidePosition;

	private Transform pauseScreenContainer;

	private bool isInSubMenu = false;
	
	// Use this for initialization
	void Awake () {
		pauseMenu.AddEventListener(this.gameObject);
		pauseScreenContainer = this.transform.Find("PauseScreenContainer");
		hidePosition = pauseScreenContainer.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		switch(moveType) {
			case MoveType.UP:
				pauseScreenContainer.localPosition -= new Vector3(0f, 0f, pauseScreenShowSpeed);
				if(pauseScreenContainer.localPosition.z < -1) {
					moveType = MoveType.NONE;
					pauseScreenContainer.localPosition =  new Vector3(0f, 0f, 0f);

					pauseMenu.SetActive();
				}
			break;

			case MoveType.DOWN:
				pauseScreenContainer.localPosition += new Vector3(0f, 0f, pauseScreenShowSpeed);
				if(pauseScreenContainer.localPosition.z > hidePosition.z) {
					moveType = MoveType.NONE;
					pauseScreenContainer.localPosition =  new Vector3(0f, 0f, hidePosition.z);
				}
			break;
		}
	}

	public void HandlePausePressed(bool pausePressed, bool backPressed) {

		if(isGamePaused) {

			RequestUnpause(false, pausePressed, backPressed);

		} else if(pausePressed) {

			isGamePaused = true;
			float currentBGVolume = SoundUtils.GetVolume (SoundType.BG);
			if (currentBGVolume > 0) {
				float newVolume = currentBGVolume * soundDecreaseOnpause;
				SoundUtils.SetSoundVolume (SoundType.BG, newVolume, false);
			}

            SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.HideInstant());
			DoPause();

		}
	}

	public void OnSubMenuEntered() {
		isInSubMenu = true;
	}

	public void RequestUnpause(bool forced, bool pausePressed, bool backPressed) {

        if(forced) {
			if (pausePressed) {
				DeActivateSubMenu ();
				DoUnPause ();            
			}
        } else {

            if(isInSubMenu) {
				if (backPressed) {
					DeActivateSubMenu ();
				}
            } else {
				if (pausePressed) {
					DoUnPause ();
				}
            }
        }
    }

    private void DeActivateSubMenu() {
        pauseMenu.DeactivateSubMenuIfActive();
        if(!pauseMenu.IsSubMenuActive()) {
            isInSubMenu = false;
        }    
    }

	private void DoPause() {
		moveType = MoveType.UP;

		SceneUtils.FindObject<Player>().PlayPauseUnPauseSound();

		Time.timeScale = 0f;

		PauseHelper.PauseGame ();
	}

	private void DoUnPause() {

        pauseMenu.SetInactive();
        isGamePaused = false;
        SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.Show());

		moveType = MoveType.DOWN;
		Time.timeScale = 1f;

		SceneUtils.FindObject<Player>().PlayPauseUnPauseSound();

		SoundUtils.SetSoundVolumeToSavedValue (SoundType.BG);

		PauseHelper.ResumeGame();
	}

    public bool IsGamePaused() {
        return isGamePaused;
    }
}
