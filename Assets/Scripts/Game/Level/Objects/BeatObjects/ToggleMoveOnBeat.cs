using UnityEngine;
using System.Collections;

public class ToggleMoveOnBeat : BeatObject {

	public bool enableMovingOnBeat = false;
	public float resetMovingTime = .2f;
	public BodyControl bodyControl;

	public void Awake() {
		if (enableMovingOnBeat) {
			bodyControl.DisableMoving ();
		} else {
			bodyControl.ReEnableMoving ();
		}
	}

	public override void OnBeatEvent () {
		if (enableMovingOnBeat) {
			bodyControl.ReEnableMoving ();
		} else {
			bodyControl.DisableMoving ();
			bodyControl.StopMoving ();
		}
		Invoke ("ResetMoving", resetMovingTime);
	}

	private void ResetMoving() {
		if (enableMovingOnBeat) {
			bodyControl.DisableMoving ();
			bodyControl.StopMoving ();
		} else {
			bodyControl.ReEnableMoving ();
		}
	}
}
