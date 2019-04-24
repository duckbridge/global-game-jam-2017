using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineFollowComponent : DispatchBehaviour {

	public float moveSpeed = 2f;
	private List<Line> linesToFollow;

	// Use this for initialization
	void Start () {
		linesToFollow = new List<Line>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StopFollowing() {
		iTween.StopByName(this.gameObject, "moveToNextLine");
		linesToFollow.Clear();
		DispatchMessage("OnStoppedFollowing", null);
		DispatchMessage("OnCommandDone", true);
	}

	public void ClearLines() {
		linesToFollow.Clear();
	}

	public void StartFollowing(List<Line> lines) {
		linesToFollow = lines;
		DispatchMessage("OnStartFollowingLine", null);
		GoToNextLine();
	}

	public void GoToNextLine() {
		linesToFollow.RemoveAt(0);
		
		if(linesToFollow.Count > 0) {
			Line lineToGoTo = linesToFollow [0];
			DispatchMessage("MovingToNextLineSegment", lineToGoTo.end);
			MoveToLine(lineToGoTo.end);
		}

		if(linesToFollow.Count <= 0) {
			StopFollowing();
		}
		DispatchMessage("OnLinePassed", null);
	}

	private void MoveToLine(Vector3 targetPosition) {
		iTween.MoveTo(this.gameObject, 
			iTween.Hash("name", "moveToNextLine", "position", targetPosition, "speed", moveSpeed, "oncompletetarget", this.gameObject, "oncomplete", "GoToNextLine", "easetype", "linear")
		);
	}
}
