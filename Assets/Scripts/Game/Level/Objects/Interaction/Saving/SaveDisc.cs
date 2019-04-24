using UnityEngine;
using System.Collections;

public class SaveDisc : InteractionObject {

	public Animation2D saveDiscAnimation;

	private bool isBusy = false;

	public override void OnInteract (Player player) {
		base.OnInteract (player);

		if(!isBusy && canInteract) {
			isBusy = true;

			saveDiscAnimation.Play(true, true);

			SceneUtils.FindObject<MapBuilder>().SaveData(SpawnType.NORMAL);

			isBusy = false;
		}
	}
}
