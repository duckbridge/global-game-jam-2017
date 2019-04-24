using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SetVelocity : CutSceneComponent {

		public bool usesPlayer = true;
		public Rigidbody rigidbodyToSetVelocityOn;
		public Vector3 newRigidBodyVelocity = Vector3.zero;

		public override void OnActivated () {

			if(usesPlayer) {
				rigidbodyToSetVelocityOn = SceneUtils.FindObject<Player>().GetComponent<Rigidbody>();
			}

			rigidbodyToSetVelocityOn.velocity = newRigidBodyVelocity;

			DeActivate();
		}
	}
}
