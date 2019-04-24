using UnityEngine;
using System.Collections;

public class DetroitShopContainer : IndoorsContainer {

	public DetroitShopKeeper shopKeeper;

	public override void OnPlayerEntered (Player player, GameObject gameCamera, Vector3 playerSpawnPositionOnExit) {
		base.OnPlayerEntered (player, gameCamera, playerSpawnPositionOnExit);
        if(shopKeeper) {
		    shopKeeper.Activate();
        }

	}

	public override void OnPlayerExitted () {
		base.OnPlayerExitted ();
    
        if(shopKeeper) {
		    shopKeeper.DeActivate();
        }
	}
}
