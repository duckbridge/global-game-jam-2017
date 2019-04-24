using UnityEngine;
using System.Collections;
using InControl;

public class VibrationComponent : MonoBehaviour {

	public float vibrationAmount = .5f;
	public float vibrationTimeout = .5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Vibrate() {
		CancelInvoke("StopVibrate");

		var inputDevice = InputManager.ActiveDevice;
		inputDevice.Vibrate(vibrationAmount);

		Invoke ("StopVibrate", vibrationTimeout);
	}
	
	private void StopVibrate() {
		var inputDevice = InputManager.ActiveDevice;
		inputDevice.StopVibration();
	}
}
