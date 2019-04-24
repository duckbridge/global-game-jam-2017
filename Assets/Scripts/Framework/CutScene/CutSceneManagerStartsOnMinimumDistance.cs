using UnityEngine;
using System.Collections;

public class CutSceneManagerStartsOnMinimumDistance : CutSceneManager {

    public float maxDistance = 5f;
    public Transform transformPosition;

    private bool enableDistanceCheck = false;
    private Player player;    

    void FixedUpdate() {
		if(enableDistanceCheck && !player.IsInside()) {
			if(Vector2.Distance(
				new Vector2(transformPosition.position.x, transformPosition.position.z), 
				new Vector2(player.transform.position.x, player.transform.position.z)) > maxDistance) {

                enableDistanceCheck = false;
                StartCutScene(false);
            }
        }
    }
	
    public void EnableDistanceCheck(Player player) {
        transformPosition.position = new Vector3(transformPosition.position.x, player.transform.position.y, transformPosition.position.z);
        this.player = player;
        enableDistanceCheck = true;
    }
}
