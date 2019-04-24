using UnityEngine;
using System.Collections;

public class BoomboxAction : DispatchBehaviour {

	public int priority = 0;
	public bool canBeInterrupted = true;
	public BoomboxActionType boomboxActionType;

	protected bool isActive = false;
	protected GameObject runTarget;
	protected Transform originalParent;
	
	protected BoomboxCompanion boomboxCompanion;

	// Use this for initialization
	void Start () {
		originalParent = this.transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(isActive) {
			OnUpdate();
		}
	}

	protected virtual void OnUpdate() {}

	public void StartAction() {
		OnStarted();
		isActive = true;
	}

	protected virtual void OnDisabled() {

	}

	protected virtual void OnStarted() {

	}

	protected virtual void OnFinished() {

	}

	public virtual void OnTriggered(Collider collider) {

	}


	public void FinishAction(BoomboxActionType boomboxActionType) {
		OnFinished();

		Disable();
		DispatchMessage("SwitchState", boomboxActionType);
	}

	public void SetRunTarget(GameObject runTarget) {
		this.runTarget = runTarget;
	}
	
	public void SetBoombox(BoomboxCompanion boomboxCompanion) {
		this.boomboxCompanion = boomboxCompanion;
	}

	protected void PlayAnimationByName(string animationName) {
		DispatchMessage("PlayAnimationByName", animationName);
	}

	public bool IsActive() {
		return this.isActive;
	}

	public override void Disable() {
		OnDisabled();
		this.isActive = false;
	}

}
