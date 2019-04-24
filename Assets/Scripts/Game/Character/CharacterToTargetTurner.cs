using UnityEngine;
using System.Collections;

public class CharacterToTargetTurner : MonoBehaviour {

	public Transform target;
	private AnimationControl animationControl;
	protected BodyControl bodyControl;

	// Use this for initialization
	void Awake () {
		bodyControl = GetComponent<BodyControl>();
		animationControl = GetComponentInChildren<AnimationControl>();
		animationControl.Initialize(bodyControl);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetTarget(Transform target) {
		this.target = target;
	}

	void FixedUpdate() {

		if(animationControl && target) {
			OnUpdate();
		}
	}

	public virtual void OnUpdate() {
		Vector3 directionToTarget = MathUtils.CalculateDirection(target.transform.position, this.transform.position);
		
		bool isLookingUpDown = false;
		
		if(Mathf.Abs(directionToTarget.x) < Mathf.Abs(directionToTarget.z)) {
			isLookingUpDown = true;
		}
		
		if(isLookingUpDown) {
			if(directionToTarget.z > 0) {
				bodyControl.SetCurrentDirection(Direction.UP);
			} else {
				bodyControl.SetCurrentDirection(Direction.DOWN);
			}
		} else {
			if(directionToTarget.x > 0) {
				bodyControl.SetCurrentDirection(Direction.RIGHT);
			} else if(directionToTarget.x < 0) {
				bodyControl.SetCurrentDirection(Direction.LEFT);
			} 
		}
	}
}
