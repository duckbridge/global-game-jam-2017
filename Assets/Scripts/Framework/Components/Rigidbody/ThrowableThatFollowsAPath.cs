using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrowableThatFollowsAPath : DispatchBehaviour {

	public float closeDistance = 1f;
	public float throwSpeed = 2f;
	public float throwableChildRotationAmount = -10f;

	protected Transform throwableChild;

	private enum State { FLYING, IDLE, DONE }
	private State state = State.IDLE;

	private List<Vector3> path;

	// Use this for initialization
	void Awake () {
		throwableChild = this.transform.Find("ThrowableChild");
		throwableChild.GetComponent<TriggerListener>().AddEventListener(this.gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(state == State.FLYING) {
			if(path.Count > 0) {
				Vector3 currentTarget = path[0];

				if(Vector2.Distance(this.transform.position, currentTarget) <= closeDistance) {
					DispatchMessage("OnReachedPathTarget", null);
					path.RemoveAt(0);
				} else {

					Vector3 throwDirection = currentTarget - this.transform.position;

					throwDirection.Normalize();
					this.transform.right = throwDirection;

					throwableChild.Rotate(throwableChild.localRotation.x, throwableChild.localRotation.y, throwableChild.localRotation.z + throwableChildRotationAmount);
					this.transform.position += (this.transform.right * throwSpeed);
				}

			} else {
				DispatchMessage("OnReachedEndTarget", null);
				Destroy (this.gameObject);
			}
		}
	}

	public void DoThrow(Vector3[] throwPath) {
		path = new List<Vector3>();
		for(int i = 0 ; i < throwPath.Length ; i++) {
			path.Add (throwPath[i]);
		}
		state = State.FLYING;
	}

	public void DoThrow(Transform[] throwPath) {
		path = new List<Vector3>();
		for(int i = 0 ; i < throwPath.Length ; i++) {
			path.Add (throwPath[i].position);
		}

		state = State.FLYING;
	}
}

