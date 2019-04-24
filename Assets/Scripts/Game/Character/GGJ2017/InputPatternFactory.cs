using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class InputPatternFactory : MonoBehaviour {

	private PlayerInputActions playerInputActions;

	// Use this for initialization
	void Start () {
		playerInputActions = SceneUtils.FindObject<PlayerInputComponent> ().GetInputActions ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private InputPattern CreateInputPattern(double start, double range, string inputName, string soundName, int framesRequred = 1) {
		return new InputPattern (
			start, 
			range,
			FindActionByName (inputName), 
			this.transform.Find ("Sounds/" + soundName).GetComponent<SoundObject> (),
			framesRequred
		);
	}

	public List<InputPattern> CreateInputPatterns(List<InputPatternInfo> inputPatternsInfo) {
		List<InputPattern> patterns = new List<InputPattern> ();

		foreach (InputPatternInfo info in inputPatternsInfo) {
			double start = (double)(info.startInMS / 1000);
			double range = (double)(info.rangeInMS / 1000);

			patterns.Add (CreateInputPattern (start, range, info.inputName, info.soundName));
		}

		return patterns;
	}

	public List<InputPattern> CopyInputPatterns(List<InputPattern> source, string soundName = "") {
		List<InputPattern> newInputPatterns = new List<InputPattern> ();
		for (int i = 0; i < source.Count; i++) {
			newInputPatterns.Add (
				CreateInputPattern (
					source [i].start, 
					source [i].range, 
					source [i].playerAction ? source [i].playerAction.Name : "none", 
					soundName != "" ? soundName : source[i].GetSoundName()
				)
			);
		}

		return newInputPatterns;
	}

	private PlayerAction FindActionByName(string playerActionName) {
		foreach (PlayerAction playerAction in playerInputActions.Actions) {
			if(playerAction.Name.Equals(playerActionName, System.StringComparison.InvariantCultureIgnoreCase)) {
				return playerAction;
			}
		}
		return null;
	}
}
