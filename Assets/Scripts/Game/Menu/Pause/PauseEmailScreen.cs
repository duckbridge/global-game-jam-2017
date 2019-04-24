using UnityEngine;
using System.Collections;

public class PauseEmailScreen : PauseSubMenu {

    public EmailsMenu emailsMenu;

	public override void Update () {
		
	}

    protected override void OnActivated () {
        emailsMenu.gameObject.SetActive(true);
        emailsMenu.Initialize();
        emailsMenu.SetActive();
    }

	public override void OnMenuButtonPressed (MenuButton menuButton) {
        
	}

    public override void SetInactive() {

		if (onExittedMenuSound) {
			onExittedMenuSound.Play ();
		}

        if(emailsMenu.IsShowingEmailDisplay()) {
            emailsMenu.CloseEmailDisplay();
        } else {

            emailsMenu.SetInactive();
            
            foreach(MenuButton menuButton in emailsMenu.menuButtons) {
                menuButton.OnUnSelected();
            }
            emailsMenu.SetCurrentIndex(0);           

            emailsMenu.gameObject.SetActive(false);

            isActive = false;
            DispatchMessage("HidePauseSubScreen", null);
        }
    }
}
