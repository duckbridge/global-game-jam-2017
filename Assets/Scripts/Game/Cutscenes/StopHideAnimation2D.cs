using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class StopHideAnimation2D : CutSceneComponent {

		public bool usesBossAnimationManager = false;
		public bool usePlayerAnimationManager = false;

		public float cutsceneTimeout = .5f;
		public AnimationManager2D animationManager;
		public Animation2D animationToPlayWithoutManager;
		
		public string animationName;

		public override void OnActivated () {

			if(usePlayerAnimationManager) {
				animationManager = SceneUtils.FindObject<Player>().GetAnimationManager();

			}

			if(usesBossAnimationManager) {
				animationManager = SceneUtils.FindObject<BossEnemy>().GetAnimationManager();
			}

			if(animationManager) {

				if(!animationManager.IsInitialized()) {
					animationManager.Initialize();
				}

				animationManager.StopHideAnimationByName(animationName);
			} else {
				animationToPlayWithoutManager.StopAndHide();
			}

			Invoke ("DeActivate", cutsceneTimeout);
		}
	}
}
