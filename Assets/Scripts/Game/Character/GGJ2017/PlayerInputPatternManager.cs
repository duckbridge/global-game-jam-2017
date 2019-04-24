using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInputPatternManager : DispatchBehaviour {

	public SoundObject interactSound;
	private PlayerInputActions playerInputActions;

	private int frameCounter = 0;

	private List<InputPattern> inputPatterns = null;
	private InputPattern currentInputPattern;

	private bool isStarted = false;
	private PlayerAction lastAction;

	private float previousTimeSinceStartup;
	private float deltaTime = 0;
	private float elapsedTime;
	private MainPlayer mainPlayer;

	private ParticleSystem cakParticles;

	void Awake() {
		previousTimeSinceStartup = Time.realtimeSinceStartup;
		mainPlayer = GetComponent<MainPlayer> ();

		cakParticles = this.transform.Find ("CakParticles").GetComponent<ParticleSystem> ();
	}

	// Use this for initialization
	void Start () {
		playerInputActions = GetComponent<PlayerInputComponent> ().GetInputActions ();
	}

	// Update is called once per frame
	void Update () {

		float realtimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realtimeSinceStartup - previousTimeSinceStartup;
		previousTimeSinceStartup = realtimeSinceStartup;

		elapsedTime += deltaTime;

		if (playerInputActions.pause.WasReleased) {
			SceneUtils.FindObject<PenguGameManager> ().LoadCurrentScene ();
		}

		if (HasPressedAnyPatternButtonOnce()) {
			interactSound.Play (true);
			mainPlayer.PlayAnimation ("Talking");
			cakParticles.Emit (1);
		}

		if (isStarted) {
			
			frameCounter++;

			currentInputPattern = InputPatternUtils.FindUnfinishedInputPatternByElapsedTime (inputPatterns, (double)elapsedTime, false);

			if (currentInputPattern == null) {
				//als je een van de knoppend drukt, dan faal je
				if (HasPressedAnyPatternButtonOnce()) {
					Logger.Log ("no pattern found.. and you pressed something..");
					DispatchMessage ("OnInputPatternInCorrect", this);
					StopSequence (false);
					return;
				}
			}

			CheckIfCorrectButtonIsPressed (true);

			if (inputPatterns != null) {
				InputPattern lastPattern = inputPatterns [inputPatterns.Count - 1];
				if (elapsedTime >= (lastPattern.start + lastPattern.range)) {
					StopSequence (true);
				}
			}
		} else {
			if (HasPressedAnyPatternButtonOnce()) {
				if (inputPatterns != null) {
					StartPattern ();
					currentInputPattern = InputPatternUtils.FindUnfinishedInputPatternByElapsedTime (inputPatterns, (double)elapsedTime, false);
					CheckIfCorrectButtonIsPressed (false);
				}
			}
		}
	}

	private void CheckIfCorrectButtonIsPressed(bool forceButtonUp) {
		if (currentInputPattern != null && currentInputPattern.playerAction != null) {
			if (currentInputPattern.playerAction.IsPressed && (!forceButtonUp || currentInputPattern.playerAction.LastValue == 0)) {
				Logger.Log ("good: " + (double)elapsedTime);

				currentInputPattern.UpdateFramesHeldBy (1);
				//currentInputPattern.SetFinished ();

				int amountOfCorrectOnes = inputPatterns.FindAll (ip => ip.IsFinished ()).Count;
				if (amountOfCorrectOnes == inputPatterns.Count) {
					Logger.Log ("correct! you're done!");
					DispatchMessage ("OnInputPatternCorrect", this);
					StopSequence (false);
					return;
				}
			}
		}
	}

	private void StartPattern() {
		frameCounter = 0;
		elapsedTime = 0;
		isStarted = true;
	}

	public void SetInputPatterns(List<InputPattern> inputPatterns) {
		ResetSequence ();
		this.inputPatterns = inputPatterns;
	}

	private void StopSequence(bool alsoInCorrect) {

		if (inputPatterns == null) {
			return;
		}

		int amountOfCorrectOnes = inputPatterns.FindAll (ip => ip.IsFinished ()).Count;
		if (amountOfCorrectOnes != inputPatterns.Count) {
			Logger.Log ("RESULT:");
			foreach (InputPattern inputPattern in inputPatterns) {
				Logger.Log ("you " + (inputPattern.IsFinished() ? "passed" : "failed") + " the pattern with start: " + inputPattern.start);
			}
		}

		if (alsoInCorrect) {
			DispatchMessage ("OnInputPatternInCorrect", this);
		}
		ResetSequence ();
	}

	private void ResetSequence() {
		if (inputPatterns != null) {
			inputPatterns.ForEach (ip => ip.ResetPattern ());
		}
		frameCounter = 0;
		elapsedTime = 0;
		isStarted = false;
	}

	private bool HasPressedAnyPatternButtonOnce() {
		return playerInputActions.interact.IsPressed && playerInputActions.interact.LastValue == 0; //TODO: Add more buttons!
	}

	private bool HasPressedAnyPatternButton() {
		return playerInputActions.interact.IsPressed; //TODO: Add more buttons!
	}
}
