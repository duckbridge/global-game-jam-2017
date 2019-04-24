using UnityEngine;
using System.Collections;

public class BonewormRibAttack : EnemyAction {

	public string actionOnDone = "Patrol";

	public float afterAttackDelay = 1f;
	public float attackDelay = 1.5f;
	public string attackAnimation = "RibAttack";
	
	public float minimumThrowPower, maximumThrowPower;
	public EnemyWeapon enemyWeaponPrefab;

	private BodyControl bodyControl;

	protected override void OnActionStarted () {
		base.OnActionStarted ();

		this.AddEventListener(controllingEnemy.gameObject);

		PlayAnimation();
	}

	public void PlayAnimation() {

		DispatchMessage("OnSwapColliders", true);

		controllingEnemy.PlayAnimationByName(attackAnimation, true);

		Invoke ("DoAttack", attackDelay);
	}

	public void DoAttack() {

		if(isActive) {
			CancelInvoke("DoAttack");

			for(int x = -1 ; x < 2 ; x++) {
				for(int y = -1 ; y < 2 ; y++) {

					if(x == 0 && y == 0) {
						continue;
					}

					EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, this.transform.position, Quaternion.identity);
					enemyWeapon.ThrowInDirection(new Vector3(x, 0f, y), Random.Range (minimumThrowPower, maximumThrowPower));
					enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;
				}
			}

			Invoke ("ContinueMoving", afterAttackDelay);
		}
	}

	public void ContinueMoving () {

		DispatchMessage("OnSwapColliders", false);

		DeActivate(actionOnDone);
	}

	protected override void OnActionFinished () {
		CancelInvoke("DoAttack");
		CancelInvoke("ContinueMoving");

		base.OnActionFinished ();
	}
}
