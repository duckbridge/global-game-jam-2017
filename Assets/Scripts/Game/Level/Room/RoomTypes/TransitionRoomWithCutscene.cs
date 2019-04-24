using UnityEngine;
using System.Collections;

public class TransitionRoomWithCutscene : TransitionRoom {

	public TileType tileTypeMusicUnlockedRequiredForCutscene;
	public CutSceneManager cutsceneManager;

	public override void OnEntered (float enemyActivationDelay, ref Player playerEntered) {
		base.OnEntered (enemyActivationDelay, ref playerEntered);
	
		if(tileTypeMusicUnlockedRequiredForCutscene != TileType.none) {
			PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();

			if(playerEntered.GetAnimationControl().GetCurrentAnimationGroup() == AnimationGroup.NakedDrum && playerSaveComponent.GetUnlockedTileTypeTracks().Contains(tileTypeMusicUnlockedRequiredForCutscene)) {
				cutsceneManager.gameObject.SetActive(true);
			}
		}
	}
}
