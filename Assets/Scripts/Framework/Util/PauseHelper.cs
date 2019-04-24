using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseHelper : MonoBehaviour {

	public static void PauseGame() {
		List<DispatchBehaviour> pausableObjects = SceneUtils.FindObjects<DispatchBehaviour>();
		foreach (DispatchBehaviour pausable in pausableObjects) {
			pausable.OnPauseGame();
		}
	}

	public static void ResumeGame() {
		List<DispatchBehaviour> pausableObjects = SceneUtils.FindObjects<DispatchBehaviour>();
		foreach (DispatchBehaviour pausable in pausableObjects) {
			pausable.OnResumeGame();
		}
	}
}
