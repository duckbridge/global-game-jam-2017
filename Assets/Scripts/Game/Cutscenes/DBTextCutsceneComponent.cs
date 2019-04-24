using UnityEngine;
using System.Collections;

public class DBTextCutsceneComponent : TextCutSceneComponent {

	public override void OnActivated() {

		BoomboxCompanion boomboxInGame = SceneUtils.FindObject<BoomboxCompanion>();
		WeaponOnBack boomboxOnBack = SceneUtils.FindObject<WeaponOnBack>();

		if(boomboxInGame) {
			textBoxManager.animationNameOnTalk = "Talking";
			textBoxManager.animationManagerToUseForTalking = boomboxInGame.GetAnimationManager();
		}

		if(boomboxOnBack && boomboxOnBack.GetDirection() == Direction.UP) {
			textBoxManager.animationNameOnTalk = boomboxOnBack.GetTalkAnimationName();
			textBoxManager.animationManagerToUseForTalking = boomboxOnBack.GetAnimationManager();
		}

		textBoxManager.AddEventListener(this.gameObject);
		textBoxManager.ResetShowAndActivate();
	}

}
