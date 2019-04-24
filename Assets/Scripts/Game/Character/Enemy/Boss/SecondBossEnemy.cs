using UnityEngine;
using System.Collections;

public class SecondBossEnemy : BossEnemy {

	public float stageTwoMoveSpeed = 10f;
	
	public int damageRequiredForStageThree = 70;

	public Collider leftRightCollider, upDownCollider;

	public float moveSpeed = 5f;
	public Transform[] rideTargets;

	private Direction rideDirection = Direction.RIGHT;
	private bool isRidingBackwards = false;
	
	private bool isInStageThree;

	private SoundObject rideSound;
	private SpriteRenderer lrShadowSprite, udShadowSprite;

	public override void Awake () {
		base.Awake ();
		lrShadowSprite = this.transform.Find("Animations/ShadowLR").GetComponent<SpriteRenderer>();
		udShadowSprite = this.transform.Find("Animations/ShadowUD").GetComponent<SpriteRenderer>();

		rideSound = this.transform.Find("Sounds/RideSound").GetComponent<SoundObject>();
	}

	public override void OnHit (float damage) {
		base.OnHit (damage);
		
		if(!isDead && HasEnteredStageTwo()) {
			SetSpeed(stageTwoMoveSpeed);
		}
	}

	public override void OnActivate () {
		base.OnActivate ();
		RideLeft();
		rideSound.Play();
		//Invoke ("SetSpeedHigher", 5f);
		//Invoke ("SetReverse", 10f);
	}

	private void SetReverse() {
		isRidingBackwards = true;
	}

	private void RideRight() {
		rideDirection = Direction.RIGHT;

		lrShadowSprite.enabled = true;
		udShadowSprite.enabled = false;

		leftRightCollider.enabled = true;
		upDownCollider.enabled = false;

		string onComplete = isRidingBackwards ? "RideDown" : "RideUp";
		Vector3 target = isRidingBackwards ? rideTargets[2].position : rideTargets[1].position;

		animationManager.PlayAnimationByName("RideRight", true);

		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder()
		              .SetName("RideRight")
		              .SetPosition(target)
		              .SetEaseType(iTween.EaseType.linear)
		              .SetSpeed(moveSpeed)
		              .SetOnComplete(onComplete)
		              .SetOnCompleteTarget(this.gameObject)
		              .Build());
	}

	private void RideLeft() {
		rideDirection = Direction.LEFT;

		lrShadowSprite.enabled = true;
		udShadowSprite.enabled = false;

		leftRightCollider.enabled = true;
		upDownCollider.enabled = false;

		string onComplete = isRidingBackwards ? "RideUp" : "RideDown";
		Vector3 target = isRidingBackwards ? rideTargets[0].position : rideTargets[3].position;

		animationManager.PlayAnimationByName("RideLeft", true);

		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder()
		              .SetName("RideLeft")
		              .SetPosition(target)
		              .SetEaseType(iTween.EaseType.linear)
		              .SetSpeed(moveSpeed)
		              .SetOnComplete(onComplete)
		              .SetOnCompleteTarget(this.gameObject)
		              .Build());
	}

	private void RideUp() {
		rideDirection = Direction.UP;

		lrShadowSprite.enabled = false;
		udShadowSprite.enabled = true;

		upDownCollider.enabled = true;
		leftRightCollider.enabled = false;

		string onComplete = isRidingBackwards ? "RideRight" : "RideLeft";
		Vector3 target = isRidingBackwards ? rideTargets[3].position : rideTargets[2].position;

		animationManager.PlayAnimationByName("RideUp", true);

		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder()
		              .SetName("RideUp")
		              .SetPosition(target)
		              .SetEaseType(iTween.EaseType.linear)
		              .SetSpeed(moveSpeed)
		              .SetOnComplete(onComplete)
		              .SetOnCompleteTarget(this.gameObject)
		              .Build());
	}

	private void RideDown() {
		rideDirection = Direction.DOWN;

		lrShadowSprite.enabled = false;
		udShadowSprite.enabled = true;

		upDownCollider.enabled = true;
		leftRightCollider.enabled = false;

		string onComplete = isRidingBackwards ? "RideLeft" : "RideRight";
		Vector3 target = isRidingBackwards ? rideTargets[1].position : rideTargets[0].position;

		animationManager.PlayAnimationByName("RideDown", true);

		iTween.MoveTo(this.gameObject, 
		              new ITweenBuilder()
		              .SetName("RideDown")
		              .SetPosition(target)
		              .SetEaseType(iTween.EaseType.linear)
		              .SetSpeed(moveSpeed)
		              .SetOnComplete(onComplete)
		              .SetOnCompleteTarget(this.gameObject)
		              .Build());
	}

	protected override void OnReallyDied () {
        DispatchMessage("OnHealthbarDepleted", null);
    }

	protected override void DoExtraOnDeath () {
		StopMoving();
	}

	private void StopMoving() {
		iTween.StopByName(this.gameObject, "RideDown");
		iTween.StopByName(this.gameObject, "RideUp");
		iTween.StopByName(this.gameObject, "RideLeft");
		iTween.StopByName(this.gameObject, "RideRight");
	}

	public void SetSpeed(float newSpeed) {
		this.moveSpeed = newSpeed;
	}

	public Direction GetRideDirection() {
		return rideDirection;
	}

	public bool IsRidingBackwards() {
		return isRidingBackwards;
	}

	public bool HasEnteredStageThree() {
		bool isInStageThree = false;

		if(!this.isInStageThree && currentDamage >= damageRequiredForStageThree) {
			this.isInStageThree = true;
			isInStageThree = true;
		}
		return isInStageThree;
	}

	protected override void OnDie () {
		StopMoving();
        animationManager.EnableSwitchAnimations();
		base.OnDie ();
	}

	public bool IsInStageThree() {
		return this.isInStageThree;
	}

	public override Collider GetCollider () {
		if(upDownCollider.enabled) {
			return upDownCollider;
		} else {
			return leftRightCollider;
		}
	}

	public override Collider[] GetColliders() {
		return new Collider[] { upDownCollider, leftRightCollider };
	}

}
