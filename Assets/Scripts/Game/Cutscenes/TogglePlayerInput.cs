using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TogglePlayerInput : CutSceneComponent {

		public bool enablePlayerInput = true;

		public override void OnActivated () {
			if(SceneUtils.FindObject<PlayerInputComponent>()) {
				SceneUtils.FindObject<PlayerInputComponent>().enabled = enablePlayerInput;
			}
			DeActivate();
		}
	}
}