using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class DeActivateGameObjects : CutSceneComponent {

		public GameObject[] gameObjectsToActivate;

		public override void OnActivated () {
			for(int i = 0; i < gameObjectsToActivate.Length ; i++) {
				gameObjectsToActivate[i].SetActive(false);
			}

			DeActivate();
		}
	}
}
