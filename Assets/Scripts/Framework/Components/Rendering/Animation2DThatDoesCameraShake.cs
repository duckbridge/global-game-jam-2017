using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animation2DThatDoesCameraShake : Animation2D {

	public Vector2 shakeScale = new Vector2 (2f, 2f);
	public List<int> framesToShakeOn;


	public override void OnFrameEntered (int enteredFrame) {
		if(framesToShakeOn.Contains(enteredFrame)) {
			SceneUtils.FindObject<CameraShaker> ().ShakeCamera (shakeScale, true);
		}
	}
}
