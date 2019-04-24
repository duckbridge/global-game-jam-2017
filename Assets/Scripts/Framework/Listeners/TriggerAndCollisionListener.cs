using UnityEngine;
using System.Collections;

public class TriggerAndCollisionListener : DispatchBehaviour {

	public void OnCollisionEnter(Collision coll) {
		DispatchMessage("OnListenerCollision", coll);
	}

	public void OnTriggerEnter(Collider coll) {
		DispatchMessage("OnListenerTrigger", coll);
	}
}
