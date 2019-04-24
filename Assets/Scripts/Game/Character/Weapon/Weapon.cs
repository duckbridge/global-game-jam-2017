using UnityEngine;
using System.Collections;

public class Weapon : DispatchBehaviour {

	public Vector2 cameraShakeOnHit = new Vector2(2f, 2f);

	public string prefabPath = "Items/";
	public string uiPrefabPath = "ItemsUI/";

	protected Direction savedThrowDirection;

	protected bool canHitEnemy = true;
	protected bool canBePickedUpByPlayer = false;
	protected bool isInAir = false;

	protected GameObject origin;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public virtual void FixedUpdate() {}

	public virtual void OnDirectionPressed(Direction throwDirection) {}

	public virtual void SetThrowTimeout(float newThrowTime) {}
	
	public virtual void DestroyWeapon() {}

	public virtual void RetractWeapon() {}

	public virtual void OnThrowingDone(){}

	public virtual void DisableWeapon() {
		canHitEnemy = false;
	}
	
	public virtual void EnableWeapon() {
		canHitEnemy = true;
	}

	public Direction GetSavedThrowDirection() {
		return savedThrowDirection;
	}

	public bool CanHitEnemy() {
		return canHitEnemy;	
	}

	public bool IsInAir() { 
		return isInAir;
	}

	public void SetOrigin(GameObject origin) {
		this.origin = origin;
	}
}
