using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ActivateGameObjects : CutSceneComponent {

		public GameObject[] gameObjectsToActivate;

		public override void OnActivated () {
			for(int i = 0; i < gameObjectsToActivate.Length ; i++) {
				gameObjectsToActivate[i].SetActive(true);
				SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, gameObjectsToActivate[i]);
			}

			DeActivate();
		}
	}
}
