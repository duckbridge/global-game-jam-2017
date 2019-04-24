using UnityEngine;
using System.Collections;
using InControl;

public class OnScreenKeyboard : DispatchBehaviour {

    public bool enableOnStart = false;
    public int maxCharacters = 7;

    public TextMesh output;
    public OnScreenKeyboardKeyRow[] keyRows;
    public SoundObject onMoveDownSound, onMoveUpSound, onMaxCharactersSound, onBackspaceSound;

    private PlayerInputActions playerInputActions;
    private bool canPressNavigationButton = true;    

    private int currentRowIndex = 0;
    private bool isInCapsMode = true;
    private bool isActive = false;

	// Use this for initialization
	void Awake () {
       
       playerInputActions = PlayerInputHelper.LoadData();

       foreach(OnScreenKeyboardKeyRow keyRow in keyRows) {

            keyRow.SetPlayerInputActions(playerInputActions);

            foreach(OnScreenKeyboardKey key in keyRow.menuButtons) {
                key.AddEventListener(this.gameObject);
            }
        }
	}

    public void Start() {
        if(enableOnStart) {    
            EnableKeyboard();
        }
    }

    public void EnableKeyboard() {
        isActive = true;

        keyRows[0].SetActive();
        keyRows[0].menuButtons[0].OnSelected();

    }

    public void ResetText() {
        output.text = "";
    }
    
    public void DisableKeyboard() {    
        foreach(OnScreenKeyboardKeyRow keyRow in keyRows) {

            keyRow.SetInactive();

            foreach(OnScreenKeyboardKey key in keyRow.menuButtons) {
                key.OnUnSelected();
            }
        }

        isActive = false;
    }

    public void Update () {
            
        if(!isActive) {
            return;        
        }

        if(canPressNavigationButton && playerInputActions.up.IsPressed && playerInputActions.up.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveToPreviousRow();
        }
        
        if(canPressNavigationButton && playerInputActions.down.IsPressed && playerInputActions.down.Value > 0.4f) {
            canPressNavigationButton = false;
            OnMoveToNextRow();
        }


        if(!canPressNavigationButton) {
            if(playerInputActions.up.Value == 0 && playerInputActions.down.Value == 0) {
                canPressNavigationButton = true;  
            }
        }

		if (playerInputActions.back.WasPressed) {
			DoBackSpace ();
			onBackspaceSound.Play (true);
		}
    }

    private void OnMoveToPreviousRow() {
        if(onMoveUpSound) {
            onMoveUpSound.Play();
        }
        
        Menu currentRow = keyRows[currentRowIndex];
        int savedIndex = currentRow.GetCurrentIndex();

        currentRow.menuButtons[savedIndex].OnUnSelected();
        currentRow.SetInactive();
        
        --currentRowIndex;

        if(currentRowIndex < 0) {
            currentRowIndex = keyRows.Length - 1;
        }

        currentRow = keyRows[currentRowIndex];
        
        if(savedIndex >= currentRow.menuButtons.Count) {
            savedIndex = currentRow.menuButtons.Count - 1;
        }

        currentRow.SetCurrentIndex(savedIndex);
        currentRow.menuButtons[savedIndex].OnSelected();
        currentRow.SetActive();
    }

    private void OnMoveToNextRow() {
        if(onMoveDownSound) {
            onMoveDownSound.Play();
        }
        
        Menu currentRow = keyRows[currentRowIndex];
        int savedIndex = currentRow.GetCurrentIndex();

        currentRow.menuButtons[savedIndex].OnUnSelected();
        currentRow.SetInactive();
        
        ++currentRowIndex;

        if(currentRowIndex >= keyRows.Length) {
            currentRowIndex = 0;
        }

        currentRow = keyRows[currentRowIndex];

        if(savedIndex >= currentRow.menuButtons.Count) {
            savedIndex = currentRow.menuButtons.Count - 1;
        }

        currentRow.SetCurrentIndex(savedIndex);
        currentRow.menuButtons[savedIndex].OnSelected();
        currentRow.SetActive();
        
    }

	private void DoBackSpace() {
		if(output.text.Length > 0) {
			output.text = output.text.Substring(0, output.text.Length - 1);
		}
	}

    public void OnMenuButtonPressed(MenuButton menuButton) {

        OnScreenKeyboardKey key = menuButton.GetComponent<OnScreenKeyboardKey>();
        if(key) {

            SoundObject soundToPlay = key.GetOnPressedSound();

            switch(menuButton.menuButtonType) {
                case MenuButtonType.OTHER:
                    if(output.text.Length < maxCharacters) {
                        output.text += key.GetKey();
                    } else {
                        soundToPlay = onMaxCharactersSound;
                    }
                break;

				case MenuButtonType.EXIT:
					DoBackSpace ();
                break;

                case MenuButtonType.STARTGAME:
                    DispatchMessage("OnSubmitPressed", output.text);
                break;

                case MenuButtonType.KEYBINDING:
                    output.text += " ";
                break;
        
                case MenuButtonType.CONTROLS:
                    isInCapsMode = !isInCapsMode;
                    foreach(OnScreenKeyboardKeyRow keyRow in keyRows) {
                        foreach(OnScreenKeyboardKey keyInRow in keyRow.menuButtons) {
                            keyInRow.ToggleCaps(isInCapsMode);
                        }
                    }
                break;
            }

            if(soundToPlay) {
                soundToPlay.Play(true);
            }
        }
    }
}
