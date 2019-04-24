using UnityEngine;
using System.Collections;

public class SwordThrowInputShower : MonoBehaviour {

	private Animation2D leftThrow, rightThrow, upThrow, downThrow;
	private bool isEnabled = true;

	// Use this for initialization
	void Start () {
	
		leftThrow = this.transform.Find("LEFT").GetComponent<Animation2D>();
		rightThrow = this.transform.Find("RIGHT").GetComponent<Animation2D>();
		upThrow = this.transform.Find("UP").GetComponent<Animation2D>();
		downThrow = this.transform.Find("DOWN").GetComponent<Animation2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(isEnabled) {

			ResetSwordDirectionInput();

			/*
			if(Input.GetAxis("LookHorizontal") > 0) {
				rightThrow.SetCurrentFrame(1);
			}

			if(Input.GetAxis("LookHorizontal") < 0) {
				leftThrow.SetCurrentFrame(1);
			}

			if(Input.GetAxis("LookVertical") > 0) {
				upThrow.SetCurrentFrame(1);
			} 

			if(Input.GetAxis("LookVertical") < 0) {
				downThrow.SetCurrentFrame(1);
			}
			*/
		}
	}

	private void ResetSwordDirectionInput() {
		leftThrow.SetCurrentFrame(0);
		rightThrow.SetCurrentFrame(0);
		upThrow.SetCurrentFrame(0);
		downThrow.SetCurrentFrame(0);
	}

	public void Disable() {
		isEnabled = false;
	}

	public void Enable() {
		isEnabled = true;
	}
}
