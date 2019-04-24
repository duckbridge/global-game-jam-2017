using UnityEngine;
using System.Collections;

public class FollowingEye : MonoBehaviour {

	public float maxEyeDistance = 5f;
	public float eyeFollowSpeed = .05f;

	private Transform eye;
	private Transform targetToLookAt;

	private bool isLookingAtTarget = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void FixedUpdate() {

		if(isLookingAtTarget) {

			Vector3 directionToTarget = new Vector3(targetToLookAt.position.x - eye.transform.position.x, 0f, targetToLookAt.position.z - eye.transform.position.z);
			directionToTarget.Normalize();

			Vector3 newPosition = (directionToTarget * eyeFollowSpeed) + eye.transform.localPosition;

			Vector3 distance = eye.transform.localPosition;
			float eyeDistance = distance.magnitude;
			
			Vector3 clampedPosition = Vector3.ClampMagnitude(newPosition, maxEyeDistance);

			eye.transform.localPosition = clampedPosition;
		
		}
	}
	
	public void Initialize(Transform targetToLookAt) {
		eye = this.transform.Find("Eye");
		this.targetToLookAt = targetToLookAt;
		isLookingAtTarget = true;
		
	}

	public void UnInitialize() {
		isLookingAtTarget = false;
	}
}
