using UnityEngine;
using System.Collections;

public class BoomBoxWeapon : SwordWeapon {

	public FakeRope ropePrefab;
	
	private FakeRope spawnedRope;
	private GameObject ropeAnchor;

	public override void Awake() {
		base.Awake();
		canBePickedUpByPlayer = true;
	}

	public override void OnDirectionPressed (Direction directionToThrowIn) {

		ropeAnchor = this.transform.Find("RopeAnchor").gameObject;
		Invoke ("SpawnWeaponRopeDelayed", .1f);

		base.OnDirectionPressed (directionToThrowIn);
	}
	
	private void SpawnWeaponRopeDelayed() {
		if(ropePrefab) {
			spawnedRope = (FakeRope) GameObject.Instantiate(ropePrefab, this.transform.position, Quaternion.identity);

			spawnedRope.ropeBackEnd = origin;
		
			spawnedRope.ropeFrontEnd = ropeAnchor;

			spawnedRope.OnSpawned(origin.transform);

		}
	}
	
	protected override void OnOutOfBounds(Wall wall) {

		if(wall) {
			PhysicsUtils.IgnoreCollisionBetween(this.GetComponent<Collider>(), wall.GetComponent<Collider>());
		}

		SceneUtils.FindObject<CameraShaker>().ShakeCamera(cameraShakeOnHit, true);

		SwitchState(BoomboxState.RETRACTABLE);
		
	}

	public override void DestroyWeapon() {
		if(spawnedRope) {
			Destroy(spawnedRope.gameObject);
		}
		
		Destroy(this.gameObject);
	}

}
