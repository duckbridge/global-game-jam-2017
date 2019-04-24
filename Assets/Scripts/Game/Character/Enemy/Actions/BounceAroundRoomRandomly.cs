using UnityEngine;
using System.Collections;

public class BounceAroundRoomRandomly : EnemyAction {

    public int minimumRotation = 65;
    public int maximumRotation = 125;

	private BodyControl bodyControl;

	private Transform directionTransform;

	private bool canCheckForNewTarget = true;
	private Vector3 currentDirection;

	private Camera gameCamera;

    public override void Start() {
        base.Start();
        directionTransform = this.transform.transform.Find("DirectionTransform");
    }

	protected override void OnActionStarted () {

		bodyControl = controllingEnemy.GetComponent<BodyControl>();

		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();

		base.OnActionStarted ();
        
        controllingEnemy.PlayAnimationByName("Walking", true);

		ChooseNewBounceTarget();
	}

	protected override void OnUpdate () {

		Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);

        bodyControl.MoveKinematic(currentDirection.x, currentDirection.z, false);
    
		Vector3 positionInCamera = gameCamera.WorldToViewportPoint(controllingEnemy.transform.position);

		if(canCheckForNewTarget) {
			if((positionInCamera.x > 0.95f)
			   || (positionInCamera.x < 0.05f) 
			   || (positionInCamera.y < 0.05f) 
				|| (positionInCamera.y > 0.95f)) {

				canCheckForNewTarget = false;
				ChooseNewBounceTarget();

				Invoke ("ResetCheckForNewTarget", 1f);
			}
		}
	}

	private void ResetCheckForNewTarget() {
		canCheckForNewTarget = true;
	}
	
	public override void OnTriggered(Collider coll) {
		Wall wall = coll.gameObject.GetComponent<Wall>();
		if(wall) {
			ChooseNewBounceTarget();
		}
	}
	
	public override void OnCollided(Collision coll) {
		Wall wall = coll.gameObject.GetComponent<Wall>();
		if(wall) {
			ChooseNewBounceTarget();
		}
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();
		iTween.StopByName(this.gameObject, "BounceMove");
	}

	private void ChooseNewBounceTarget() {
		int rotationAmount = UnityEngine.Random.Range(minimumRotation, maximumRotation);
		directionTransform.Rotate(new Vector3(0f, rotationAmount, 0f));
		
		currentDirection = directionTransform.right;
	}
}
