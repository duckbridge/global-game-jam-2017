using UnityEngine;
using System.Collections;

public class MrFoxAction : DispatchBehaviour {

	public int priority = 0;
	public bool canBeInterrupted = true;
	public MrFoxActionType mrFoxActionType;

	protected bool isActive = false;
	protected GameObject runTarget;
	protected Transform originalParent;

	protected BodyControl mrFoxBodycontrol;
	protected MrFox mrFox;

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


	public void FinishAction(MrFoxActionType newActionType) {
		OnFinished();

		Disable();
		DispatchMessage("SwitchState", newActionType);
	}

	public void SetRunTarget(GameObject flyTarget) {
		this.runTarget = flyTarget;
	}

	public void SetBodyControl(BodyControl bodyControl) {
		this.mrFoxBodycontrol = bodyControl;
	}

	public void SetMrFox(MrFox mrFox) {
		this.mrFox = mrFox;
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
