using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

	public bool canShakeCamera = true;
	public float cameraShakeTime = .05f;
	public float cameraShakeOffset = .05f;
	private Vector3 shakeScale = Vector3.one;

	private Vector3 savedLocalPosition;

	public void Start() {
        int canShakeCameraSaved = PlayerPrefs.GetInt("TEMP_BBW_CAMERASHAKETOGGLE", 1);
        canShakeCamera = (canShakeCameraSaved == 1 ? true : false);
    }

	public void Update() {}

	public void ShakeCamera(Vector2 shakeScale, bool reset = true) {

		if(!canShakeCamera) {
			return;
		}

		this.shakeScale = shakeScale;

		if(reset) {
			ResetShake();
		}

		CancelInvoke ("ShakeRight");
		CancelInvoke ("ShakeLeft");
		CancelInvoke ("ShakeUp");
		CancelInvoke ("ShakeDown");

		CancelInvoke("ResetShake");

		ShakeLeft();
		Invoke ("ShakeRight", cameraShakeTime);

		Invoke ("ShakeLeft", cameraShakeTime * 2 );
		Invoke ("ShakeRight", cameraShakeTime * 3);

		Invoke ("ShakeUp", cameraShakeTime * 2);
		Invoke ("ShakeDown", cameraShakeTime * 3);
	
		Invoke ("ResetShake", cameraShakeTime * 4);
	}


	private void ShakeLeft() {
		this.transform.localPosition = 
			new Vector3(this.transform.localPosition.x - (cameraShakeOffset * shakeScale.x), this.transform.localPosition.y, this.transform.localPosition.z);
	}
	
	private void ShakeRight() {
		this.transform.localPosition = 
			new Vector3(this.transform.localPosition.x + (cameraShakeOffset * shakeScale.x), this.transform.localPosition.y, this.transform.localPosition.z);
	}

	private void ShakeUp() {
		this.transform.localPosition = 
			new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.y  - (cameraShakeOffset * shakeScale.y));
	}

	private void ShakeDown() {
		this.transform.localPosition = 
			new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.y  + (cameraShakeOffset * shakeScale.y));
	}

	private void ResetShake() {
		this.transform.localPosition = Vector3.zero;
	}
}
