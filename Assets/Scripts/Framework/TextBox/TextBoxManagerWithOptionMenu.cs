using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBoxManagerWithOptionMenu : TextBoxManager {

	public ChooseOptionMenu chooseOptionMenu;

	void Start() {
		
	}

    public void Initialize() {
        chooseOptionMenu.AddEventListener(this.gameObject);
    }

	public void OnOptionChosen(int itemId) {

        if(chooseOptionMenu.isActive) {

            chooseOptionMenu.RemoveEventListener(this.gameObject);

            chooseOptionMenu.SetInactive();
            chooseOptionMenu.gameObject.SetActive(false);
    
            DispatchMessage("OnItemChosen", itemId);
    
    		Hide ();
        }
	}
		
	protected override void OnHideAnimationDone () {
	}

	protected override void OnTextBoxManagerDone() {

		chooseOptionMenu.gameObject.SetActive(true);
		chooseOptionMenu.SetActive();
	}
}
