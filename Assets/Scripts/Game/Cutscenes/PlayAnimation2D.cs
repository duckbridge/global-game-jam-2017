using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PlayAnimation2D : CutSceneComponent {

		public bool forceAnimation = false;
		public bool playInReverse = false;

		public bool usesBossAnimationManager = false;
		public bool usePlayerAnimationManager = false;
        public bool usesDBAnimationManager = false;

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

            if(usesDBAnimationManager) {
                animationManager = SceneUtils.FindObject<Player>().GetBoomboxCompanion().GetAnimationManager();            
            }

			if(animationManager) {

				if(!animationManager.IsInitialized()) {
					animationManager.Initialize();
				}

				if(playInReverse) {
					animationManager.PlayAnimationByNameReversed(animationName);
				} else {
 					animationManager.PlayAnimationByName(animationName, true, false, forceAnimation);
				}
			} else {
				animationToPlayWithoutManager.Play(true, playInReverse);
			}

			Invoke ("DeActivate", cutsceneTimeout);
		}
	}
}
