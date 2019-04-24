using UnityEngine;
using System.Collections;

public class RopePart : MonoBehaviour {

	public Transform lookAtTarget;
	private float originalScaleX = 0f;

	// Use this for initialization
	public void Awake () {
		originalScaleX = this.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(lookAtTarget) {
			Vector3 directionToTarget = MathUtils.CalculateDirection(lookAtTarget.position, this.transform.position);
			this.transform.right = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

			if(originalScaleX != 0f) {
				float distanceBetweenThisAndTarget = Vector3.Distance(lookAtTarget.position, this.transform.position);
				this.transform.localScale = new Vector3(distanceBetweenThisAndTarget * originalScaleX, this.transform.localScale.y, this.transform.localScale.z);
			}
		}
	}
}
