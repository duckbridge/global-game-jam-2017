using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutScene : DispatchBehaviour {

	public List<CutSceneComponent> cutSceneComponents;
	protected int currentCutSceneComponentIndex = 0;

	public virtual void Awake() {
		cutSceneComponents.ForEach(cutSceneComponent => cutSceneComponent.AddEventListener(this.gameObject));
	}

	public virtual void Start() {
	}
	
	public virtual void Update() {
	}

	public virtual void OnActivate() {
		if(currentCutSceneComponentIndex > 0)
			cutSceneComponents[currentCutSceneComponentIndex - 1].DeActivate();

		cutSceneComponents[currentCutSceneComponentIndex].Activate();
	}

	public virtual void OnDeActivate() {
		//do something?
	}

	public void ResetIndex() {
		currentCutSceneComponentIndex = 0;
	}

	public virtual void OnCutSceneComponentDone(CutSceneComponent cutSceneComponent) {
		currentCutSceneComponentIndex++;

		if(currentCutSceneComponentIndex == cutSceneComponents.Count) {
			DispatchMessage("OnCutSceneDone", this);
		} else {
			OnActivate();
		}
	}
}
