using UnityEngine;
using System.Collections;

public class BounceAroundRoom : EnemyAction {

	public Vector3[] bouncePositions;

	private BodyControl bodyControl;

	private Vector3 bounceTarget;
	private Camera gameCamera;
	private GameObject cameraContainer;

	private int currentIndex = -1;
	private bool canCheckForNewTarget = true;

	protected override void OnActionStarted () {

		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();
		cameraContainer = SceneUtils.FindObject<CameraBorderManager>().gameObject;

		bodyControl = controllingEnemy.GetComponent<BodyControl>();

		for(int i = 0 ; i < bouncePositions.Length ; i++) {
			bouncePositions[i] = gameCamera.ViewportToWorldPoint(bouncePositions[i]);
		}

		base.OnActionStarted ();
        
        controllingEnemy.PlayAnimationByName("Walking", true);

		ChooseNewBounceTarget();
	}

	protected override void OnUpdate () {
		Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);

		bodyControl.MoveKinematic(bounceTarget.x, bounceTarget.z, false);

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
        CancelInvoke("ResetCheckForNewTarget");

	}

	private void ChooseNewBounceTarget() {
		iTween.StopByName(this.gameObject, "BounceMove");

		currentIndex++;

		if(currentIndex >= bouncePositions.Length) {
			currentIndex = 0;
		}

		bounceTarget = MathUtils.CalculateDirection(bouncePositions[currentIndex], controllingEnemy.transform.position);
		bounceTarget.y = this.transform.position.y;
		
		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder().SetPosition(bounceTarget)
		              .SetSpeed(bodyControl.moveSpeed).SetName("BounceMove")
		              .SetOnCompleteTarget(this.gameObject).SetOnComplete("ChooseNewBounceTarget").Build ());
	}
}
