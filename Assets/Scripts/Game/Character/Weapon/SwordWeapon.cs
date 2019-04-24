using UnityEngine;
using System.Collections;

public class SwordWeapon : Weapon {

	public float throwTime = .3f;

	public float retractPower = 2f;
	public float throwPower = 5f;

	protected float currentThrowPower;
	protected BoomboxContainer sprites;
	protected Vector3 throwDirection;

	protected Vector3 originalThrowDirection;
	protected Camera gameCamera;

	protected bool isFalling = false;

	public enum BoomboxState {
		ONPLAYER,
		THROWN,
		RETRACTABLE,
		RETRACTING,
	}
	
	public BoomboxState swordState = BoomboxState.ONPLAYER;

	public virtual void Awake() {
		currentThrowPower = throwPower;
		sprites = this.transform.Find("Sprites").GetComponent<BoomboxContainer>();
		gameCamera = SceneUtils.FindObject<CameraShaker>().GetComponent<Camera>();
	}

	public override void OnDirectionPressed (Direction directionToThrowIn) {

		switch(swordState) {

			case BoomboxState.ONPLAYER:
				SwitchState(BoomboxState.THROWN);
				
				savedThrowDirection = directionToThrowIn;

				if(directionToThrowIn == Direction.DOWN) {
					throwDirection = new Vector3(0, 0f, -1f);
				}

				if(directionToThrowIn == Direction.UP) {
					throwDirection = new Vector3(0, 0f, 1f);	
				}
				
				if(directionToThrowIn == Direction.LEFT) {
					throwDirection = new Vector3(-1, 0f, 0f);
				}
				
				if(directionToThrowIn == Direction.RIGHT) {
					throwDirection = new Vector3(1f, 0f, 0f);
				}

				this.transform.right = throwDirection;
				originalThrowDirection = throwDirection;

				isInAir = true;

				EnablePickedUp();
				
				sprites.ShowShadow(directionToThrowIn);

				DispatchMessage("OnWeaponThrown", this);
				
				Invoke ("OnThrowingDone", throwTime); 
			break;

			case BoomboxState.RETRACTING:
			break;
		}
	}

	public override void RetractWeapon () {
		if(swordState != BoomboxState.RETRACTING) {
			SwitchState(BoomboxState.RETRACTING);
		}
	}

	private void EnablePickedUp() {
		canBePickedUpByPlayer = true;
	}

	public override void FixedUpdate () {

		switch(swordState) {
				
			case BoomboxState.THROWN:

				this.transform.position += throwDirection * currentThrowPower;

				if(gameCamera) {
					Vector3 positionInCamera = gameCamera.WorldToViewportPoint(this.transform.position);
				
					if(positionInCamera.x > 0.99f 
				   	|| positionInCamera.x < 0.01f 
				   	|| positionInCamera.y < 0.03f 
				   	|| positionInCamera.y > 0.97f) {
						OnOutOfBounds(null);
					}
				}
			break;

			case BoomboxState.RETRACTING:
				OnRetracting();
			break;

		}

		if(isFalling) {
			if(GetSavedThrowDirection() == Direction.LEFT || GetSavedThrowDirection() == Direction.RIGHT) {
				this.transform.position += new Vector3(0f, 0f, -0.06f);
				sprites.MoveShadowsBy(0.06f);
			}
		}
	}

	private void OnRetracting() {

		isInAir = true;

		Vector3 directionToTarget = MathUtils.CalculateDirection(origin.transform.position, this.transform.position);
		
		throwDirection = directionToTarget;
		
		this.transform.position += throwDirection * currentThrowPower;
	}

	public void OnTriggerEnter(Collider coll) {

		Wall wall = coll.gameObject.GetComponent<Wall>();
		
		if(wall) {
			OnOutOfBounds(wall);
		}
		
		Player player = coll.gameObject.GetComponent<Player>();
	}

	protected virtual void OnOutOfBounds(Wall wall) {

		CancelInvoke("OnThrowingDone");

		SceneUtils.FindObject<CameraShaker>().ShakeCamera(cameraShakeOnHit, true);

		OnThrowingDone();
		     
	}

	protected void SwitchState(BoomboxState newState) {
		this.swordState = newState;

		switch(newState) {

			case BoomboxState.RETRACTING:
				CancelInvoke("OnThrowingDone");
				OnBeforeRetracting();
			break;

		}
	}

	protected virtual void OnBeforeRetracting() {
		isFalling = false;
		currentThrowPower = retractPower;
	}
	
	public override void DestroyWeapon() {
		Destroy(this.gameObject);
	}

	public override void OnThrowingDone() {
		DispatchMessage("OnWeaponThrowingDone", this);
		isInAir = false;
		swordState = BoomboxState.RETRACTABLE;
	}
	
	public override void Disable () {
		this.DisableWeapon();
	}
}
