using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSceneWithParallelPlayingComponents : CutScene {

	public override void OnActivate() {
		foreach(CutSceneComponent cutSceneComponent in cutSceneComponents) {
			cutSceneComponent.Activate();
		}
	}

	public override void OnDeActivate() {
		//do something?
	}

	public override void OnCutSceneComponentDone(CutSceneComponent cutSceneComponent) {
		currentCutSceneComponentIndex++;

		if(currentCutSceneComponentIndex == cutSceneComponents.Count) {
			DispatchMessage("OnCutSceneDone", this);
		}
	}
}
