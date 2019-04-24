using UnityEngine;
using System.Collections;

public class MusicHut : IndoorsContainer {

	public DanceMinigame danceMinigame;
	public TileType cassetteTileType;
	public CassettePickup cassettePickup;

	public override void OnPlayerEntered (Player player, GameObject gameCamera, Vector3 playerSpawnPositionOnExit) {
		base.OnPlayerEntered (player, gameCamera, playerSpawnPositionOnExit);

		player.GetComponent<PlayerDanceComponent> ().AddEventListener (danceMinigame.gameObject);

		player.SetInDanceMinigame (true);

		bool playerHasCassette = SceneUtils.FindObject<PlayerSaveComponent>().GetUnlockedTileTypeTracks().Contains(cassetteTileType);

		if (!playerHasCassette) {
			cassettePickup.tileType = cassetteTileType;
			danceMinigame.ResetPoints ();
			danceMinigame.OnPlayerEnteredAndDoesntHaveCassette (player);
		} else {
			if (cassettePickup) {
				cassettePickup.gameObject.SetActive (false);
			}
			danceMinigame.OnPlayerEnteredAndHasCassette ();
		}
	}
}
