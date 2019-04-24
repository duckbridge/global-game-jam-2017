using UnityEngine;
using System.Collections;

public class CustomTriggerListener : TriggerListener {

	public string dispatchMessageName = "OnCustomTrigger";

	public override void OnTriggerEnter(Collider coll) {
		DispatchMessage(dispatchMessageName, coll);
	}

	public override void OnTriggerExit(Collider coll) {
		DispatchMessage(dispatchMessageName + "Exit", coll);
	}
}
