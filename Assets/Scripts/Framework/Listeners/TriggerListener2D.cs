using UnityEngine;
using System.Collections;

public class TriggerListener2D : DispatchBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D coll) {
		DispatchMessage("OnListenerTrigger", coll);
	}
}
