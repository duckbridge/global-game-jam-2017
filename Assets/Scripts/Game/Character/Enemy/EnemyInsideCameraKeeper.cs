using UnityEngine;
using System.Collections;

public class EnemyInsideCameraKeeper : MonoBehaviour {

	private GameObject cameraContainer;
	private Camera gameCamera;

	private bool isEnabled = false;

	// Use this for initialization
	void Awake () {
		cameraContainer = SceneUtils.FindObject<CameraBorderManager>().gameObject;
		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		if(isEnabled) {
			Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);
			
			Vector3 positionInCamera = gameCamera.WorldToViewportPoint(this.transform.position);

			
			float clampedPositionX = Mathf.Clamp(this.transform.position.x, cameraContainer.transform.position.x - cameraBounds.extents.x, cameraContainer.transform.position.x + cameraBounds.extents.x);
			float clampedPositionZ = Mathf.Clamp(this.transform.position.z, cameraContainer.transform.position.z - cameraBounds.extents.z, cameraContainer.transform.position.z + cameraBounds.extents.z);
			
			this.transform.position = new Vector3(clampedPositionX, this.transform.position.y, clampedPositionZ);
		}
	}

	public void DoEnable() {
		isEnabled = true;
	}
}
