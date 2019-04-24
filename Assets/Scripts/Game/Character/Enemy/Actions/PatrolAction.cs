using UnityEngine;
using System.Collections;

public class PatrolAction : EnemyAction {
	
	public float randomMinStopTime = .5f;
	public float randomMaxStopTime = 1f;

	public string actionNameOnDone = "";
	public float minimumRandomTimeout, maximumRandomTimeout;

	public float closeToTargetDistance = .3f;
	private BodyControl bodyControl;

	protected Camera gameCamera;
	protected GameObject cameraContainer;

	protected Vector4 patrolArea;

	protected override void OnActionStarted () {
		controllingEnemy.PlayAnimationByName("Walking", true);

		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();
		cameraContainer = SceneUtils.FindObject<CameraBorderManager>().gameObject;
		
		bodyControl = controllingEnemy.GetComponent<BodyControl>();

		patrolArea = GetPatrolArea();

		base.OnActionStarted ();
	
		Vector2 randomPosition = new Vector2(Random.Range (patrolArea.x, patrolArea.y),
		                                     Random.Range (patrolArea.z, patrolArea.w));

		iTween.MoveTo(controllingEnemy.gameObject, 
		              new ITweenBuilder().SetSpeed(bodyControl.moveSpeed)
		              .SetPosition(new Vector3(
									randomPosition.x,
		                         	controllingEnemy.transform.position.y,
		                         	randomPosition.y))
		              .SetEaseType(iTween.EaseType.linear)
		              .SetOnCompleteTarget(this.gameObject)
		              .SetOnComplete("OnDoneWithAction")
		              .Build());

	}

	public override void OnTriggered (Collider coll) {
		Wall wall = coll.gameObject.GetComponent<Wall>();
		if(wall) {
			OnDoneWithAction();
		}
	}

	public override void OnCollided (Collision coll) {
		Wall wall = coll.gameObject.GetComponent<Wall>();
		if(wall) {
			OnDoneWithAction();
		}
	}

	protected override void OnUpdate () {}

	private void OnDoneWithAction() {
		CancelInvoke("OnDoneWithAction");
		Invoke("SwitchToNewAction", Random.Range (randomMinStopTime, randomMaxStopTime));
	}

	public void SwitchToNewAction() {
		CancelInvoke("SwitchToNewAction");
		DeActivate (actionNameOnDone);
	}

	protected virtual Vector4 GetPatrolArea() {
		return new Vector4(gameCamera.ViewportToWorldPoint(new Vector3(.15f, 0f, 0f)).x,
		            gameCamera.ViewportToWorldPoint(new Vector3(.85f, 0f, 0f)).x,
		            gameCamera.ViewportToWorldPoint(new Vector3(0f, .2f, 0f)).z,
		            gameCamera.ViewportToWorldPoint(new Vector3(0f, .8f, 0f)).z);
	}
}
