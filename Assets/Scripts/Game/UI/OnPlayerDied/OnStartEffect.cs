using UnityEngine;
using System.Collections;

public class OnStartEffect : DispatchBehaviour {

	public OnStartAnimation onStartAnimation;
	public SoundObject onStartSound;

	private GameObject listeningGameObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowEffect(GameObject listeningGameObject) {
		this.listeningGameObject = listeningGameObject;

		this.AddEventListener (listeningGameObject);

		onStartAnimation.PrepareAndPlay ();
		onStartAnimation.AddEventListener (this.gameObject);
		onStartSound.Play ();
	}

	public void OnAnimationDone() {
		DispatchMessage ("OnStartEffectDone", null);
		this.RemoveEventListener (listeningGameObject);
	}
}
