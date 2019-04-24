using UnityEngine;
using System.Collections;

public class MusicHutExitDoor : Door {

	public override void OnInteract (Player player) {
		player.SetInDanceMinigame (false);
		player.GetComponent<PlayerDanceComponent> ().ResetBeatListener ();

		base.OnInteract (player); 
	}
}
