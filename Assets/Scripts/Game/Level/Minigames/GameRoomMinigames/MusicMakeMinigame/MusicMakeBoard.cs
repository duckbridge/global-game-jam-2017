using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class MusicMakeBoard : DispatchBehaviour {

	public int pointsRequired = 5;
	public float beatSpeed = 20f;

	public List<string> beatResourcesAvailable;

	private List<MusicMakeMusicInput> musicMakeMusicInputs;
	private List<MusicMakeMusicInput> inputsInsideTrigger;

	private PlayerInputActions playerInputActions;
	private int points;
	private Transform beatContainer;

	private TextMesh pointsOutput;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		if(playerInputActions != null) {
			if(playerInputActions.interact.WasPressed) {
				CheckIfButtonWasCorrect(MusicMakeMusicInput.MusicInputType.BlastOne);
			}

			if(playerInputActions.secondAttack.WasPressed) {
				CheckIfButtonWasCorrect(MusicMakeMusicInput.MusicInputType.BlastTwo);
			}

			if(playerInputActions.thirdAttack.WasPressed) {
				CheckIfButtonWasCorrect(MusicMakeMusicInput.MusicInputType.BlastThree);
			}
		}
	}

	private void CheckIfButtonWasCorrect(MusicMakeMusicInput.MusicInputType musicInputType) {
		foreach(MusicMakeMusicInput inputButton in inputsInsideTrigger) {
			if(inputButton.CanBeUsed()) {

				if(inputButton.musicInputType == musicInputType) {
					
					points += inputButton.pointsRewardedOnCorrect;
					UpdatePointDisplay();

					inputButton.PlayOnCorrectSound();
					inputButton.DisableUsage();

					Logger.Log ("correct button pressed!");
					
				} else {
					
					Logger.Log ("wrong button pressed!");
					
				}
			}
		}
	}

	void FixedUpdate() {
	}

	void OnTriggerEnter(Collider coll) {
		MusicMakeMusicInput musicMakeInput = coll.gameObject.GetComponent<MusicMakeMusicInput>();
		if(musicMakeInput) {
			musicMakeMusicInputs.Remove (musicMakeInput);
			
			Destroy (musicMakeInput.gameObject);
			
			if(musicMakeMusicInputs.Count == 0) {
				DispatchMessage("OnMusicBoardDone", null);
			}
		}
	}

	void OnListenerTrigger(Collider coll) {
		MusicMakeMusicInput musicMakeInput = coll.gameObject.GetComponent<MusicMakeMusicInput>();
		if(musicMakeInput) {
			inputsInsideTrigger.Add (musicMakeInput);
		}
	}

	void OnListenerTriggerExit(Collider coll) {
		MusicMakeMusicInput musicMakeInput = coll.gameObject.GetComponent<MusicMakeMusicInput>();
		if(musicMakeInput) {
			inputsInsideTrigger.Remove (musicMakeInput);
		}
	}

	public void StartBoard(PlayerInputActions playerInputActions, TextMesh pointsOutput) {
		points = 0;
		this.pointsOutput = pointsOutput;
		UpdatePointDisplay();

		beatContainer = this.transform.Find("Beats");

		GetComponentInChildren<TriggerListener>().AddEventListener(this.gameObject);

		this.playerInputActions = playerInputActions;

		musicMakeMusicInputs = new List<MusicMakeMusicInput>();
		inputsInsideTrigger = new List<MusicMakeMusicInput>();

		string beatSourceToUse = beatResourcesAvailable[Random.Range (0, beatResourcesAvailable.Count)];

		TextAsset textAsset = Resources.Load("InternalData/MusicMakeMinigame/" + beatSourceToUse) as TextAsset;
		string text = textAsset.text;
		string[] splittedData = text.Split(',');

		foreach(string data in splittedData) {

			string[] splittedSubData = data.Split(':');
			int ms = System.Convert.ToInt32(splittedSubData[0]);
			string beatPrefabName = splittedSubData[1];

			MusicMakeMusicInput musicMakeInput = (MusicMakeMusicInput) GameObject.Instantiate(Resources.Load ("Minigames/MusicMakeMinigame/" + beatPrefabName, typeof(MusicMakeMusicInput)), beatContainer.transform.position, Quaternion.identity);
			musicMakeInput.Initialize(ms, beatSpeed, beatContainer);
		
			musicMakeMusicInputs.Add (musicMakeInput);

		}
	}

	private void UpdatePointDisplay() {
		pointsOutput.text = points + "/" + pointsRequired;
	}

	public void HidePointsDisplay() {
		pointsOutput.text ="";
	}

	public bool HasEnoughScore() {
		return points >= pointsRequired;
	}
}
