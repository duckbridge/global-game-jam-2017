using UnityEngine;
using System.Collections;

public class ITweenEffectsComponent : DispatchBehaviour {

	private GameObject currentGO;
	private Hashtable currentItweenParams;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoWiggle(Hashtable iTweenParams, GameObject targetGO) {
		currentItweenParams = iTweenParams;
		currentGO = targetGO;

		iTween.PunchRotation(targetGO, iTweenParams);

		if(iTweenParams.ContainsKey("time")) {
			Invoke("WiggleAgain", (float) iTweenParams["time"]);
			Invoke ("OnWiggleDone", ((float) iTweenParams["time"]) * 2);
		}
	}

	private void WiggleAgain() {

		Vector3 currentRotationAmount = (Vector3) currentItweenParams["amount"];
		currentRotationAmount *= -1f;
		currentItweenParams["amount"] = currentRotationAmount;

		iTween.PunchRotation(currentGO, currentItweenParams);
	}
	
	public void StopWiggle(GameObject targetGO) {
		CancelInvoke("WiggleAgain");
		iTween.StopByName(targetGO, "WiggleWiggle");
	}

	private void OnWiggleDone() {
		DispatchMessage("OnWigglingDone", null);
	}
}
