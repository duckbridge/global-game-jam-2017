using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SetSavedVolumeForSoundObjects : CutSceneComponent {

		public SoundObject[] soundObjects;

		public override void OnActivated () {
			for(int i = 0; i < soundObjects.Length ; i++) {
				SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, soundObjects[i].gameObject);
			}

			DeActivate();
		}
	}
}
