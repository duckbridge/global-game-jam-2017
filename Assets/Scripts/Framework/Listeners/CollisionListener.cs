using UnityEngine;
using System.Collections;

public class CollisionListener : DispatchBehaviour {

	public void Start() {
	}
	
	public void Update() {
	}
	
	public void OnCollisionEnter(Collision coll) {
		DispatchMessage("OnListenerCollision", coll);
	}
}
