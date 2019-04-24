using UnityEngine;
using System.Collections;

public class PlatformerAIAction : DispatchBehaviour {

	public string actionName = "NONE";
	protected bool isActive = false;

	public virtual void Start () {}
	
	public void Update () {
		if(isActive) {
			OnRegularUpdate();
		}
	}

	public virtual void FixedUpdate() {
		if(isActive) {
			OnUpdate();
		}
	}

	protected virtual void OnUpdate() {
		
	}

	protected virtual void OnRegularUpdate() {
	
	}

	protected virtual void OnActionStarted() {
		
	}
	
	protected virtual void OnActionFinished() {
		
	}
	
	public virtual void OnTriggered(Collider coll) {
		
	}
	
	public virtual void OnCollided(Collision coll) {
		
	}

	public virtual void FinishAction() {
		OnActionFinished();
		this.isActive = false;
	}

	public virtual void StartAction() {
		isActive = true;

		OnActionStarted();
	}

	public virtual void OnSecondStageEntered() {}

	protected void DeActivate(string nextActionName) {
		if(isActive) {
			DispatchMessage("OnActionDone", nextActionName);
		}
	}

	public string GetActionName() {
		return this.actionName;
	}
}
