using UnityEngine;
using System.Collections;

public class FishRoom : Room {	

	private Player player;
	private bool hasSpawnedFish = false;

    public virtual void Start() {
		DisableSpawning ();
    }

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {

		if (!hasSpawnedFish) {
			SpawnFish ();
		}
        
        base.OnEntered (enemyActivationDelay, ref playerEntered);
        
		player = playerEntered;
        player.SetInTown(true);
	}

    protected void SpawnFish() {
		hasSpawnedFish = true;

        if(GetComponentsInChildren<FishCollider>().Length > 0) {
            foreach(FishCollider fishCollider in GetComponentsInChildren<FishCollider>()) {
                fishCollider.SpawnFish();
            }
        }
    }

	public override void OnExitted () {
		base.OnExitted ();
		if(player) {
		    player.SetInTown(false);
		}
	}
}
