using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TogglePlayerAnimationSwitching : CutSceneComponent {
	
		public bool enableAnimationSwitching = true;

		public override void OnActivated () {
			if(enableAnimationSwitching) {
				SceneUtils.FindObject<Player>().GetAnimationManager().EnableSwitchAnimations();
			} else {
				SceneUtils.FindObject<Player>().GetAnimationManager().DisableSwitchAnimations();
			}

			DeActivate ();
		}
	}
}
