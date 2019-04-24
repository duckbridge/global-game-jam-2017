using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerRoom : VillageCenterRoom {

	public GameObject[] cutscenesInRoom;

	public override void Start() {
		DisableSpawning ();
	}
	
	public override void SpawnCorrectGroundTile() {

	}
}
