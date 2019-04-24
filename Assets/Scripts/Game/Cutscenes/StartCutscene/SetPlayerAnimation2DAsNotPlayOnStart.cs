using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SetPlayerAnimation2DAsNotPlayOnStart : CutSceneComponent {

		public string animationName;

		public override void OnActivated () {

			AnimationManager2D animationManager = SceneUtils.FindObject<Player>().GetAnimationManager();

			Animation2D animationToRemoveItOn = animationManager.transform.Find("Naked/" + animationName).GetComponent<Animation2D>();
	
			animationToRemoveItOn.playOnStartup = false;
			animationToRemoveItOn.StopAndHide();

			DeActivate();
		}
	}
}