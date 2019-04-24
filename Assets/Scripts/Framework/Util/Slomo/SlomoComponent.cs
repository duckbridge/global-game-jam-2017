using UnityEngine;
using System.Collections;

public class SlomoComponent : MonoBehaviour {

	public float timeout = 1f;
	public float timeScale = .6f;

	public float incrementTime = .5f;
	public float timeScaleIncrement = .1f;

	public SlomoTypes currentSlomoType;

	private float currentTimeScale;

	private bool isInSlomotion;
	private bool slomoSound = true;

	private bool destroyOnFinish = false;

	void Start () {}
	void Update () {}

	public virtual void DoSlomoIndependent(bool slomoSound = true) {
		destroyOnFinish = true;

		this.transform.parent = null;
		DoSlomo(slomoSound);
		
	}	

	public virtual void DoSlomo(bool slomoSound = true) {
		this.slomoSound = slomoSound;

		switch(currentSlomoType) {
			case SlomoTypes.INSTANT:
				
				PrepareSlomo();
				OnSlomo ();
				
				if(timeout > 0) {
					Invoke ("ResetSlomo", timeout);
				}
			break;

			case SlomoTypes.OVERTIME:

				PrepareSlomo();
				OnSlomo ();
			
				Invoke ("Increment", incrementTime);
			break;
		}
	}

	public void StopSlomo() {
		CancelInvoke("Increment");
		CancelInvoke("ResetSlomo");
		Time.timeScale = 1f;
		SoundUtils.SetTimeScale(1f);
	}
	
	private void OnSlomo() {
		if(!isInSlomotion) {
			isInSlomotion = true;

			if(slomoSound) {
				SoundUtils.SetTimeScale(currentTimeScale);
			}

			Time.timeScale = currentTimeScale;
		}
	}

	private void Increment() {
		if(isInSlomotion) {

			currentTimeScale += timeScaleIncrement;

			if(currentTimeScale >= 1f) {
				currentTimeScale = 1f;
			}

			if(slomoSound) {
				SoundUtils.SetTimeScale(currentTimeScale);
			}

			Time.timeScale = currentTimeScale;

			if(currentTimeScale < 1f) {
				Invoke ("Increment", incrementTime);
			} else {
				CancelInvoke("Increment");
				ResetSlomo ();
			}
		}
	}

	private void PrepareSlomo() {
		currentTimeScale = timeScale;
	}

	public void ResetSlomo() {
		
		isInSlomotion = false;
		SoundUtils.ResetTimeScale();
		
		Time.timeScale = 1f;
		currentTimeScale = 1f;

		if(destroyOnFinish) {
			Destroy (this.gameObject);
		}
	}
}
