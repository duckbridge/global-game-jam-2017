using UnityEngine;
using System.Collections;

public class MoveInputShower : MonoBehaviour {

	private Animation2D leftMove, rightMove, upMove, downMove;
	private bool isEnabled = true;

	// Use this for initialization
	void Start () {
	
		leftMove = this.transform.Find("LEFT").GetComponent<Animation2D>();
		rightMove = this.transform.Find("RIGHT").GetComponent<Animation2D>();
		upMove = this.transform.Find("UP").GetComponent<Animation2D>();
		downMove = this.transform.Find("DOWN").GetComponent<Animation2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(isEnabled) {
			
			leftMove.SetCurrentFrame(0);
			rightMove.SetCurrentFrame(0);
			upMove.SetCurrentFrame(0);
			downMove.SetCurrentFrame(0);

			/*
			if(Input.GetAxis("MoveHorizontal") > 0) {
				rightMove.SetCurrentFrame(1);
			}

			if(Input.GetAxis("MoveHorizontal") < 0) {
				leftMove.SetCurrentFrame(1);
			}

			if(Input.GetAxis("MoveVertical") > 0) {
				upMove.SetCurrentFrame(1);
			}

			if(Input.GetAxis("MoveVertical") < 0) {
				downMove.SetCurrentFrame(1);
			}
			*/
		}

	}

	public void Disable() {
		isEnabled = false;
	}

	public void Enable() {
		isEnabled = true;
	}
}
