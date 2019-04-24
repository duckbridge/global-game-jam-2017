using UnityEngine;
using System.Collections;

public class ChargePlayer : EnemyAction {

	public float actionDoneDelay = 2f;
	public string actionOnDone = "Charge";
	public float closeToTargetDistance = 5f;

	private Vector3 moveTarget;
	
	private BodyControl bodyControl;
	private bool isMoving = false;

	private Vector3 direction;
	private Camera gameCamera;
	private GameObject cameraContainer;

	protected override void OnActionStarted () {

		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();
		cameraContainer = SceneUtils.FindObject<CameraBorderManager>().gameObject;

		bodyControl = controllingEnemy.GetComponent<BodyControl>();
		controllingEnemy.PlayAnimationByName("Walking", true);
		
		base.OnActionStarted ();

		moveTarget = SceneUtils.FindObject<Player>().transform.position;
		direction = MathUtils.CalculateDirection(moveTarget, controllingEnemy.transform.position);

		isMoving = true;
	}

	protected override void OnUpdate () {
		if(isMoving) {

			Bounds cameraBounds = MathUtils.OrthographicBounds(gameCamera);

			bodyControl.MoveKinematic(direction.x, direction.z, true);

			Vector3 positionInCamera = gameCamera.WorldToViewportPoint(controllingEnemy.transform.position);

			if((positionInCamera.x > 0.99f)
			   || (positionInCamera.x < 0.01f) 
			   || (positionInCamera.y < 0.01f) 
				|| (positionInCamera.y > 0.99f)) {
			

				SceneUtils.FindObject<CameraShaker>().ShakeCamera(new Vector2(2f, 2f), true);

				isMoving = false;
				controllingEnemy.PlayAnimationByName("Idle", true);
				Invoke ("OnDoneWithAction", actionDoneDelay);

			} 

			float clampedPositionX = Mathf.Clamp(controllingEnemy.transform.position.x, 
			                                     controllingEnemy.transform.position.x - cameraBounds.extents.x, 
			                                     controllingEnemy.transform.position.x + cameraBounds.extents.x);
			
			float clampedPositionZ = Mathf.Clamp(controllingEnemy.transform.position.z, 
			                                     controllingEnemy.transform.position.z - cameraBounds.extents.z, 
			                                     controllingEnemy.transform.position.z + cameraBounds.extents.z);
			
			controllingEnemy.transform.position = new Vector3(clampedPositionX, controllingEnemy.transform.position.y, clampedPositionZ);
		}
	}

	private void OnDoneWithAction() {
		DeActivate(actionOnDone);
	}
}
