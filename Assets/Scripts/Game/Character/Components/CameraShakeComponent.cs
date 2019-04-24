using UnityEngine;
using System.Collections;

public class CameraShakeComponent : MonoBehaviour {

	public Vector2 heavyShakeAmount = new Vector2(3f, 3f);
	public Vector2 shakeAmount = new Vector2(2f, 2f);
	private CameraShaker cameraShaker;

	// Use this for initialization
	void Awake () {
		cameraShaker = SceneUtils.FindObject<CameraShaker>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Shake() {
		cameraShaker.ShakeCamera(shakeAmount);
	}

	public void HeavyShake() {
		cameraShaker.ShakeCamera(heavyShakeAmount);
	}
}
