using UnityEngine;
using System.Collections;

public class BulletBlastAnimation2D : SoundBlastAnimation2D {

   	public float[] blastPositions;
	
    private TriggerListener boxTriggerListener;
    private Vector3 blastDirection = new Vector3(1f, 0f, 0f);

	// Use this for initialization
	protected override void OnAwake () {
		base.OnAwake ();

		boxTriggerListener = GetComponentInChildren<TriggerListener>();
		boxTriggerListener.AddEventListener(this.gameObject);
	}

	public override void OnFrameEntered (int enteredFrame) {
        Logger.Log(blastDirection * blastPositions[enteredFrame]);
		
        if(this.lastFrameOverride != -1) {
    
            boxTriggerListener.GetComponent<Collider>().enabled = true;
            boxTriggerListener.transform.localPosition = blastDirection * blastPositions[enteredFrame];
            
			if(enteredFrame > this.lastFrameOverride) {

				Stop ();
				OnAnimationDone();
			}
		} else {
			boxTriggerListener.GetComponent<Collider>().enabled = true;
			boxTriggerListener.transform.localPosition = blastDirection * blastPositions[enteredFrame];
        }
	}

	protected override void OnAnimationDone () {
		base.OnAnimationDone ();
		boxTriggerListener.GetComponent<Collider>().enabled = false;
        boxTriggerListener.transform.localPosition = Vector3.zero;
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
