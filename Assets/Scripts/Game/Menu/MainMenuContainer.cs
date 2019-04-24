using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class MainMenuContainer : MonoBehaviour {

	public ChooseOptionMenu overrideMenu;

	public Color slotNoDataLoadColor, slotAvailableColor;

	public string slotAvailableText = "SLOT AVAILABLE!";
	public string slotNoDataLoadGameText = "NO DATA!";

	public Fading2D onStartFader;
    public SlotChooseMenu slotChooseMenu;
	public Menu settingsMenu, controlsMenu;

    public MainMenu mainMenu;

    public Animation2D menuScreenAnimation;
    public SoundObject menuScreenSound;

    public enum MainMenuState { mainMenu, newGameSlotMenu, loadGameSlotMenu, activatingSettingsMenu, deactivatingSlotMenu, deactivatingSettingsMenu, activateControlsMenu, deactivatingControlsMenu }
    private MainMenuState mainMenuState;

    private Scene sceneToLoad;
	private int chosenSlotForNewGame = -1;

	// Use this for initialization
	void Awake() {
        slotChooseMenu.AddEventListener(this.gameObject);
        mainMenu.AddEventListener(this.gameObject);
        menuScreenAnimation.AddEventListener(this.gameObject);
		settingsMenu.AddEventListener(this.gameObject);
		controlsMenu.AddEventListener(this.gameObject);
	}
		
	// Update is called once per frame
	void Update () {
	
	}

     public void OnAnimationDone(Animation2D animation2D) {
            
        switch(mainMenuState) {
            case MainMenuState.deactivatingSlotMenu:
            case MainMenuState.deactivatingSettingsMenu:
			case MainMenuState.deactivatingControlsMenu:
                mainMenu.gameObject.SetActive(true);
                mainMenu.SetActive();
            break;

            case MainMenuState.newGameSlotMenu:
            case MainMenuState.loadGameSlotMenu:
                slotChooseMenu.gameObject.SetActive(true);
                slotChooseMenu.SetActive();
            break;

            case MainMenuState.activatingSettingsMenu:
                settingsMenu.gameObject.SetActive(true);
                settingsMenu.SetActive();
            break;

			case MainMenuState.activateControlsMenu:
				controlsMenu.gameObject.SetActive (true);
				controlsMenu.SetActive ();
			break;
        }

		SyncAllButtonColors ();
    }

    public void DoMenuSwitch(MainMenuState newMainMenuState) {
        
        switch(newMainMenuState) {
            case MainMenuState.loadGameSlotMenu:
            case MainMenuState.newGameSlotMenu:

                mainMenu.SetInactive();    
                mainMenu.gameObject.SetActive(false);

				XmlSerializer serializer = new XmlSerializer(typeof(SerializablePlayerDataSummary));

                if(newMainMenuState == MainMenuState.loadGameSlotMenu) {
    
					foreach(MenuButton menuButton in slotChooseMenu.menuButtons) {
    
						SlotChooseMenuButton slotMenuButton = menuButton.GetComponent<SlotChooseMenuButton> ();
						if (slotMenuButton == null) {
							continue;
						}

						slotMenuButton.Initialize ();
						slotMenuButton.SetText(slotMenuButton.GetOriginalText());
    
						if(!SaveUtils.HasMapSaveFile(slotMenuButton.id)) {
							slotMenuButton.ShowData (null, slotNoDataLoadGameText, slotNoDataLoadColor);
							slotMenuButton.Disable();
                    	} else {
						slotMenuButton.ShowData (SaveUtils.LoadSaveFileForSlot(serializer, slotMenuButton.id), "", Color.white);
							slotMenuButton.Enable();
                        }
                    }
                } else {
    
					foreach(MenuButton menuButton in slotChooseMenu.menuButtons) {
                    
						SlotChooseMenuButton slotMenuButton = menuButton.GetComponent<SlotChooseMenuButton> ();
						if (slotMenuButton == null) {
							continue;
						}

                        slotMenuButton.Enable();
    
						if(!SaveUtils.HasMapSaveFile(slotMenuButton.id)) {
							slotMenuButton.Initialize ();
							slotMenuButton.SetText(slotMenuButton.GetOriginalText()); 
							slotMenuButton.ShowData (null, slotAvailableText, slotAvailableColor);
						} else {
							slotMenuButton.Initialize ();
						slotMenuButton.ShowData (SaveUtils.LoadSaveFileForSlot(serializer, slotMenuButton.id), "", Color.white);
						}
                    }
                }
            break;

            case MainMenuState.deactivatingSlotMenu:
                slotChooseMenu.SetInactive();    
                slotChooseMenu.gameObject.SetActive(false);
            break;

            case MainMenuState.deactivatingSettingsMenu:
                settingsMenu.SetInactive();    
                settingsMenu.gameObject.SetActive(false);
            break;

			case MainMenuState.deactivatingControlsMenu:
				controlsMenu.SetInactive();    
				controlsMenu.gameObject.SetActive(false);
			break;
    
			case MainMenuState.activateControlsMenu:
            case MainMenuState.activatingSettingsMenu:
                mainMenu.SetInactive();    
                mainMenu.gameObject.SetActive(false);
            break;
        }

        this.mainMenuState = newMainMenuState;
	
        menuScreenSound.Play();
        menuScreenAnimation.Play(true);
    }

    public void OnQuittingSoundsMenu() {
        
        DoMenuSwitch(MainMenuState.deactivatingSettingsMenu);
    
    }

	public void OnQuittingControlsMenu() {

		DoMenuSwitch(MainMenuState.deactivatingControlsMenu);

	}

    public void OnQuittingSlotsMenu() {

       DoMenuSwitch(MainMenuState.deactivatingSlotMenu);
    }

    public void OnSlotChosen(int chosenSlot) {
        
        switch(mainMenuState) {
			case MainMenuState.newGameSlotMenu:

				if (SaveUtils.HasMapSaveFile (chosenSlot) && overrideMenu) {
					this.chosenSlotForNewGame = chosenSlot;

					overrideMenu.gameObject.SetActive (true);
					SyncAllButtonColors ();
					overrideMenu.AddEventListener (this.gameObject);
					overrideMenu.SetActive ();
					slotChooseMenu.SetInactive ();
					slotChooseMenu.gameObject.SetActive (false);

				} else {
					StartNewGame (chosenSlot);
				}
            break;

            case MainMenuState.loadGameSlotMenu:
                if(SaveUtils.HasMapSaveFile(chosenSlot)) {
					slotChooseMenu.SetInactive ();
				    
					GameSettings.CHOSEN_SAVE_SLOT = chosenSlot;
                    
					sceneToLoad = Scene.MainScene;
					GameObject.Find ("CameraContainer/OnStartFadeIn").GetComponent<Fading2D> ().FadeInto (Color.black);
					Invoke ("LoadNewScene", 1f);
                }
            break;
        }
    }

	public void OnOptionChosen(int menuButtonId) {
		if (chosenSlotForNewGame != -1) {
			if (menuButtonId == 1) {
				overrideMenu.SetInactive ();
				StartNewGame (chosenSlotForNewGame);
			} else {
				overrideMenu.SetInactive ();
				overrideMenu.gameObject.SetActive (false);
				slotChooseMenu.gameObject.SetActive (true);
				slotChooseMenu.SetActive ();
				SyncAllButtonColors ();
			}
		}
	}

	private void StartNewGame(int chosenSlot) {
		slotChooseMenu.SetInactive ();

		SaveUtils.DeleteMapSaveFile (chosenSlot);
		SaveUtils.DeleteTPFile (chosenSlot);

		GameSettings.SKIP_START_CUTSCENE = false;
		GameSettings.CHOSEN_SAVE_SLOT = chosenSlot;

		sceneToLoad = Scene.Intro;

		GameObject.Find ("CameraContainer/OnStartFadeIn").GetComponent<Fading2D> ().FadeInto (Color.black);
		Invoke ("LoadNewScene", 1f);
	}

	public void LoadNewScene() {
		PlayerInputHelper.ResetInputHelper ();
        Loader.LoadScene (sceneToLoad, LoadingScreenType.overworld_newgame);
    }

	private void SyncAllButtonColors() {
		SceneUtils.FindObjects<MenuButtonColorsInOrderOnBeatObject> ()
			.ForEach (mbco => mbco.SyncCurrentIndex ());
	}
}
