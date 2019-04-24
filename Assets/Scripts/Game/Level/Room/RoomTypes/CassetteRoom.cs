using UnityEngine;
using System.Collections;

public class CassetteRoom : Room {	

	private Player player;

    public virtual void Start() {
		SoundUtils.SetSoundVolumeToSavedValueForGameObject (SoundType.BG, this.gameObject);
		DisableSpawning ();
    }

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		base.OnEntered (enemyActivationDelay, ref playerEntered);
		player = playerEntered;
		playerEntered.GetMusicManager().StopPlayingMusic();
	}

	public override void OnExitted () {
		base.OnExitted ();
		if(player) {
			player.GetMusicManager().PlayMusicByTileType(this.GetTileType(), true);
		}
	}
}
