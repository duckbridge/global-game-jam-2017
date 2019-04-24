using UnityEngine;
using System.Collections;

public class PauseCollectionItemScreen : PauseSubMenu {

    public CollectionItemMenu collectionItemMenu;

	public override void Update () {
		
	}

    protected override void OnActivated () {
        collectionItemMenu.UpdateVisibleButtons();
        collectionItemMenu.gameObject.SetActive(true);
        collectionItemMenu.SetActive();
    }

	public override void OnMenuButtonPressed (MenuButton menuButton) {
        
	}

    public override void SetInactive() {

		if (onExittedMenuSound) {
			onExittedMenuSound.Play ();
		}

		if(collectionItemMenu.collectionItemDisplay && 
			collectionItemMenu.IsShowingDisplay()) {

            collectionItemMenu.CloseDisplay();
        } else {

            collectionItemMenu.SetInactive();
            
            collectionItemMenu.SelectFirstButton();       

            collectionItemMenu.gameObject.SetActive(false);

            isActive = false;
            DispatchMessage("HidePauseSubScreen", null);
       }
    }
}
