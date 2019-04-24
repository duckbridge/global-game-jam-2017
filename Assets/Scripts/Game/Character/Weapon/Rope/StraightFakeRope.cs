using UnityEngine;
using System.Collections;

public class StraightFakeRope : FakeRope {

	private Transform targetPosition;

	public override void OnSpawned (Transform ropeOrigin) {
		targetPosition = ropeOrigin;
	}

	public void FixedUpdate() {

		if(ropeFrontEnd && targetPosition) {
			this.transform.position = targetPosition.position;

			Vector3 directionToTarget = MathUtils.CalculateDirection(ropeFrontEnd.transform.position, this.transform.position);
			this.transform.right = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

			float distanceBetweenThisAndTarget = Vector3.Distance(ropeFrontEnd.transform.position, this.transform.position);
			this.transform.localScale = new Vector3(distanceBetweenThisAndTarget * 1.7f, this.transform.localScale.y, this.transform.localScale.z);
		}
	}
}
