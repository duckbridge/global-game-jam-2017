using UnityEngine;
using System.Collections;

public class TeleportToRandomPosition : EnemyAction {

	public SoundObject hideSound, showSound;

	public float minimumShowTime = 2f;
	public float maximumShowTime = 5f;

	public float minimumShowUpTime = .5f;
	public float maximumShowUpTime = 1f;

	private Vector3 oldPlayerPosition;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		controllingEnemy.HideHealthbar();
		controllingEnemy.AddEventListenerTo("ShowUp", this.gameObject);
		controllingEnemy.AddEventListenerTo("Hide", this.gameObject);

		Invoke("PrepareShowUp", .5f);
	}

	private void PrepareShowUp() {

		oldPlayerPosition = player.transform.position;

		Invoke ("ShowUp", Random.Range (minimumShowUpTime, maximumShowUpTime));

	}

	private void ShowUp() {
		showSound.Play();

		controllingEnemy.transform.position = new Vector3(oldPlayerPosition.x, controllingEnemy.transform.position.y, oldPlayerPosition.z);
		
		controllingEnemy.PlayAnimationByName("ShowUp", true);
		Invoke ("EnableColliderAndHealthbar", .25f);
	}

	private void EnableColliderAndHealthbar() {
		controllingEnemy.GetComponent<Collider>().enabled = true;
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
