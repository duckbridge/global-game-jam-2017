using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class StopSound : CutSceneComponent {

		public float cutsceneTimeout = .5f;
		public SoundObject soundToStop;

		public override void OnActivated () {
			SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, soundToStop.gameObject);
			soundToStop.Stop ();

			Invoke ("DeActivate", cutsceneTimeout);
		}
	}
}