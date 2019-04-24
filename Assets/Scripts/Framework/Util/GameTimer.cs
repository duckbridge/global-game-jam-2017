using UnityEngine;
using System.Collections;
using System;

public class GameTimer : DispatchBehaviour {

	public float startTimeInms;
	private float originalStartTimeInMs;
	private float currentTimeInms;
	private float changeAmount;
	private float originalChangeAmount;
	
	public bool hasStarted = false;
	public bool displayInMinutes = true;
	public bool countUp = false;

	public float timeOutTime = -1f;
	private int previousTimeInSeconds = -1;
	public TextMesh counterOutput;
	
	void Awake() {
		Initialize();
		DisplayTime();
	}

	public void Initialize() {
		originalStartTimeInMs = startTimeInms;
		this.currentTimeInms = startTimeInms;
		this.changeAmount = (1000/60);
		if(!countUp) { changeAmount *= -1f; }

		this.originalChangeAmount = changeAmount;
	}

	public void StartWithSettings(bool countUp, float timeOutTime) {
		this.countUp = countUp;
		this.timeOutTime = timeOutTime;
		this.hasStarted = true;
		Enable();
	}
	
	void FixedUpdate () {
		if(hasStarted) {
			if(timeOutTime == -1 || 
				(currentTimeInms < timeOutTime && countUp) || 
				currentTimeInms > timeOutTime && !countUp) {
				
				currentTimeInms += changeAmount;
				DisplayTime();
			} else {
				hasStarted = false;
				DispatchMessage("OnGameTimerFinished", this);
			}
		}
	}

	private void DisplayTime() {
		int timeInSeconds = (int)(currentTimeInms / 1000);

		if(timeInSeconds != previousTimeInSeconds) {
			DispatchMessage("OnSecondsTick", timeInSeconds);
		}
		
		previousTimeInSeconds = timeInSeconds;
		if(displayInMinutes) {
			string secondsLeft = (timeInSeconds % 60)+"";
			string minutes = (timeInSeconds / 60)+"";
			
			if((Convert.ToInt32(secondsLeft)) < 10) {
				secondsLeft = "0"+secondsLeft;
			}

			counterOutput.text = minutes + " : " + secondsLeft;
		}	
	}

	public float GetCurrentTimeInMs() {
		return this.currentTimeInms;
	}

	public float GetCurrentTimeInS() {
		return this.currentTimeInms/1000;
	}

	public void SetStartTimeInMS(float newTime) {
		this.currentTimeInms = newTime;
		this.startTimeInms = newTime;
		DisplayTime();
	}

	public override void Disable() {
		if(this.counterOutput) {
			this.counterOutput.GetComponent<Renderer>().enabled = false;
		}
	}

	public override void Enable() {
		if(this.counterOutput) {
			this.counterOutput.GetComponent<Renderer>().enabled = true;
		}
	}

	public override void OnPauseGame() {
		this.changeAmount = 0;
		this.hasStarted = false;
	}

	public override void OnResumeGame() {
		Debug.Log("OnResumeGame");
		this.changeAmount = originalChangeAmount;
		this.hasStarted = true;
	}

	public float GetOriginalStartTimeInMs() {
		return originalStartTimeInMs;
	}

	public float GetOriginalStartTimeInS() {
		return this.originalStartTimeInMs/1000;
	}
}
