using UnityEngine;
using System.Collections;

public class EnemyWeapon : Weapon {

	public float destroyTimeout = 5f;
	public float rotationSpeed = 0f;

	protected float currentThrowPower;
	protected Vector3 throwDirection;

	public void ThrowAt(Transform target, float throwPower) {
		Invoke ("DoDestroy", destroyTimeout);

		currentThrowPower = throwPower;
		this.throwDirection = MathUtils.CalculateDirection(new Vector2(target.position.x, target.position.z), new Vector2(this.transform.position.x, this.transform.position.z));
	}
	
	public void ThrowInDirection(Vector3 throwDirection, float throwPower) {
		Invoke ("DoDestroy", destroyTimeout);
		
		currentThrowPower = throwPower;

		if(rotationSpeed == 0) {
			this.transform.right = throwDirection;
		}

		this.throwDirection = throwDirection;
	}

	public void ThrowHorizontalVerticallyAt(Transform target, float throwPower) {
		Invoke ("DoDestroy", destroyTimeout);

		Direction directionToTarget = MathUtils.GetDirection(new Vector2(target.position.x, target.position.z), new Vector2(this.transform.position.x, this.transform.position.z));

		this.throwDirection = MathUtils.GetDirectionAsVector3(directionToTarget);

		if(rotationSpeed == 0) {
			this.transform.right = throwDirection;
		}

		currentThrowPower = throwPower;
	}

	public override void FixedUpdate () {

		if(rotationSpeed > 0f) {
			this.transform.Rotate (new Vector3(0f, rotationSpeed, 0f));			
		}

		this.transform.position += throwDirection * currentThrowPower;

	}

	public void OnTriggerEnter(Collider coll) {

		Wall wall = coll.gameObject.GetComponent<Wall>();
		
		if(wall) {
			
			Destroy (this.gameObject);
			
		}

		Player player = coll.gameObject.GetComponent<Player>();
		
		if(player && !canBePickedUpByPlayer) {
			
			player.OnHit(this.transform.position);
			Destroy (this.gameObject);

		}
	}

	private void DoDestroy() {
		Destroy(this.gameObject);
	}
}
