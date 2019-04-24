using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class WaitForObjectToArrive : CutSceneComponent {

		public Transform objectToWaitFor;
		public Transform positionThatObjectShouldBeAt;

		public float inPositionRange = .5f;

		public void FixedUpdate() {
			if(isActivated) {
				float distance = Vector3.Distance (
					new Vector3(objectToWaitFor.position.x, 0f, objectToWaitFor.position.z),
					new Vector3(positionThatObjectShouldBeAt.position.x, 0f, positionThatObjectShouldBeAt.position.z));
			
				if(distance < inPositionRange) {

					DeActivate();
				}
			}
		}

		public override void OnActivated () {}
	}
}