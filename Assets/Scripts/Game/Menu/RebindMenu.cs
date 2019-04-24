using UnityEngine;
using System.Collections;
using InControl;

public class RebindMenu : Menu {

	public BindingSourceType bindingSourceType;
	public GameObject changeInputPrompt;

	private PlayerInputActions playerInputActions;

	public void SetPlayerInputActions(PlayerInputActions playerInputActions) {
		this.playerInputActions = playerInputActions;

		foreach(PlayerAction playerAction in playerInputActions.Actions) {

			if(this.transform.Find("Inputs/"+playerAction.Name)) {

				foreach(BindingSource bindingSource in playerAction.Bindings) {
					if(bindingSource.BindingSourceType == bindingSourceType) {
						this.transform.Find("Inputs/"+playerAction.Name).GetComponent<TextMesh>().text = bindingSource.Name;
					}
				}
			}
		}
	}

	public override void OnMenuButtonPressed(MenuButton menuButton) {

		if(onPressedSound) {
			onPressedSound.Play();
		}

		switch(menuButton.menuButtonType) {
			case MenuButtonType.KEYBINDING:

				PlayerAction foundAction = null;

				foreach(PlayerAction playerAction in playerInputActions.Actions) {
					if(playerAction.Name == menuButton.name) {
						foundAction = playerAction;
					}
				}

				if(foundAction != null) {
					changeInputPrompt.SetActive(true);
					
					isActive = false;
					
					for(int i = 0 ; i < foundAction.Bindings.Count ; i++) {
						if(foundAction.Bindings[i].BindingSourceType == bindingSourceType) {
							foundAction.RemoveBindingAt(i);
						}
					}

					foundAction.ListenForBinding();
					
					playerInputActions.ListenOptions.OnBindingFound = ( action, binding ) => {
						if (binding == new KeyBindingSource( Key.Escape )) {
							
							action.StopListeningForBinding();
							changeInputPrompt.SetActive(false);
							isActive = true;

							return false;
						}
						
						if(binding.BindingSourceType == bindingSourceType) {
							Logger.Log ("new binding found " + binding.Name + " for action " + action.Name);
							
							menuButton.GetComponent<TextMesh>().text = binding.Name;
							changeInputPrompt.SetActive(false);
							action.StopListeningForBinding();
							
							PlayerPrefs.SetString(GameSettings.INPUT_SAVE_NAME, playerInputActions.Save ());
							PlayerPrefs.Save ();

							isActive = true;
							
							return true;

						} else {
							return false;
						}
					};
				}

				break;

			case MenuButtonType.EXIT:

				PlayerPrefs.SetString(GameSettings.INPUT_SAVE_NAME, playerInputActions.Save ());
				PlayerPrefs.Save ();
				
				SetInactive();
				DispatchMessage("OnHideControlsMenu", null);
			break;

				
			}
	}

	protected override void OnActivated () {
	
		currentIndex = 0;
		currentMenuButton = menuButtons[currentIndex];
		currentMenuButton.OnSelected();
	}

	protected override void OnDeactivated () {
		if(menuButtons[currentIndex]) {
			menuButtons[currentIndex].OnUnSelected();
		}
	}
}
