using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextCutSceneComponent : CutSceneComponent {

    public bool waitsUntillHidingAnimationDone = true;

	public TextBoxManager textBoxManager;

	public override void OnActivated() {
		textBoxManager.AddEventListener(this.gameObject);
		textBoxManager.ResetShowAndActivate();
	}

	private void OnTextBoxDoneAndHidden() {
        if(waitsUntillHidingAnimationDone) {
		    DeActivate();
        }
	}

    public void OnTextDone() {
        if(!waitsUntillHidingAnimationDone) {
            DeActivate();
        }
    }
}
