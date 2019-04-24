using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TogglePlayerAnimationControl : CutSceneComponent {

		public bool enableAnimationControl;

		public override void OnActivated () {
			if(!enableAnimationControl) {
				SceneUtils.FindObject<Player>().GetComponentInChildren<AnimationControl>().Disable();
			} else {
				SceneUtils.FindObject<Player>().GetComponentInChildren<AnimationControl>().Enable();
			}

			DeActivate();
		}
	}
}
