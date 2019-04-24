using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ToggleUI : CutSceneComponent {

		public bool hide = true;

		public override void OnActivated () {
			if(hide) {
				SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.Hide ());
			} else {
				SceneUtils.FindObjects<UIElement>().ForEach(uiElement => uiElement.Show ());
			}

			DeActivate();
		}
	}
}