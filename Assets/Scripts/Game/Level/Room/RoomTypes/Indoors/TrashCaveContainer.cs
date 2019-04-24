using UnityEngine;
using System.Collections;

public class TrashCaveContainer : IndoorsContainer {

	public CutSceneManager cutsceneManager;
	public GameObject drums;

	public override void OnPlayerEntered (Player player, GameObject gameCamera, Vector3 playerSpawnPositionOnExit) {
		base.OnPlayerEntered (player, gameCamera, playerSpawnPositionOnExit);

		if(SceneUtils.FindObject<MapBuilder>().IsNewGame()) { //tmp
			cutsceneManager.gameObject.SetActive(true);
			if (drums) {
				drums.SetActive (true);
			}
		}
	}
}
