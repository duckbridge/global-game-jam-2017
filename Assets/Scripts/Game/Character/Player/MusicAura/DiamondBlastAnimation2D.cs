using UnityEngine;
using System.Collections;

public class DiamondBlastAnimation2D : SoundBlastAnimation2D {

	public float[] blastRadius;
	private TriggerListener boxTriggerListener;

	// Use this for initialization
	protected override void OnAwake () {
		base.OnAwake ();

		boxTriggerListener = GetComponentInChildren<TriggerListener>();
		boxTriggerListener.AddEventListener(this.gameObject);
	}

	public override void OnFrameEntered (int enteredFrame) {
		if(this.lastFrameOverride != -1) {

			boxTriggerListener.GetComponent<Collider>().enabled = true;
			boxTriggerListener.GetComponent<BoxCollider>().size = new Vector3(blastRadius[enteredFrame], boxTriggerListener.GetComponent<BoxCollider>().size.y, blastRadius[enteredFrame]);

			if(enteredFrame > this.lastFrameOverride) {

				Stop ();
				OnAnimationDone();
			}
		} else {
			boxTriggerListener.GetComponent<Collider>().enabled = true;
			boxTriggerListener.GetComponent<BoxCollider>().size = new Vector3(blastRadius[enteredFrame], boxTriggerListener.GetComponent<BoxCollider>().size.y, blastRadius[enteredFrame]);
		}
	}

	protected override void OnAnimationDone () {
		base.OnAnimationDone ();
		boxTriggerListener.GetComponent<Collider>().enabled = false;
		boxTriggerListener.GetComponent<BoxCollider>().size = new Vector3(blastRadius[0], boxTriggerListener.GetComponent<BoxCollider>().size.y, blastRadius[0]);
	}
}
