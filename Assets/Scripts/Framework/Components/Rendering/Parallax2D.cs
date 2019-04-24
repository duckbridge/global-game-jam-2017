using UnityEngine;
using System.Collections;

public class Parallax2D : MonoBehaviour {

	public bool onlyMoveXOnRespawn = false;

	public Transform targetTransform;
	public float parallaxSpeed;

	public Transform spawnPosition;

	private Vector3 startPosition;
	private Vector3 targetPosition;
	private float originalParallaxSpeed;

	private bool isInitialized = false;

	void Awake () {	
		Initialize();
	}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			FollowCamera2D followCamera2D = SceneUtils.FindObject<FollowCamera2D>();
			if(followCamera2D) {
				followCamera2D.AddEventListener(this.gameObject);
			}
			
			targetPosition = targetTransform.localPosition;
			startPosition = this.spawnPosition.transform.localPosition;
		}
	}
	
	void Update () {
	}

	public void OnCameraMoved(Vector3 moveDifference) {

		if(this.enabled) {
			float newXPosition = this.transform.localPosition.x;
			
			newXPosition += parallaxSpeed * -moveDifference.x;
			
			this.transform.localPosition = new Vector3(newXPosition, this.transform.localPosition.y, this.transform.localPosition.z);
			
			if((Mathf.Abs(targetPosition.x - this.transform.localPosition.x) < parallaxSpeed * 2f) && moveDifference.x > 0) {
				if(onlyMoveXOnRespawn) {
					this.transform.localPosition = TransformUtils.ModifyX(this.transform, startPosition.x, true);
				} else {
					this.transform.localPosition = startPosition;
				}
			}
			
			if((Mathf.Abs(startPosition.x - this.transform.localPosition.x) < parallaxSpeed * 2f) && moveDifference.x < 0) {
				if(onlyMoveXOnRespawn) {
					this.transform.localPosition = TransformUtils.ModifyX(this.transform, targetPosition.x, true);
				} else {
					this.transform.localPosition = targetPosition;
				}
			}
		}
	}
}
