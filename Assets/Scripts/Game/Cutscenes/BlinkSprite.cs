using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class BlinkSprite : CutSceneComponent {

		public Blink2D blink2D;
		public float cutsceneTimeout = 1f;

		public override void OnActivated () {
			blink2D.DoBlink();
		}

		public void OnDoneBlinking() {
			blink2D.StopBlinking();
			DeActivate();
		}
	}
}
