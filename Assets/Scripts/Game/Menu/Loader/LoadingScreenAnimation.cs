using UnityEngine;
using System.Collections;

public class LoadingScreenAnimation : DispatchBehaviour {

	public Animation animationToPlay;
	public TextMesh progressOutput;

	public void Start() {
	}

	public virtual void OnActivate() {
		animationToPlay.Play ();
	}
	
	public void Awake() {

	}
	
	public void Update() {
	}

	public void OnDone() {
		DispatchMessage("OnAnimationDone", null);
	}

	public void SetProgress(string text, int progress) {
		progressOutput.text = "["+progress+"%] " + text; 	
	}
}
