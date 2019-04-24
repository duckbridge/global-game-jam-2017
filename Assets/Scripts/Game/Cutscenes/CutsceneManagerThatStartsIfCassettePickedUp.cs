using UnityEngine;
using System.Collections;

public class CutsceneManagerThatStartsIfCassettePickedUp : CutSceneManager {

	public CassettePickup cassettePickup;

	public override void Awake () {
		base.Awake ();
		cassettePickup.AddEventListener (this.gameObject);
	}

	public void OnCassettePickedUp() {
		StartCutScene(true);
	}
}
