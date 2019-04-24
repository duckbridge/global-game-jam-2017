using UnityEngine;
using System.Collections;

public class StunAction : EnemyAction {

	private string actionToSwitchTo;
	private float unStunTime = 120f;
	private string oldAnimationName;
	private Color stunColor;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		if (!player) {
			player = SceneUtils.FindObject<Player> ();
		}

		controllingEnemy.SetStunned (true);

		controllingEnemy.PlayStunnedSound ();

		if(controllingEnemy.GetComponent<BodyControl>()) {
			controllingEnemy.GetComponent<BodyControl> ().StopMoving ();
		}

		controllingEnemy.GetAnimationManager ().DisableSwitchAnimations ();
		oldAnimationName = controllingEnemy.GetAnimationManager ().GetCurrentAnimation ().name;

		SpriteRenderer targetSpriteRenderer = 
			controllingEnemy.GetAnimationManager ().GetAnimationByName ("Stun").outputRenderer;

		controllingEnemy.GetAnimationManager ().PlayAnimationByName ("Stun", true, false, true);
		controllingEnemy.GetAnimationManager ().SetColorForAnimation ("Stun", stunColor);

		GetComponent<Fading2D> ().SetTarget (targetSpriteRenderer);

		GetComponent<Fading2D> ().AddEventListener (this.gameObject);
		GetComponent<Fading2D> ().FadeInto (Color.white, unStunTime, FadeType.FADEIN);
	}

	public void OnFadingDone(FadeType fadeType) {
		OnStunDone ();
	}

	public void SetActionToSwitchTo(string actionToSwitchTo) {
		this.actionToSwitchTo = actionToSwitchTo;
	}

	public void SetStunColor(Color stunColor) {
		this.stunColor = stunColor;
	}

	public void SetUnStunTime(float unStunTime) {
		this.unStunTime = unStunTime;
	}

	protected override void OnActionFinished () {
		base.OnActionFinished ();
		DisableStun ();
	}

	private void OnStunDone() {
		DeActivate (actionToSwitchTo);
	}

	private void DisableStun() {

		controllingEnemy.SetStunned (false);

		if (!player.IsRolling ()) {
			PhysicsUtils.RestoreCollisionBetween (player.GetComponent<Collider> (), controllingEnemy.GetColliders ());
		}

		controllingEnemy.GetAnimationManager ().EnableSwitchAnimations ();
		controllingEnemy.GetAnimationManager ().PlayAnimationByName (oldAnimationName, true, false, true);
	}
}
