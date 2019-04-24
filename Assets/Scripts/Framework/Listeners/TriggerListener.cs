using UnityEngine;
using System.Collections;

public class TriggerListener : DispatchBehaviour {

	public virtual void Start() {
	}
	
	public void Update() {
	}
	
	public virtual void OnTriggerEnter(Collider coll) {
		DispatchMessage("OnListenerTrigger", coll);
	}

	public virtual void OnTriggerStay(Collider coll) {
		DispatchMessage("OnListenerTriggerStay", coll);
	}

	public virtual void OnTriggerExit(Collider coll) {
		DispatchMessage("OnListenerTriggerExit", coll);
	}
}
