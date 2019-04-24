using UnityEngine;
using System.Collections;

public class ThrowInArcComponent : MonoBehaviour {

	public float flyTime = 2f;
	public float travelTime = 3f;

	public void Start() {

	}

	public void Update() {

	}

	public void DoThrow(Transform target) {
		
		Vector3 currentUsedVelocity = Vector3.zero;
		
		Vector2 diff = (target.position - this.transform.position);
		currentUsedVelocity.x = diff.x / travelTime;
		currentUsedVelocity.y = (diff.y - 0.5f * Physics.gravity.y * flyTime * flyTime) / flyTime;
		
		this.GetComponent<Rigidbody>().velocity = currentUsedVelocity;
	}

	public void StopThrow() {
		this.GetComponent<Rigidbody>().velocity = new Vector3(this.GetComponent<Rigidbody>().velocity.x, 0f);
	}
}