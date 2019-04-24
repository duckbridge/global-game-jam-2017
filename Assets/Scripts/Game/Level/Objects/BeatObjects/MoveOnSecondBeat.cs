using UnityEngine;
using System.Collections;

public class MoveOnSecondBeat : BeatObject {

	public BodyControl bodyControl;

	public override void OnBeatEvent () {
		if (!bodyControl.CanMove()) {
			bodyControl.ReEnableMoving ();
		} else {
			bodyControl.DisableMoving ();
			bodyControl.StopMoving ();
		}
	}
}
