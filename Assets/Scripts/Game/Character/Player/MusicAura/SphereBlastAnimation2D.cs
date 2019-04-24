using UnityEngine;
using System.Collections;

public class SphereBlastAnimation2D : SoundBlastAnimation2D {

	public float[] blastRadius;
	private SphereCollider sphereCollider;

	// Use this for initialization
	protected override void OnAwake () {
		base.OnAwake ();
		sphereCollider = GetComponent<SphereCollider>();
	}

	public override void OnFrameEntered (int enteredFrame) {
		if(this.lastFrameOverride != -1) {

			sphereCollider.enabled = true;
			sphereCollider.radius = blastRadius[enteredFrame];

			if(enteredFrame > this.lastFrameOverride) {
				Stop ();
				OnAnimationDone();
			}
		} else {
			sphereCollider.enabled = true;
			sphereCollider.radius = blastRadius[enteredFrame];
		}
	}

	protected override void OnAnimationDone () {
		base.OnAnimationDone ();
		sphereCollider.enabled = false;
		sphereCollider.radius = blastRadius[0];
	}

	public void OnTriggerEnter(Collider coll) {
		musicAura.OnTriggerEnter(coll);
	}
}
