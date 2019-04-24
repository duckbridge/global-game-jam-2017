using UnityEngine;
using System.Collections;

public abstract class TimeScaleIndependentUpdate : DispatchBehaviour {

	public float timeRequiredForUpdateTick = .1f;

	protected float previousTimeSinceStartup;
	protected float deltaTime = 0;
	protected float elapsedTime;

	// Use this for initialization
	void Awake () {
		previousTimeSinceStartup = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	public void Update () {
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realtimeSinceStartup - previousTimeSinceStartup;
		previousTimeSinceStartup = realtimeSinceStartup;
		
		if(deltaTime < 0) {
			deltaTime = 0;
		}

		OnUpdate();
	}

	public IEnumerator TimeScaleIndependentWaitForSeconds(float seconds, GameObject listenerObject) {
		float elapsedTime = 0;

		this.AddEventListener (listenerObject);

		while(elapsedTime < seconds) {
			yield return null;
			elapsedTime += deltaTime;
		}
		DoneWaiting ();
	}

	protected virtual void DoneWaiting() {
		DispatchMessage ("OnDoneWaiting", null);
	}	

	public abstract void OnUpdate();

	public override void OnPauseGame() {
	}
	
	public override void OnResumeGame() {
	}

}
