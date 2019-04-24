using UnityEngine;
using System.Collections;

public class GameRoom : Room {	

	public GameRoomMinigame gameRoomMinigame;

	protected Player player;

    public virtual void Start() {
  		DisableSpawning ();
    }

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		base.OnEntered (enemyActivationDelay, ref playerEntered);

		player = playerEntered;
		OnGameRoomEntered (playerEntered);

		gameRoomMinigame.OnRoomEntered();

	}

	public override void OnExitted () {
		base.OnExitted ();
		if(player) {
			OnGameRoomExitted (player);
		}
		gameRoomMinigame.OnRoomExitted();
	}

	protected virtual void OnGameRoomEntered(Player playerEntered) {
		playerEntered.GetMusicManager().StopPlayingMusic();
	}

	protected virtual void OnGameRoomExitted(Player playerExitted) {
		playerExitted.GetMusicManager().PlayMusicByTileType(this.GetTileType(), true);
	}
}
