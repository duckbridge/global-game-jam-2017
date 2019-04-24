using UnityEngine;
using System.Collections;

public class WallUnstucker : MonoBehaviour {

	public float raycastLength = 2f;

	public Transform leftRaycastSource, rightRaycastSource, upRaycastSource, downRaycastSource;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	public void CheckForWall(Direction moveDirection) {

		switch(moveDirection) {

			case Direction.LEFT:
				AssertIfWallIsInFrontOf(leftRaycastSource, new Vector3(-1f, 0f));
				AssertIfWallIsInFrontOf(upRaycastSource, new Vector3(0f, 0f, 1f));
				AssertIfWallIsInFrontOf(downRaycastSource, new Vector3(0f, 0f, -1f));
			break;

			case Direction.RIGHT:
				AssertIfWallIsInFrontOf(rightRaycastSource, new Vector3(1f, 0f));
				AssertIfWallIsInFrontOf(upRaycastSource, new Vector3(0f, 0f, 1f));
				AssertIfWallIsInFrontOf(downRaycastSource, new Vector3(0f, 0f, -1f));
			break;

			case Direction.UP:
				AssertIfWallIsInFrontOf(upRaycastSource, new Vector3(0f, 0f, 1f));
				AssertIfWallIsInFrontOf(rightRaycastSource, new Vector3(1f, 0f));
				AssertIfWallIsInFrontOf(leftRaycastSource, new Vector3(-1f, 0f));
			break;

			case Direction.DOWN:
				AssertIfWallIsInFrontOf(downRaycastSource, new Vector3(0f, 0f, -1f));
				AssertIfWallIsInFrontOf(rightRaycastSource, new Vector3(1f, 0f));
				AssertIfWallIsInFrontOf(leftRaycastSource, new Vector3(-1f, 0f));
			break;

		}
	}

	private bool AssertIfWallIsInFrontOf(Transform usedTransform, Vector3 direction) {
		bool isObjectinFrontOfBody = false;
		
		RaycastHit raycastHit;
		
		if(usedTransform) {
			Physics.Raycast(usedTransform.position, direction, out raycastHit, raycastLength);
			Debug.DrawRay(usedTransform.position, direction * raycastLength);
			
			if(raycastHit.collider) {
				if(raycastHit.collider.gameObject.GetComponent<Wall>()) {
					isObjectinFrontOfBody = true;
					OnObjectInFrontOfBody(direction);
				}
			}
		}
		
		return isObjectinFrontOfBody;
	}

	private void OnObjectInFrontOfBody(Vector3 direction) {
		Vector3 newVelocity = GetComponent<Rigidbody>().velocity;

		if((direction.x > 0 && newVelocity.x > 0) || (direction.x < 0 && newVelocity.x < 0)) {
			newVelocity.x = 0f;	
		}
		
		if((direction.z > 0 && newVelocity.z > 0) || (direction.z < 0 && newVelocity.z < 0)) {
			newVelocity.z = 0f;	
		}

		GetComponent<Rigidbody>().velocity = newVelocity;
	}
}
