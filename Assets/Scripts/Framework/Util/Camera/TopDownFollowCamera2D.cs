using UnityEngine;
using System.Collections;

public class TopDownFollowCamera2D : DispatchBehaviour {

	public GameObject gameObjectToFollow;

    public float cameraMoveSpeedX = 0.05f;
    public float cameraMoveSpeedY = 0.05f;
   
    public float minimumDistanceX = 0.5f;
    public float minimumDistanceY = 0.5f;

	private Vector3 oldPosition;

	public bool doStickyFollowing = true;

	void Awake() {

	}

	void Start () {}
	
	void FixedUpdate () {
		oldPosition = this.transform.position;

		if(!doStickyFollowing) {
			MoveCameraToTarget();
		} else {
			this.transform.position = new Vector3(gameObjectToFollow.transform.position.x, this.transform.position.y, gameObjectToFollow.transform.position.z);
		}

		if((this.transform.position - oldPosition) != Vector3.zero) {
			DispatchMessage("OnCameraMoved", (this.transform.position - oldPosition));
		}
	}
	
	private void MoveCameraToTarget() {
		Vector3 distanceToTarget = GetDistanceToFollowingObject();
		if(Mathf.Abs(distanceToTarget.x) > minimumDistanceX || Mathf.Abs (distanceToTarget.z) > minimumDistanceY) {

			MoveCameraWithDistance(distanceToTarget);
		}
	}
	

	private Vector3 GetDistanceToFollowingObject() {
		Vector3 distanceToFollowingObject = gameObjectToFollow.transform.position - this.transform.position;

		return distanceToFollowingObject;
	}

	public void MoveCameraWithDistance(Vector3 distance) {
		this.transform.position = 
			new Vector3(this.transform.position.x + (distance.x * cameraMoveSpeedX), 
			            this.transform.position.y, this.transform.position.z  + (distance.z * cameraMoveSpeedY));
	}
	
	public override void OnPauseGame() {}
	
	public override void OnResumeGame() {}
}
