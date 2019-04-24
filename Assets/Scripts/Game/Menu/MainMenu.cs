using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : Menu {

    public SlotChooseMenu slotChooseMenu;
	public MenuMusicManager menuMusicManager;
	public MenuKiddoManager menuKiddoManager;

	public float showZPosition = 20f;
	public float hideZPosition = 50f;

	public Transform settingsShowPosition, settingsHidePosition;

	private List<SerializablePlayerDataSummary> allSaveFiles;


	public override void Start () {
		PlayerInputHelper.ResetInputHelper ();
		base.Start ();

		allSaveFiles = SaveUtils.LoadAllSaveFiles();

		menuKiddoManager.Initialize(allSaveFiles);

		SaveUtils.CreateMapSaveFolderIfNotExist();

		if(allSaveFiles.Count == 0) {
			menuButtons[1].Disable();
		}

        SettingsSaveComponent settingsSaveComponent = SceneUtils.FindObject<SettingsSaveComponent>();
        if(settingsSaveComponent) {
            SerializableSettingsDataSummary settingsDataSummary = settingsSaveComponent.LoadSettingsData();
            if(settingsDataSummary.isCorrupt) {
                settingsSaveComponent.SaveSettingsData();
                settingsSaveComponent.LoadSettingsData();
            }
        }

        SoundUtils.SetSoundVolumeToSavedValue();
	}

	protected override void OnStartNewGame() {
       DispatchMessage("DoMenuSwitch", MainMenuContainer.MainMenuState.newGameSlotMenu);
	}
   
	protected override void OnLoadGame() {
	    DispatchMessage("DoMenuSwitch", MainMenuContainer.MainMenuState.loadGameSlotMenu);
	}

    protected override void OnSettingsPressed() {
        DispatchMessage("DoMenuSwitch", MainMenuContainer.MainMenuState.activatingSettingsMenu);
    }

	protected override void OnControlsPressed () {
		DispatchMessage("DoMenuSwitch", MainMenuContainer.MainMenuState.activateControlsMenu);
	}

	public override void Update () {

		if(playerInputActions.nextTrack.WasPressed) {
			menuMusicManager.SwapToNextTrack();
		}
		
		if(playerInputActions.previousTrack.WasPressed) {
			menuMusicManager.SwapToPreviousTrack();
		}

		base.Update();
	}
		
	private void EnableMainMenu() {
		SetActive();
	}

	protected override void OnExitPressed () {
		GameObject.Find ("CameraContainer/OnStartFadeIn").GetComponent<Fading2D> ().FadeInto (Color.black);
		Invoke ("CloseGame", 1f);
	}

	private void CloseGame() {
		Application.Quit ();
	}
}
