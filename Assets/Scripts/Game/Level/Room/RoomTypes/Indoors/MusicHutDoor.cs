using UnityEngine;
using System.Collections;

public class MusicHutDoor : HouseDoor {

	public Transform musicToMove;

	public Transform musicPositionOnDoorEnter;
	protected Vector3 originalMusicPosition;

	public DisableIfTileTypeTrackUnlocked disableIfTileTypeTrackUnlocked;

	public override void OnInteract (Player player) {
		if(canInteract) {
			if(musicToMove) {
				originalMusicPosition = musicToMove.position;
				musicToMove.position = musicPositionOnDoorEnter.position;
			}

			base.OnInteract (player);

			MuteMusic ();
		}
	}

	public override void OnPlayerExitHouse () {
		UnMuteMusic ();
	}

	public void MuteMusic() {
		musicToMove.gameObject.SetActive(false);
	}

	public void UnMuteMusic() {
		musicToMove.gameObject.SetActive(true);
	}

	public virtual void OnCassettePickedUp() {
		if(musicToMove) {
			musicToMove.position = originalMusicPosition;
		}
		
		disableIfTileTypeTrackUnlocked.TryDisable(false);
	}

	protected override void DoExtraOnDoorEnterExitAnimation () {

		MusicHut musicHut = loadedHouse.GetComponent<MusicHut>();
		musicHut.cassettePickup.AddEventListener(this.gameObject);
		musicHut.GetComponentInChildren<DanceMinigame>().AddEventListener(this.gameObject);

		base.DoExtraOnDoorEnterExitAnimation ();
	}
}
