using UnityEngine;
using System.Collections;

public class CutsceneManagerThatStartsBasedOnLastVisitedVillage : CutSceneManager {

	public int maxVillageNumber = 1;

	public override void OnListenerTrigger (Collider coll) {

		Player player = coll.gameObject.GetComponent<Player>();
		PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();

		if(player) {
			if(playerSaveComponent && playerSaveComponent.GetLastVisitedVillage() < maxVillageNumber) {
				playerSaveComponent.SetLastVillageVisited(maxVillageNumber);
				StartCutScene(false);
			}

			playerSaveComponent.SetLastVillageVisited(maxVillageNumber);
		}
	}
}
