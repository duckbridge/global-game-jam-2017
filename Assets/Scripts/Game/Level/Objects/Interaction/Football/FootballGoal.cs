using UnityEngine;
using System.Collections;

public class FootballGoal : DispatchBehaviour {

	public TextMesh scoreOutput;
	private int score = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		Football football = coll.gameObject.GetComponent<Football>();
		if(football) {
			++score;
			scoreOutput.text = score+"";
			DispatchMessage("OnGoalMade", this);
		}
	}
}
