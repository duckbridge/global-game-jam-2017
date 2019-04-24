using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Cutscenes {
	public class LoadSceneGGJ : CutSceneComponent {
		
		public string sceneToLoad;

		public override void OnActivated () {
			PlayerPrefs.DeleteKey ("BGMUSIC_TIME");
			PlayerPrefs.Save ();
			SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
		}
	}
}