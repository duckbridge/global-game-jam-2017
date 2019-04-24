using UnityEngine;
using System.Collections;

public class FirstBossEnemy : BossEnemy {

	public BoxCollider regularBoxCollider, flyingBoxCollider;
	public AnimationManager2D regularAnimationManager, flyingAnimationManager;
	public GameObject legsOnly;

	protected override void OnReallyDied () {}

	protected override void DoExtraOnDeath () {}

	public override void OnHit (float damage) {
		base.OnHit (damage);

		if(!isDead && HasEnteredStageTwo()) {
			regularAnimationManager.gameObject.SetActive (false);
			flyingAnimationManager.gameObject.SetActive (true);

			this.animationManager = flyingAnimationManager;
			this.animationManager.PlayAnimationByName ("Idle", true);

			legsOnly.SetActive (true);
			legsOnly.transform.parent = null;

			regularBoxCollider.enabled = false;
			flyingBoxCollider.enabled = true;

			this.GetComponent<Rigidbody> ().isKinematic = false;
			this.GetComponent<Rigidbody> ().useGravity = true;
			enemyAIActionManager.StopCurrentAction ();
			enemyAIActionManager.OnActionDone ("MoveToPlayer");
		}
	}

}
