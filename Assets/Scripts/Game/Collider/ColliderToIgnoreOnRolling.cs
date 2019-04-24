using UnityEngine;
using System.Collections;

public class ColliderToIgnoreOnRolling : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IgnoreCollisionWith(Player player) {
		PhysicsUtils.IgnoreCollisionBetween(this.GetComponent<Collider>(), player.GetComponent<Collider>());
	}

	public void RestoreCollisionWith(Player player) {
		PhysicsUtils.RestoreCollisionBetween(this.GetComponent<Collider>(), player.GetComponent<Collider>());
	}
}
