using UnityEngine;
using System.Collections;

public class SkeletonRibAttack : EnemyAction {

	public float attackDelay = .5f;
	public string attackAnimation = "RibAttack";

	public float minimumThrowPower, maximumThrowPower;
	public EnemyWeapon enemyWeaponPrefab;
	public Transform throwPosition;
	public float minimimThrowTimeout, maximumThrowTimeout;

	protected bool isMoving = false;
	protected Vector3 moveDirection = Vector3.zero;

	private BodyControl bodyControl;

	protected override void OnActionStarted () {
		Invoke ("PlayAnimation", Random.Range (minimimThrowTimeout, maximumThrowTimeout));

		bodyControl = controllingEnemy.GetComponent<BodyControl>();

		controllingEnemy.PlayAnimationByName("Walking", true);
		
		isMoving = true;

		base.OnActionStarted ();
	}
	
	protected override void OnActionFinished () {
		CancelInvoke("PlayAnimation");
		CancelInvoke("DoAttack");

		base.OnActionFinished ();
	}

	protected override void OnUpdate () {
		if(isMoving) {

			ChooseMoveDirection();
			controllingEnemy.transform.position += (bodyControl.moveSpeed * moveDirection);
		}
	}

	public void PlayAnimation() {
		isMoving  = false;

		controllingEnemy.PlayAnimationByName(attackAnimation, true);
	
		Invoke ("DoAttack", attackDelay);
	}

	public virtual void DoAttack() {

		OnMoving();

		EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, throwPosition.position, Quaternion.identity);
		enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;
		enemyWeapon.ThrowHorizontalVerticallyAt(SceneUtils.FindObject<Player>().transform, Random.Range (minimumThrowPower, maximumThrowPower));

		Invoke ("PlayAnimation", Random.Range (minimimThrowTimeout, maximumThrowTimeout));
	}

	protected virtual void ChooseMoveDirection() {
		Direction directionToPlayer = MathUtils.GetDirection(new Vector2(player.transform.position.x, player.transform.position.z), 
		                                                     new Vector2(controllingEnemy.transform.position.x, controllingEnemy.transform.position.z));
		
		moveDirection = MathUtils.GetDirectionAsVector3(directionToPlayer);
	}

	protected virtual void OnMoving() {
		controllingEnemy.PlayAnimationByName("Walking", true);
		
		isMoving = true;
	}
}
