using UnityEngine;
using System.Collections;

public class CutsceneManagerThatStartsIfCorrectAnimationGroup : CutSceneManager {

	public AnimationGroup animationGroupRequired;

	public override void OnListenerTrigger (Collider coll) {

		Player player = coll.gameObject.GetComponent<Player>();

		if(player && player.GetAnimationControl().GetCurrentAnimationGroup() == animationGroupRequired) {
			StartCutScene(false);
		}
	}
}
