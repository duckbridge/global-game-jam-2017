using UnityEngine;
using System.Collections;

public class BodyControl : MonoBehaviour {

	public float accelerationMoveSpeed =.2f;
	public float jumpToWeaponSpeed = .2f;
	public float moveSpeed = .2f;
	private float originalMoveSpeed = 0f;

	private Direction currentDirection;
	private bool canMove = true;

	// Use this for initialization
	void Start () {
		originalMoveSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetMoveSpeed(float newMoveSpeed) {
		if(originalMoveSpeed == 0) {
			originalMoveSpeed = moveSpeed;
		}
		moveSpeed = newMoveSpeed;
	}

	public void ResetMoveSpeed() {
		moveSpeed = originalMoveSpeed;
	}

	public void DoMoveAndTurn(float directionX, float directionZ, bool correctDirection = true) {

		DoTurn(directionX);

		DoMove(directionX, directionZ, correctDirection);
	}

	
	public void DoMoveWithAcceleration(Vector3 target) {
		
		Vector3 directionToTarget = MathUtils.CalculateDirection(target, this.transform.position);
		float calculatedMovementSpeed = Vector3.Distance(target, this.transform.position) * accelerationMoveSpeed; 
		
		this.GetComponent<Rigidbody>().velocity = (new Vector3(directionToTarget.x*.7f, 0f, directionToTarget.z*.7f) * calculatedMovementSpeed);
	}

	public void DoMoveWithMinimumSpeed(Vector3 target, float minimumDirectionSpeed) {

		Vector3 directionToTarget = MathUtils.CalculateDirection(target, this.transform.position);

		if(directionToTarget.x < minimumDirectionSpeed && directionToTarget.x > 0f) {
			directionToTarget.x = minimumDirectionSpeed;
		}

		if(directionToTarget.x > -minimumDirectionSpeed && directionToTarget.x < 0f) {
			directionToTarget.x = -minimumDirectionSpeed;
		}

		if(directionToTarget.z < minimumDirectionSpeed && directionToTarget.z > 0f) {
			directionToTarget.z = minimumDirectionSpeed;
		}
		
		if(directionToTarget.z > -minimumDirectionSpeed && directionToTarget.z < 0f) {
			directionToTarget.z = -minimumDirectionSpeed;
		}

		this.GetComponent<Rigidbody>().velocity = (new Vector3(directionToTarget.x, 0f, directionToTarget.z) * moveSpeed);

	}

	public void DoMove(float directionX, float directionZ, bool correctDirection = true) {
		if(canMove) {

			if(correctDirection) {
				directionX = CorrectDirection(directionX);
				directionZ = CorrectDirection(directionZ);
			}

			if(directionX == 0) {

				this.GetComponent<Rigidbody>().velocity = (new Vector3(0f, 0f, directionZ) * moveSpeed);
				
			} else if(directionZ == 0) {
			
				this.GetComponent<Rigidbody>().velocity = (new Vector3(directionX, 0f, 0f) * moveSpeed);
			
			} else {

				this.GetComponent<Rigidbody>().velocity = (new Vector3(directionX*.7f, 0f, directionZ*.7f) * moveSpeed);
			}

			DoTurn (directionX);
		}
	}

	public void MoveKinematic(Vector3 target) {
		Vector3 directionToTarget = MathUtils.CalculateDirection(target, this.transform.position);
		this.transform.position = (transform.position + (directionToTarget * moveSpeed));
	}

	public void MoveKinematic(float directionX, float directionZ, bool alsoTurn) {

		if(alsoTurn) {
			DoTurn(directionX);
		}

		this.transform.position += new Vector3(directionX * moveSpeed, 0f, directionZ * moveSpeed);
	}

	public void MoveKinematicWithAcceleration(Vector3 target) {

		Vector3 directionToTarget = MathUtils.CalculateDirection(target, this.transform.position);
		directionToTarget.Normalize();

		float calculatedMovementSpeed = Vector3.Distance(target, this.transform.position) * accelerationMoveSpeed; 

		this.transform.position += directionToTarget * calculatedMovementSpeed;
	}

	public void StopMoving() {
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}


	private float CorrectDirection(float direction) {
		float correctedDirection = 0f;

		if(direction > .5f) {
			correctedDirection = 1f;
		}
		
		if(direction < -.5f) { 
			correctedDirection = -1f;
		}
		return correctedDirection;

	}

	public void Push(Vector3 pushingPower, float resetMovingTimeout) {
		canMove = false;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().AddForce(pushingPower, ForceMode.Impulse);

		Invoke("ReEnableMoving", resetMovingTimeout);
	}

	public void ReEnableMoving() {
		CancelInvoke("ReEnableMoving");
		canMove = true;
	}

	public void DisableMoving() {
		canMove = false;
	}

	public Direction GetCurrentDirection() {
		return currentDirection;
	}

	public void SetCurrentDirection(Direction newDirection) {
		
		if(newDirection != currentDirection) {
			this.currentDirection = newDirection;	
		}
	}

	private void DoTurn(float directionX) {
		if(directionX < 0 && this.transform.localScale.x >= 0) {
			this.transform.localScale = new Vector3(-Mathf.Abs (this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
		}
		
		if(directionX > 0 && this.transform.localScale.x <= 0) {
			this.transform.localScale = new Vector3(Mathf.Abs (this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
		}
	}

	public bool CanMove() {
		return canMove;
	}
}
