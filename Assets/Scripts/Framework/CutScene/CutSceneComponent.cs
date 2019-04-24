using UnityEngine;
using System.Collections;

public abstract class CutSceneComponent : DispatchBehaviour {

	protected bool isActivated = false;

	public abstract void OnActivated();

	public virtual void OnDeActivated() {
		OnFinished();
	}
	
	public virtual void Start() {}
	
	public virtual void Update() {}

	public void Activate() {
		isActivated = true;
		OnActivated();
	}

	public void DeActivate() {
		if(isActivated) {
			isActivated = false;
			OnDeActivated();
		}
	}

	private void OnFinished() {
		DispatchMessage("OnCutSceneComponentDone", this);
	}
}
