using UnityEngine;
using System.Collections;

public class FollowCamera2D : DispatchBehaviour {

	public GameObject gameObjectToFollow;

    public float cameraMoveSpeedX = 0.05f;
    public float cameraMoveSpeedY = 0.05f;

    public float minimumDistanceX = 0.5f;
    public float minimumDistanceY = 0.5f;

	private Vector3 oldPosition;

	public bool doStickyFollowing = true;

	private Vector3 positionFromTargetBeforeSwap;
    private Vector4 cameraFollowBorders = Vector4.zero;

	void Awake() {
	}

	void Start () {
    }

    public void Initialize(Transform tileBlockTransform) {
        VillageCenterRoom villageCenter = tileBlockTransform.GetComponentInChildren<VillageCenterRoom>();
        cameraFollowBorders = new Vector4(villageCenter.GetLeftCameraBorder().position.x, villageCenter.GetRightCameraBorder().position.x, villageCenter.GetUpCameraBorder().position.z, villageCenter.GetDownCameraBorder().position.z);
        
        if(GetComponentInChildren<Camera>().aspect < 1.7f) {        
            cameraFollowBorders.x -= 4f;
            cameraFollowBorders.y += 4f;
        }
                
    }
	
	void FixedUpdate () {
		oldPosition = this.transform.position;

		if(!doStickyFollowing) {
			MoveCameraToTarget();
		} else {

            Vector3 newPosition = new Vector3();
        
            newPosition.x = Mathf.Clamp(gameObjectToFollow.transform.position.x, cameraFollowBorders.x, cameraFollowBorders.y);
            newPosition.y = this.transform.position.y;
            newPosition.z = Mathf.Clamp(gameObjectToFollow.transform.position.z, cameraFollowBorders.z, cameraFollowBorders.w);
    
            this.transform.position = newPosition;
		}
	}

	private void MoveCameraToTarget() {
		Vector2 distanceToTarget = GetDistanceToFollowingObject();
		if(Mathf.Abs(distanceToTarget.x) > minimumDistanceX || Mathf.Abs (distanceToTarget.y) > minimumDistanceY) {

			MoveCameraWithDistance(distanceToTarget);
		}
	}
	

	private Vector2 GetDistanceToFollowingObject() {
		Vector2 distanceToFollowingObject = Vector2.zero;

        distanceToFollowingObject = 
            new Vector2((gameObjectToFollow.transform.position.x - this.transform.position.x), 
                        (gameObjectToFollow.transform.position.z - this.transform.position.z));

		return distanceToFollowingObject;
	}

	public void MoveCameraWithDistance(Vector2 distance) {

        Vector3 newPosition = new Vector3();

		newPosition.x = this.transform.position.x + (distance.x * cameraMoveSpeedX);
        newPosition.y = this.transform.position.y;
		newPosition.z = this.transform.position.z + (distance.y * cameraMoveSpeedY);

	    this.transform.position = newPosition;
            
	}

	public override void OnPauseGame() {}
	
	public override void OnResumeGame() {}
}
