using UnityEngine;
using System.Collections;

public class JumpGameRoom : GameRoom {
	
	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		gameRoomMinigame.StartMinigame ();
	}

	protected override void OnGameRoomEntered (Player playerEntered) {}
	protected override void OnGameRoomExitted (Player playerExitted) {}
}
