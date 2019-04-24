using UnityEngine;
using System.Collections;

public class ShootFullRotationPlayer : EnemyAction {

	public bool shootsRandomly = false;

	public Transform shootSource;
	public float shootTimeout = .5f;
	public float afterShootTimeout = 1f;

	public string actionOnDone = "ShootFullRotation";
	public EnemyWeapon enemyWeaponPrefab;

	public float minimumThrowPower = .8f;
	public float maximumThrowPower = 1f;
	public SoundObject shootSound;

	private Player playerTarget;
	private Vector3 shootDirection;

	protected override void OnActionStarted () {

		if(!playerTarget) {
			playerTarget = SceneUtils.FindObject<Player>();
		}

		Animation2D shootAnimation = controllingEnemy.GetAnimationManager().GetAnimationByName("Shoot");

		if(shootsRandomly) {

			shootDirection = new Vector2(Random.Range (-1f, 1.1f), Random.Range (-1f, 1.1f));
			if(shootDirection.x == 0 && shootDirection.y == 0) {
				shootDirection = new Vector2(-1f, -1f);
			}

			shootDirection.Normalize();

		} else {
			shootDirection = MathUtils.CalculateDirection(
				new Vector2(playerTarget.transform.position.x, playerTarget.transform.position.z), 
				new Vector2(controllingEnemy.transform.position.x, controllingEnemy.transform.position.z)
			);
		}

		//shootAnimation.transform.right = new Vector3(shootDirection.x, 0f, shootDirection.y);

		controllingEnemy.PlayAnimationByName("Shoot", true);

		Invoke ("DoShoot", shootTimeout);

		base.OnActionStarted ();
	}

	private void DoShoot() {
		shootSound.Play();

		EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, shootSource.position, Quaternion.identity);
		enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;
		enemyWeapon.ThrowInDirection(new Vector3(shootDirection.x, 0f, shootDirection.y), Random.Range(minimumThrowPower, maximumThrowPower));

		Invoke ("OnActionDone", afterShootTimeout);
	}

	private void OnActionDone() {
		DeActivate(actionOnDone);
	}
}
