using UnityEngine;
using System.Collections;

public class SlotChooseMenu : Menu {

	public MenuButton cancelButton;

	public override void Update ()
	{
		base.Update ();

		if (this.isActive) {
			if(playerInputActions.back.WasPressed) {
				cancelButton.OnPressed();
			}
		}
	}
	public override void OnMenuButtonPressed(MenuButton menuButton) {
        if(!this.isActive) {
            return;
        }

        if(onPressedSound) {
            onPressedSound.Play();
        }
        
        if(menuButton.menuButtonType == MenuButtonType.EXIT) {
            DispatchMessage("OnQuittingSlotsMenu", null);
        } else {
            DispatchMessage("OnSlotChosen", menuButton.GetComponent<MenuButtonWithId>().id);
        }
    }
}
