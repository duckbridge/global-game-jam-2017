using UnityEngine;
using System.Collections;

public class SwimmingFish : MonoBehaviour {

	public float farFromDobberDistance = 2f;

	public int buttonBashesRequired = 5;
	public GameObject lootOnFish;

	public float turningSpeed = 0.15f;
	public float newDirectionTimeout = 1f;
	public float moveSpeed = 1f;
	private Bounds moveBounds;

	private Vector2 moveDirection;

	private enum FishStates { SwimmingRandom, SwimmingToDobber, Struggling, SwimmingAway }
	private FishStates fishState;
	private Transform swimTarget;

	private AnimationManager2D animationManager;
	private TriggerListener triggerListener;

	// Use this for initialization
	void Start () {
		triggerListener = GetComponentInChildren<TriggerListener>();
		triggerListener.AddEventListener(this.gameObject);

		animationManager = GetComponentInChildren<AnimationManager2D>();
		SwitchFishState(FishStates.SwimmingRandom);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		switch(fishState) {

			case FishStates.SwimmingRandom:
				DoMoveRandom();
			break;

			case FishStates.SwimmingAway:
				DoMoveAwayFromTarget();
			break;

			case FishStates.SwimmingToDobber:
				DoMoveToTarget();
			break;

		}
	}

	public void SetMoveBounds(Bounds moveBounds) {
		this.moveBounds = moveBounds;
	}

	public void DoMoveRandom() {
		DoMove(moveDirection.x, moveDirection.y);
	}

	private void DoMoveAwayFromTarget() {
		if(swimTarget) {

			Vector3 directionAwayFromTarget = (this.transform.localPosition - swimTarget.localPosition );
			DoMove(directionAwayFromTarget.x, directionAwayFromTarget.z);

			if(Vector3.Distance(swimTarget.localPosition, this.transform.localPosition) > farFromDobberDistance) {
				SwitchFishState(FishStates.SwimmingRandom);
			}
		} else {
			SwitchFishState(FishStates.SwimmingRandom);
		}
	}

	private void DoMoveToTarget() {
		if(swimTarget) {
			Vector3 directionToTarget = (swimTarget.localPosition - this.transform.localPosition);
			DoMove(directionToTarget.x, directionToTarget.z);
		} else {
			SwitchFishState(FishStates.SwimmingRandom);
		}
	}

	private void DoMove(float moveDirectionX, float moveDirectionZ) {

		float lerpedDirectionX = Mathf.Lerp(this.transform.right.x, moveDirectionX, turningSpeed);
		float lerpedDirectionZ = Mathf.Lerp(this.transform.right.z, moveDirectionZ, turningSpeed);

		float newX = Mathf.Clamp(this.transform.localPosition.x + lerpedDirectionX * moveSpeed, -moveBounds.extents.x + 0.1f, moveBounds.extents.x - 0.1f);
		float newZ = Mathf.Clamp(this.transform.localPosition.z + lerpedDirectionZ * moveSpeed, -moveBounds.extents.z + 0.1f, moveBounds.extents.z - 0.1f);
		
		this.transform.localPosition = new Vector3(newX, 0f, newZ);
		this.transform.right = new Vector3(lerpedDirectionX, 0f, lerpedDirectionZ);
	}

	private void PickNewDirection() {
		if(this.fishState == FishStates.SwimmingRandom) {
			Vector2 randomMoveDirection = new Vector2(Random.Range (-1f, 1.1f), Random.Range (-1f, 1.1f));
			if(randomMoveDirection.x == 0 && randomMoveDirection.y == 0) {
				randomMoveDirection = new Vector2(.5f, 0);
			}

			moveDirection = new Vector2(randomMoveDirection.x, randomMoveDirection.y);

			Invoke ("PickNewDirection", newDirectionTimeout);
		}

	}

	public void OnPlayerFailedToCatchFish(Transform targetToSwimAwayFrom) {
		this.swimTarget = targetToSwimAwayFrom;
		if(fishState != FishStates.SwimmingAway) {
			SwitchFishState(FishStates.SwimmingAway);
		}
	}

	public void OnPlayerAttemptToCatchFish() {
		if(fishState != FishStates.Struggling) {
			SwitchFishState(FishStates.Struggling);
		}
	}

	public void OnListenerTriggerStay(Collider coll) {
		FishDobber fishDobber = coll.gameObject.GetComponent<FishDobber>();
		if(fishDobber) {
			if(fishState == FishStates.SwimmingRandom) {
				if(fishDobber.IsStill()) {
					this.swimTarget = fishDobber.transform;
					SwitchFishState(FishStates.SwimmingToDobber);
				}
			}

			if(fishState == FishStates.SwimmingToDobber) {
				if(!fishDobber.IsStill()) {
					this.swimTarget = fishDobber.transform;
					SwitchFishState(FishStates.SwimmingAway);
				}
			}
		}
	}

	public void OnListenerTriggerExit(Collider coll) {
		FishDobber fishDobber = coll.gameObject.GetComponent<FishDobber>();
		if(fishDobber) {
			SwitchFishState(FishStates.SwimmingRandom);
		}
	}

	public void OnCaught(Player player) {
		if(lootOnFish) {

			if(lootOnFish.GetComponent<GamePickup>()) {
				player.GetComponent<PlayerPickupComponent>().OnPlayableGamePickedUp(lootOnFish.GetComponent<GamePickup>());
			}

			if(lootOnFish.GetComponent<CassettePickup>()) {
				player.GetComponent<PlayerPickupComponent>().OnCassettePickupPickedUp(lootOnFish.GetComponent<CassettePickup>());
			}

			if(lootOnFish.GetComponent<HeartDrop>()) {
				player.OnHealthPickedUp(lootOnFish.GetComponent<HeartDrop>());
			}

			if(lootOnFish.GetComponent<CandyDrop>()) {
				player.OnCandyPickedup(lootOnFish.GetComponent<CandyDrop>());
			}
		}
	}

	private void SwitchFishState(FishStates fishState) {
		Logger.Log ("Fish switching to " + fishState);
		this.fishState = fishState;

		switch(this.fishState) {
			case FishStates.SwimmingRandom:
				animationManager.PlayAnimationByName("Swimming", true);
				PickNewDirection();
			break;

			case FishStates.SwimmingAway:
			case FishStates.SwimmingToDobber:
				animationManager.PlayAnimationByName("Swimming", true);
			break;

			case FishStates.Struggling:
				
				if(swimTarget) {
					Vector3 directionToTarget = (swimTarget.localPosition - this.transform.localPosition);
					this.transform.right = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
				}

				animationManager.PlayAnimationByName("Struggling", true);
			break;

		}
	}
}
