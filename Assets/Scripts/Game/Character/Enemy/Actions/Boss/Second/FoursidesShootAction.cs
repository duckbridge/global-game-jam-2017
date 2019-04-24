using UnityEngine;
using System.Collections;

public class FoursidesShootAction : EnemyAction {

	public Transform leftShootSource, upShootSource, rightShootSource, downShootSource;

	public float minimumAfterShootTimeoutStageThree;
	public float maximumAfterShootTimeoutStageThree;

	public float minimumAfterShootTimeout = .3f;
	public float maximumAfterShootTimeout = 1f;
	
	public string actionOnDone = "MoveAligned";
	public EnemyWeapon enemyWeaponPrefab;
	
	public float minimumBulletSpeed = .8f;
	public float maximumBulletSpeed = 1f;
	
	public float minimumBulletSpeedStageThree = .8f;
	public float maximumBulletSpeedStageThree = 1f;
	
	public SoundObject shootSound;

	private SecondBossEnemy bossEnemy;
	private Direction shootDirection;

	protected override void OnActionStarted () {
		bossEnemy = controllingEnemy.GetComponent<SecondBossEnemy>();	
		Invoke("PrepareShooting", .5f);
		
		base.OnActionStarted ();
	}

	private void PrepareShooting() {

		shootDirection = Direction.RIGHT;

		switch(bossEnemy.GetRideDirection()) {
			case Direction.RIGHT:
				
				shootDirection = Direction.UP;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.DOWN;
				}
			break;

			case Direction.LEFT:

				shootDirection = Direction.DOWN;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.UP;
				}
			break;
		
			case Direction.UP:

				shootDirection = Direction.LEFT;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.RIGHT;
				}
			break;

			case Direction.DOWN:

				shootDirection = Direction.RIGHT;
				
				if(bossEnemy.IsRidingBackwards()) {
					shootDirection = Direction.LEFT;
				}
			break;

		}

		string animationName = bossEnemy.GetRideDirection() + "-" + shootDirection + "-Shoot";
	
		Logger.Log (animationName);

		controllingEnemy.GetAnimationManager().PlayAnimationByName(animationName, true);

		DoShoot ();
	}
	
	private void DoShoot() {
		shootSound.Play();

		Transform shootSource = this.transform;

		switch(shootDirection) {
			case Direction.UP:
				shootSource = upShootSource;
			break;

			case Direction.DOWN:
				shootSource = downShootSource;
			break;

			case Direction.RIGHT:
				shootSource = rightShootSource;
			break;

			case Direction.LEFT:
				shootSource = leftShootSource;
			break;

		}

		EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, shootSource.position, Quaternion.identity);
		enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;

		if(bossEnemy.IsInStageThree()) {
			enemyWeapon.ThrowInDirection(MathUtils.GetDirectionAsVector3(shootDirection), Random.Range (minimumBulletSpeedStageThree, maximumBulletSpeedStageThree));
			Invoke ("OnActionDone", Random.Range (minimumAfterShootTimeoutStageThree, maximumAfterShootTimeoutStageThree));
		} else {
			enemyWeapon.ThrowInDirection(MathUtils.GetDirectionAsVector3(shootDirection), Random.Range (minimumBulletSpeed, maximumBulletSpeed));
			Invoke ("OnActionDone", Random.Range (minimumAfterShootTimeout, maximumAfterShootTimeout));
		}
	}
	
    protected override void OnActionFinished() {
        CancelInvoke("PrepareShooting");
        CancelInvoke("OnActionDone");

        base.OnActionFinished();
    }

	private void OnActionDone() {
		DeActivate(actionOnDone);
	}
}
