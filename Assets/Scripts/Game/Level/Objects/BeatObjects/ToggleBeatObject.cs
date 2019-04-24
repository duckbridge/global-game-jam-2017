using UnityEngine;
using System.Collections;

public class ToggleBeatObject : BeatObject {

	public int maxAmountOfStates = 2;

	private int currentStateIndex = 0;

	public override void OnBeatEvent () {

		if(currentStateIndex >= maxAmountOfStates) {
			currentStateIndex = 0;
		}

		OnStateEntered(currentStateIndex);

		currentStateIndex++;
	}

	protected virtual void OnStateEntered(int stateIndex) {
		switch(stateIndex) {
			case 0:
				OnFirstStateEntered();
			break;

			case 1:
				OnSecondStateEntered();
			break;
		}
	}

	protected virtual void OnFirstStateEntered() {
		iTween.RotateTo(this.gameObject, new ITweenBuilder().SetRotation(new Vector3(0f, 90f, 0)).SetTime(.5f).Build());
	}

	protected virtual void OnSecondStateEntered() {
		iTween.RotateTo(this.gameObject, new ITweenBuilder().SetRotation(new Vector3(0f, 0f, 0f)).SetTime(.5f).Build());
	}
}
