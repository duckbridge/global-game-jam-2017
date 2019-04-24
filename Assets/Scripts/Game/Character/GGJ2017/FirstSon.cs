using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FirstSon : MonoBehaviour {

	private enum state { cooking, serving, idle }
	private state currentState = state.cooking;

	private AnimationManager2D animationManager;
	private AnimalWithInputPattern currentCustomer;

	private PlayerInputActions playerInputActions;
	private SoundObject pukeSound;

	// Use this for initialization
	void Start () {
		animationManager = GetComponentInChildren<AnimationManager2D> ();
		animationManager.AddEventListenerTo ("Serve", this.gameObject);
		pukeSound = this.transform.Find ("Sounds/PukeSound").GetComponent<SoundObject> ();
		playerInputActions = SceneUtils.FindObject<MainPlayer> ().GetComponent<PlayerInputComponent> ().GetInputActions ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider coll) {
		MainPlayer mainPlayer = coll.gameObject.GetComponent<MainPlayer> ();
		if (mainPlayer) {
			if (playerInputActions.interact.IsPressed) {
				Logger.Log ("IM SCARED DAD, STOP SHOUTING!");
			}
		}
	}

	public void DoSpit(AnimalWithInputPattern customer) {
		this.currentCustomer = customer;

		currentState = state.serving;
		Invoke ("ServeDelayed", .5f);
	}

	private void ServeDelayed() {
		animationManager.PlayAnimationByName ("Serve", true);
		pukeSound.Play (true);
		Invoke ("SpawnFood", 1f);
	}

	private void SpawnFood() {
		this.currentCustomer.SpawnFood();
		animationManager.PlayAnimationByName ("Cook", true);
	}
}
