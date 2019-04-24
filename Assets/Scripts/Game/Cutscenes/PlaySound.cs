using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PlaySound : CutSceneComponent {

		public float cutsceneTimeout = .5f;
		public SoundObject soundToPlay;

		public override void OnActivated () {
			//SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, soundToPlay.gameObject);
			if(soundToPlay.soundType == SoundType.FX) {
				soundToPlay.Play(true);
			} else {
				soundToPlay.PlayScheduled(AudioSettings.dspTime);
			}

			Invoke ("DeActivate", cutsceneTimeout);
		}
	}
}