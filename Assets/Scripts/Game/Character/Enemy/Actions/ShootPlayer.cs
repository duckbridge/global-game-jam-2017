using UnityEngine;
using System.Collections;

public class ShootPlayer : EnemyAction {

	public Transform shootSource;
	public float shootTimeout = .5f;
	public float afterShootTimeout = 1f;

	public string actionOnDone = "MoveAligned";
	public EnemyWeapon enemyWeaponPrefab;

	public float minimumThrowPower = .8f;
	public float maximumThrowPower = 1f;
	public SoundObject shootSound;

	protected override void OnActionStarted () {

		if(controllingEnemy.GetAnimationControl()) {
			controllingEnemy.GetAnimationControl().PlayAnimationByName("Shoot", true);
		} else {
			controllingEnemy.PlayAnimationByName("Shoot", true);
		}

		Invoke ("DoShoot", shootTimeout);

		base.OnActionStarted ();
	}

	private void DoShoot() {
		shootSound.Play();

		EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, this.transform.position, Quaternion.identity);
		enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;
		enemyWeapon.ThrowHorizontalVerticallyAt(player.transform, Random.Range (minimumThrowPower, maximumThrowPower));

		Invoke ("OnActionDone", afterShootTimeout);
	}

	private void OnActionDone() {
		DeActivate(actionOnDone);
	}
}
