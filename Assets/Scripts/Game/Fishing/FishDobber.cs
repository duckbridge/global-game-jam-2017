using UnityEngine;
using System.Collections;

public class FishDobber : DispatchBehaviour {

	public float maxMoveSpeed = 0.15f;

	public float moveSpeedIncrementAmount = 0.01f;
	public float moveSpeedDecrementAmount = 0.02f;

	private float currentMoveSpeedRight, currentMoveSpeedLeft, currentMoveSpeedUp, currentMoveSpeedDown;

	private Bounds moveBounds;
	private Vector3 lastMoveDirection;

	private Animation2D waterAnimation, movingDobberAnimation;
	private Vector3 oldPosition;

	private enum DobberState { Moving, Still}
	private DobberState dobberState = DobberState.Still;

	void Awake() {
		waterAnimation = this.transform.Find("waterAnimation").GetComponent<Animation2D>();
		movingDobberAnimation = this.transform.Find("movingDobberAnimation").GetComponent<Animation2D>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		float newX = Mathf.Clamp(this.transform.localPosition.x + (currentMoveSpeedRight - currentMoveSpeedLeft), -moveBounds.extents.x, moveBounds.extents.x);
		float newZ = Mathf.Clamp(this.transform.localPosition.z + (currentMoveSpeedUp - currentMoveSpeedDown), -moveBounds.extents.z, moveBounds.extents.z);

		this.transform.localPosition = new Vector3(newX, 0f, newZ);
	
		lastMoveDirection = (this.transform.localPosition - oldPosition);

		this.transform.right = new Vector3(lastMoveDirection.x, 0f, lastMoveDirection.z);

		if(currentMoveSpeedRight == 0 && currentMoveSpeedLeft == 0 && currentMoveSpeedUp == 0 && currentMoveSpeedDown == 0) {
			movingDobberAnimation.StopAndHide();

			if(!waterAnimation.IsPlaying()) {
				waterAnimation.Play(true);
			}
			dobberState = DobberState.Still;

		} else {
			dobberState = DobberState.Moving;
			waterAnimation.StopAndHide();

			if(!movingDobberAnimation.IsPlaying()) {
				movingDobberAnimation.Play (true);
			}
		}

		oldPosition = this.transform.localPosition;
	}

	public void SetMoveBounds(Bounds moveBounds) {
		this.moveBounds = moveBounds;
	}

	public void OnTriggerEnter(Collider coll) {
		SwimmingFish swimmingFish = coll.gameObject.GetComponent<SwimmingFish>();
		if(swimmingFish) {
			DispatchMessage("ToggleReelingIn", true);
			DispatchMessage("SetFishThatCanBeReeledIn", swimmingFish);
		}
	}

	public void OnTriggerExit(Collider coll) {
		SwimmingFish swimmingFish = coll.gameObject.GetComponent<SwimmingFish>();
		if(swimmingFish) {
			DispatchMessage("ToggleReelingIn", false);
			DispatchMessage("UnSetFishThatCanBeReeledIn", null);
		}
	}

	public void DoMove(float directionX, float directionZ) {
	
		if(directionX == 0) {
			DecrementMoveSpeed(ref currentMoveSpeedLeft);
			DecrementMoveSpeed(ref currentMoveSpeedRight);

		}

		if(directionZ == 0) {
			DecrementMoveSpeed(ref currentMoveSpeedUp);
			DecrementMoveSpeed(ref currentMoveSpeedDown);
		}

		if(directionX > 0) {
			DecrementMoveSpeed(ref currentMoveSpeedLeft);
			IncrementMoveSpeed(ref currentMoveSpeedRight);
		}

		if(directionX < 0) {
			DecrementMoveSpeed(ref currentMoveSpeedRight);
			IncrementMoveSpeed(ref currentMoveSpeedLeft);
		} 

		if(directionZ > 0) {
			DecrementMoveSpeed(ref currentMoveSpeedDown);
			IncrementMoveSpeed(ref currentMoveSpeedUp);
		}

		if(directionZ < 0) {
			DecrementMoveSpeed(ref currentMoveSpeedUp);
			IncrementMoveSpeed(ref currentMoveSpeedDown);
		}
	}

	private void IncrementMoveSpeed(ref float moveSpeedToIncrement) {

		moveSpeedToIncrement += moveSpeedIncrementAmount;

		if(moveSpeedToIncrement > maxMoveSpeed) {
			moveSpeedToIncrement = maxMoveSpeed;
		}
	}

	private void DecrementMoveSpeed(ref float moveSpeedToDecrement) {

		moveSpeedToDecrement -= moveSpeedDecrementAmount;

		if(moveSpeedToDecrement < 0) {
			moveSpeedToDecrement = 0;
		}
	}

	public bool IsStill() {
		return dobberState == DobberState.Still;
	}
}
