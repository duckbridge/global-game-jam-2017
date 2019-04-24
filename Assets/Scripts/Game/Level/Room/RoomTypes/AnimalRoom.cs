using UnityEngine;
using System.Collections;

public class AnimalRoom : Room {

	public CritterNames animalInRoomName;

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {

		if (SceneUtils.FindObject<PlayerSaveComponent> ().GetSavedAnimals ().Contains (animalInRoomName.ToString ())) {
			DisableSpawning ();
		} else {
			SpawnItemsAndEnemies ();
			DisableSpawning ();
		}

		base.OnEntered (enemyActivationDelay, ref playerEntered);
	}
}
