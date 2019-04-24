using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour {

	private CharacterControl characterControl;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (8, 8);
		characterControl = GetComponent<CharacterControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayAnimation(string animationName) {
		characterControl.PlayTalkingAnimation ();
	}

	public Transform GetFeet() {
		return this.transform.Find ("Feet");
	}
}
