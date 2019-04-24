using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossEnemy : Enemy {

	public int maxBreakFreeFrame = 4;
	private int currentBreakFreeFrame = 0;
	
	public int damageRequiredForStageTwo = 200;

	public Vector2 screenShakeOnDeath = new Vector2(4f, 4f);
	public Blink2D deathAnimationBlinker;
	public List<Animation2D> explosionAnimations;

	protected SlomoComponent slomoComponent;
	protected bool isInStageTwo = false;

	private bool canBreakBoss = false;

	public override void Awake () {
		base.Awake ();
		slomoComponent = GetComponent<SlomoComponent>();
	}

	protected override void OnDie () {

		canHitPlayer = false;

		if(this.onDeathSound) {
			this.onDeathSound.PlayIndependent(true);
		}

		List<Enemy> enemiesInScene = SceneUtils.FindObjects<Enemy>();

		for(int i = 0 ; i < enemiesInScene.Count ; i++) {
			if(enemiesInScene[i].GetComponent<BossEnemy>()) {
				continue;
			} else {
				Enemy enemyToDestroy = enemiesInScene[i];
				enemiesInScene.RemoveAt(i);
				Destroy (enemyToDestroy.gameObject);
				--i;
			}
		}

		if(deathAnimationBlinker) {
			deathAnimationBlinker.StopBlinking();
		}

		OnDeActivate();

		if(GetComponent<CharacterControl>()) {
			GetComponent<CharacterControl>().OnDie();
		}

		OnReallyDied();

		enemyAIActionManager.StopCurrentAction();
		
		if(shadow) {
			shadow.enabled = false;
		}

		healthbar.gameObject.SetActive(false);
		
		animationManager.StopHideAllAnimations();
		
		animationManager.AddEventListenerTo("Death", this.gameObject);
		animationManager.PlayAnimationByName("Death", true);

		if(slomoComponent) {
			slomoComponent.DoSlomo();
		}

		SceneUtils.FindObject<CameraShaker>().ShakeCamera(screenShakeOnDeath);
			
		isDead = true;
	}

	protected virtual void IgnoreCollisionBetweenPlayerAndBoss() {
		PhysicsUtils.IgnoreCollisionBetween(player.GetComponent<Collider>(), this.GetComponent<Collider>());
	}

	protected virtual void OnReallyDied() {

		this.GetComponent<Rigidbody>().isKinematic = true;
		this.GetComponent<Collider>().enabled = false;
   
        DispatchMessage("OnHealthbarDepleted", null);

	}

	public override void OnHit (float damage) {
		if(isDead) {
			
			if(canBreakBoss) {
				if(currentBreakFreeFrame > maxBreakFreeFrame) {
					canBreakBoss = false;
					
					animationManager.ResumeAnimationByName("BreakFree");
					OnBrokenFree();
				} else {
					++currentBreakFreeFrame;
					
					animationManager.StopHideAnimationByName("Death");
					
					animationManager.AddEventListenerTo("BreakFree", this.gameObject);
					animationManager.SetFrameForAnimation("BreakFree", currentBreakFreeFrame);
				}
			}
		} else {
			base.OnHit (damage);
		}
	}

	protected override void OnDeathAnimationDone () {

		enemyAIActionManager.StopCurrentAction();
		enemyAIActionManager.GetActions().ForEach(action => action.Disable());

		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().useGravity = false;

		GetCollider ();

        BoomBox boomboxOnBack = player.GetComponentInChildren<BoomBox>();
        if(boomboxOnBack) {
            boomboxOnBack.ShowTextBox("FinishBoss");
        }
		canBreakBoss = true;
	}

	public override void OnAnimationDone(Animation2D animation2D) {
		base.OnAnimationDone(animation2D);

		if(animation2D.name.Contains("Explosion")) {
			animationManager.StopHideAnimationByName("Death");
		}

		if(animation2D.name == "BreakFree") {
			DispatchMessage("OnEnemyDied", this);
		}
	}

	private void DoExplode() {
		explosionAnimations[0].AddEventListener(this.gameObject);
		
		foreach(Animation2D explosion in explosionAnimations) {
			explosion.Play (true);
		}
		Invoke ("DoExtraOnDeath", .5f);
	}

	protected override void DoExtraOnDeath() {
		DispatchMessage("OnEnemyDied", this);
	}

	public bool HasEnteredStageTwo() {
		bool isInStageTwo = false;
		
		if(!this.isInStageTwo && currentDamage >= damageRequiredForStageTwo) {
			this.isInStageTwo = true;
			isInStageTwo = true;
		}
		
		return isInStageTwo;
	}

	public bool IsInStageTwo() {
		isInStageTwo = currentDamage >= damageRequiredForStageTwo;

		return isInStageTwo;
	}

	protected void OnBrokenFree() {
		if(slomoComponent) {
			slomoComponent.StopSlomo();
			SceneUtils.FindObject<MusicManager>().StopPlayingMusic();
		}
	}
}
