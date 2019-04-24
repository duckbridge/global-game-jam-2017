using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CutscenemanagerStartOnInteract : CutSceneManager {

	private PlayerInputActions playerInputActions;

	private bool hasStarted = false;

	void Awake() {
		PlayerInputHelper.ResetInputHelper ();
		playerInputActions = PlayerInputHelper.LoadData();
	}

	void Update() {
		if (playerInputActions.interact.IsPressed && !hasStarted) {
			hasStarted = true;
			StartCutScene (false);
		}
	}
}
