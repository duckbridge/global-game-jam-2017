using UnityEngine;
using System.Collections;

public class AnimationListener : DispatchBehaviour {
	
	public void Start() {
	}
	
	public void Update() {
	}

	public void OnDone() {
		DispatchMessage("OnAnimationDone", null);
	}

	public void OnPause() {
		DispatchMessage("OnAnimationPaused", null);
	}
}
