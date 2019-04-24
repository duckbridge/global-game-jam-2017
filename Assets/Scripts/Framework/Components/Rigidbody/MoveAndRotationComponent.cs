using UnityEngine;
using System.Collections;

public class MoveAndRotationComponent : DispatchBehaviour {

	private bool isRotating = false;
	private float currentAngle = 0f;
	private float destinationAngle = 0f;
	
	public float movementSpeed = 1f;
	public float rotationSpeed = 1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(isRotating) {
			currentAngle = Mathf.Lerp(currentAngle, destinationAngle, rotationSpeed);
			this.transform.eulerAngles = new Vector3(0, currentAngle, 0);
			if(currentAngle == destinationAngle) { isRotating = false; }	
		}
	}

	public void MoveInDirection(Vector3 direction) {
		this.transform.position += direction * (movementSpeed * Time.deltaTime);	
	}

	public void Move() {
		this.transform.position += this.transform.forward * (movementSpeed * Time.deltaTime);	
	}

	public void MoveTowards(Vector3 target) {
		Vector3 direction = MathUtils.CalculateDirection(this.transform.position, target);
		MoveInDirection(direction);
	}

	public void Rotate(Vector3 direction) {
		destinationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		DoRotation();
		this.isRotating = true;
	}

	public void LookAt(Vector3 target) {
		this.transform.LookAt(target);
	}

	public void MoveITween(Vector3 targetPosition) {
		iTween.MoveTo(this.gameObject, 
			iTween.Hash("name", "moveToTarget", "position", targetPosition, "speed", movementSpeed, "easetype", "linear")
		);
	}

	public void StopMovingITween() {
		iTween.StopByName(this.gameObject, "moveToTarget");
	}

	private void DoRotation() {
		float difference = destinationAngle - currentAngle;
		if (difference > 180) currentAngle += 360;
		if (difference < -180) currentAngle -= 360;

		this.isRotating = true;
	}

	public bool IsRotating() {
		return this.isRotating;
	}
}
