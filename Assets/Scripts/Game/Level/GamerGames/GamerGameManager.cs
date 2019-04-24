using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GamerGameManager : MonoBehaviour {

	private PlayerInputActions playerInputActions;

	// Use this for initialization
	void Start () {
		PlayerInputHelper.ResetInputHelper ();
		playerInputActions = PlayerInputHelper.LoadData();

	}
	
	// Update is called once per frame
	void Update () {
		if (playerInputActions.pause.IsPressed) {
			Logger.Log ("quitting game!");
			SceneUtils.FindObject<PlayerSaveComponent> ().UpdateSpawnInfo (SpawnType.ATGAMECONSOLE, true);
			Loader.LoadScene (Scene.MainScene, LoadingScreenType.overworld_default);
		}

		if (playerInputActions.interact.IsPressed) {
			Logger.Log ("interacting!");
		}
	}
}
