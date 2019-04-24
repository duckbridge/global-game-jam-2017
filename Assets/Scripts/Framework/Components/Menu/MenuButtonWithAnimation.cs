using UnityEngine;
using System.Collections;

public class MenuButtonWithAnimation : MenuButton {

	public bool stopAnimationOnPressed = true;
	public bool resetAnimationOnSelect = false;
	public string animationName = "SpinAnimation";

	public override void Start () {
		base.Start ();
		if(animationName.Length > 0 && !resetAnimationOnSelect) {
			GetComponent<Animation>().Play (animationName);
			GetComponent<Animation>()[animationName].speed = 0f;
		}
	}

	public override void OnSelected () {
		base.OnSelected ();

		if(animationName.Length > 0 && !resetAnimationOnSelect) {
			GetComponent<Animation>()[animationName].speed = 1f;
		} else {
			GetComponent<Animation>().Play();
		}
	}

	public override void OnUnSelected () {
		base.OnUnSelected ();

		if(animationName.Length > 0 && !resetAnimationOnSelect) {
			GetComponent<Animation>()[animationName].speed = 0f;
		} else {
			GetComponent<Animation>().Stop ();
		}
	}

	public override void OnPressed () {
		if(stopAnimationOnPressed) {
			if(animationName.Length > 0 && !resetAnimationOnSelect) {
				GetComponent<Animation>()[animationName].speed = 0f;
				GetComponent<Animation>()[animationName].time = 0f;
			} else {
				GetComponent<Animation>().Stop ();
			}
		}

		base.OnPressed ();
	}
}
