using UnityEngine;
using System.Collections;

public class CrossBlastAnimation2D : SoundBlastAnimation2D {

	public float[] blastRadius;

	private TriggerListenerThatGrows triggerListenerThatGrows;

	// Use this for initialization
	protected override void OnAwake () {
		base.OnAwake ();

		triggerListenerThatGrows = GetComponentInChildren<TriggerListenerThatGrows>();
		triggerListenerThatGrows.AddEventListener(this.gameObject);
	}

	public override void OnFrameEntered (int enteredFrame) {

		Logger.Log (this.transform.localRotation);

		if(this.lastFrameOverride != -1) {
			triggerListenerThatGrows.GetComponent<Collider>().enabled = true;
			triggerListenerThatGrows.DoGrow(blastRadius[enteredFrame]);

			if(enteredFrame > this.lastFrameOverride) {
				Stop ();
				OnAnimationDone();
			}
		} else {
			triggerListenerThatGrows.GetComponent<Collider>().enabled = true;
			triggerListenerThatGrows.DoGrow(blastRadius[enteredFrame]);
		}
	}

	protected override void OnAnimationDone () {
		base.OnAnimationDone ();

		triggerListenerThatGrows.GetComponent<Collider>().enabled = false;
		triggerListenerThatGrows.DoGrow(blastRadius[0]);
	}

	public void SetPlayerDirection(Direction playerDirection) {
		switch(playerDirection) {
			case Direction.RIGHT:
				this.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			break;

			case Direction.UP:
				this.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			break;

			case Direction.LEFT:
				this.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			break;

			case Direction.DOWN:
				this.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
			break;

		}
	}
}
