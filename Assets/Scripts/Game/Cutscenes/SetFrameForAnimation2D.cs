using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SetFrameForAnimation2D : CutSceneComponent {

		public bool doScreenShake = true;
		public int frameNumber = 1;

		public float cutsceneTimeout = .5f;
		public Animation2D targetAnimation;
		public SoundObject soundToPlay;

		public override void OnActivated () {

			targetAnimation.SetCurrentFrame (frameNumber);
			if (soundToPlay) {
				soundToPlay.Play (true);
			}

			if (doScreenShake) {
				SceneUtils.FindObject<CameraShaker> ().ShakeCamera(new Vector2(2f, 2f), true);
			}

			Invoke ("DeActivate", cutsceneTimeout);
		}
	}
}
