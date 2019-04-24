using UnityEngine;
using System.Collections;

public class TeleportToPosition : EnemyAction {

	public SoundObject hideSound, showSound;

	public float minimumShowTime = 2f;
	public float maximumShowTime = 5f;

	public float minimumShowUpTime = .5f;
	public float maximumShowUpTime = 1f;

    public Transform[] positions;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		controllingEnemy.HideHealthbar();
		controllingEnemy.AddEventListenerTo("ShowUp", this.gameObject);
		controllingEnemy.AddEventListenerTo("Hide", this.gameObject);

		Invoke("PrepareShowUp", .5f);
	}

	private void PrepareShowUp() {
        Invoke ("ShowUp", Random.Range (minimumShowUpTime, maximumShowUpTime));
	}

	private void ShowUp() {

        Transform randomPosition = positions[Random.Range(0, positions.Length)];
		showSound.Play();

		controllingEnemy.transform.position = new Vector3(randomPosition.position.x, controllingEnemy.transform.position.y, randomPosition.position.z);
		
		controllingEnemy.PlayAnimationByName("ShowUp", true);
		Invoke ("EnableColliderAndHealthbar", .25f);
	}

	private void EnableColliderAndHealthbar() {
		controllingEnemy.GetComponent<Collider>().enabled = true;
		PhysicsUtils.RestoreCollisionBetween (controllingEnemy.GetComponent<Collider> (), player.GetComponent<Collider> ());
		controllingEnemy.ShowHealthbar();
	}
	
	public void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "ShowUp") {
			Invoke ("Hide", Random.Range (minimumShowTime, maximumShowTime));
		}

		if(animation2D.name == "Hide") {
			controllingEnemy.HideAnimationByName("Hide");

			controllingEnemy.HideHealthbar();
			controllingEnemy.GetComponent<Collider>().enabled = false;
		
			PrepareShowUp();
		}
	}

	private void Hide() {
		hideSound.Play();

		controllingEnemy.ShowAnimationByName("Hide");
		controllingEnemy.PlayAnimationByName("Hide", true);
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();
		CancelInvoke("Hide");
		CancelInvoke("ShowUp");
		CancelInvoke("PrepareShowUp");
	}
}
