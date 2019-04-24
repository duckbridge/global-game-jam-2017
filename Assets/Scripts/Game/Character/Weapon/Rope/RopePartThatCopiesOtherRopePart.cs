using UnityEngine;
using System.Collections;

public class RopePartThatCopiesOtherRopePart : MonoBehaviour {

	public Transform lookAtTarget;
	public RopePart ropePartToCopy;

	private float originalScaleX = 0f;

	// Use this for initialization
	void Awake () {
		originalScaleX = this.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		this.transform.right = ropePartToCopy.transform.right;

		if(originalScaleX != 0f && lookAtTarget) {
			float distanceBetweenThisAndTarget = Vector3.Distance(lookAtTarget.position, this.transform.position);
			this.transform.localScale = new Vector3(distanceBetweenThisAndTarget * originalScaleX, this.transform.localScale.y, this.transform.localScale.z);
		}
	}
}
