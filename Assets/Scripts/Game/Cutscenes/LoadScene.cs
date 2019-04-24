using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class LoadScene : CutSceneComponent {
		
		public Scene sceneToLoad;
		public LoadingScreenType loadingScreenType;

		public override void OnActivated () {
			PlayerInputHelper.ResetInputHelper ();
			Loader.LoadScene (sceneToLoad, loadingScreenType);
		}
	}
}