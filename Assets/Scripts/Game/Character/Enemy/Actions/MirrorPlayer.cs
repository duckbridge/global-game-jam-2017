using UnityEngine;
using System.Collections;
using InControl;

public class MirrorPlayer : EnemyAction {

	public float minimumThrowPower, maximumThrowPower;
	public EnemyWeapon enemyWeaponPrefab;

	private PlayerInputActions playerInputActions;
	private BodyControl bodyControl;

	protected override void OnRegularUpdate () {

		bodyControl.DoMove(playerInputActions.moveHorizontally.Value * -1, playerInputActions.moveVertically.Value * -1);

	}

	public void OnBeatDone() {
		if(isActive) {
			EnemyWeapon enemyWeapon = (EnemyWeapon) GameObject.Instantiate(enemyWeaponPrefab, this.transform.position, Quaternion.identity);
			enemyWeapon.transform.parent = controllingEnemy.GetRoom().transform;
			enemyWeapon.ThrowHorizontalVerticallyAt(player.transform, Random.Range (minimumThrowPower, maximumThrowPower));
		}
	}

	protected override void OnActionStarted () {

		base.OnActionStarted ();
		playerInputActions = new PlayerInputActions();
		
		playerInputActions.left.AddDefaultBinding(Key.A);
		playerInputActions.left.AddDefaultBinding(InputControlType.LeftStickLeft);
		
		playerInputActions.right.AddDefaultBinding(Key.D);
		playerInputActions.right.AddDefaultBinding(InputControlType.LeftStickRight);
		
		playerInputActions.up.AddDefaultBinding(Key.W);
		playerInputActions.up.AddDefaultBinding(InputControlType.LeftStickUp);
		
		playerInputActions.down.AddDefaultBinding(Key.S);
		playerInputActions.down.AddDefaultBinding(InputControlType.LeftStickDown);

		player.AddEventListener(this.gameObject);

		bodyControl = controllingEnemy.GetComponent<BodyControl>();
	}
}
