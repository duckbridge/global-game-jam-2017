using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingPengu : AnimalWithInputPattern {

	public int amountCorrectInARowRequired = 3;

	private PatternInfoList[] allInputPatterns;
	private int currentIndex = 0;

	private int amountCorrectInARow = 0;

	// Use this for initialization
	public override void Awake () {
		allInputPatterns = this.transform.Find ("InputPatterns").GetComponentsInChildren<PatternInfoList> ();
		currentIndex = Random.Range (0, allInputPatterns.Length);
		inputPatternsInfo = allInputPatterns [currentIndex].inputPatternsInfo;
		base.Awake ();
	}

	public override void OnInputPatternCorrect(PlayerInputPatternManager playerInputPatternManager) {
		amountCorrectInARow++;
		Logger.Log (amountCorrectInARow);

		if (amountCorrectInARow >= amountCorrectInARowRequired) {
			OnDoneInQueue ();
			Logger.Log ("KING HAPPY");
			this.isHappy = true;
			//do eat animation  
			DispatchMessage ("OnFirstCustomerHelped", this);
		} else {
			this.isHappy = true;
			SceneUtils.FindObject<QueueManager>().UpdateCurrency (this);
			currentIndex = Random.Range (0, allInputPatterns.Length);

			MainPlayer mainPlayer = SceneUtils.FindObject<MainPlayer> ();
			mainPlayer.GetComponent<PlayerInputPatternManager> ().RemoveEventListener (this.gameObject);
			mainPlayer.GetComponent<PlayerInputPatternManager> ().SetInputPatterns (null);

			StopPattern ();

			SceneUtils.FindObject<FirstSon>().DoSpit(this);
		}
	}

	public override void OnInputPatternInCorrect(PlayerInputPatternManager playerInputPatternManager) {
		amountCorrectInARow = 0;

		if (amountOfAttempts + 1 < attemptsAllowed) {
			this.isHappy = false;
			SceneUtils.FindObject<QueueManager> ().UpdateCurrency (this);
		} else {
			base.OnInputPatternInCorrect (playerInputPatternManager);
		}
	}

	protected override void OnDoneEating ()
	{
		if (amountCorrectInARow >= amountCorrectInARowRequired) {
			base.OnDoneEating ();
		} else {
			inputPatternsInfo = allInputPatterns [currentIndex].inputPatternsInfo;

			CreatePatterns ();

			MainPlayer mainPlayer = SceneUtils.FindObject<MainPlayer> ();
			mainPlayer.GetComponent<PlayerInputPatternManager> ().AddEventListener (this.gameObject);
			mainPlayer.GetComponent<PlayerInputPatternManager> ().SetInputPatterns (playerPatterns);

			animationManager.EnableSwitchAnimations ();
			animationManager.PlayAnimationByName ("Happy", true);

			Logger.Log ("restarting pattern");
			Invoke ("StartPattern", 1f);
		}
	}
}
