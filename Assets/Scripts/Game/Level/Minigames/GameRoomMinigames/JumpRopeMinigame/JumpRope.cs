using UnityEngine;
using System.Collections;

public class JumpRope : DispatchBehaviour {

	public Animation2D jumpRopeUp, jumpRopeDown, jumpRopeDefault;
	private Animation2D currentJumpRopeAnimation;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartJumpRopeAnimationOnMinigameStart() {
		jumpRopeDown.StopAndHide();
		jumpRopeDefault.StopAndHide();
		
		jumpRopeDown.AddEventListener(this.gameObject);
		jumpRopeUp.AddEventListener(this.gameObject);
		
		jumpRopeUp.Play(true);
		currentJumpRopeAnimation = jumpRopeUp;
	}

	public void PauseJumpRopeAnimation() {
		currentJumpRopeAnimation.Pause ();
	}

	public void ResumeJumpRopeAnimation() {
		currentJumpRopeAnimation.Resume ();
	}

	public void StartJumpRopeAnimation() {
		jumpRopeDown.StopAndHide();
		jumpRopeDefault.StopAndHide();

		jumpRopeDown.AddEventListener(this.gameObject);
		jumpRopeUp.AddEventListener(this.gameObject);
		
		jumpRopeDown.Play(true);
		currentJumpRopeAnimation = jumpRopeDown;
	}

	public void StopJumpRopeAnimation() {
		jumpRopeDown.StopAndHide();
		jumpRopeUp.StopAndHide();

		jumpRopeDefault.Show();

	}

	public void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "JumpropeUP") {
			jumpRopeUp.StopAndHide();
			jumpRopeDown.Play(true);
			currentJumpRopeAnimation = jumpRopeDown;
			//reset
			DispatchMessage("OnJumpRopeReset", null);
		}

		if(animation2D.name == "JumpropeDOWN") {
			jumpRopeDown.StopAndHide();
			jumpRopeUp.Play(true);
			currentJumpRopeAnimation = jumpRopeUp;
			DispatchMessage("OnRopeSwingDone", null);
		}
	}
}
