using UnityEngine;
using System.Collections;

public class MusicHutDoorThatPlaysCutsceneOnExitAndDone : MusicHutDoor {

	public int minigameId = -1;
	public CutSceneManager cutsceneManagerToPlayOnDone;
	
	public override void OnPlayerExitHouse () {
		base.OnPlayerExitHouse  ();
		
		if(SceneUtils.FindObject<PlayerSaveComponent>().HasFinishedMiniGame(minigameId)) {
			cutsceneManagerToPlayOnDone.StartCutScene(true);
		}
	}

	public override void OnCassettePickedUp() {
		if(musicToMove) {
			musicToMove.position = originalMusicPosition;
		}
		disableIfTileTypeTrackUnlocked.gameObject.SetActive(false);
	}
}
