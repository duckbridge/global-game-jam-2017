using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boomerang : DispatchBehaviour {

	public float closeDistance = .5f;

	public Vector2 onDamageKnockBack = new Vector2(3000f, 3000f);
	public bool destroyOnDone = true;

	public Transform target;
	public Transform origin;

	public float throwSpeed = 2f;
	public float arcSize = .5f;

	private float arcSizeIncrementer;
	private float arcSizeIncrementerDecrementer;

	public float boomerangChildRotationAmount = -10f;

	private Vector3 targetPosition;
	private Vector3 originPosition;

	protected enum State { FLYING, FLYINGBACK, IDLE, FALLING }
	protected State state = State.IDLE;
	protected Direction horizontalDirection;

	private float realThrowTime;
	private float flyUpTime;

	protected Transform boomerangChild;

	void Start() {
		boomerangChild = this.transform.Find("BoomerangChild");
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {

		switch(state) {
			case State.FLYING:
				BoomerangToTarget(targetPosition);
			break;

			case State.FLYINGBACK:
				BoomerangToTarget(originPosition);
			break;
		}

		if(state == State.FLYING || state == State.FLYINGBACK) {
			boomerangChild.Rotate(boomerangChild.localRotation.x, boomerangChild.localRotation.y, boomerangChild.localRotation.z + boomerangChildRotationAmount);
			this.transform.position += (this.transform.right * throwSpeed);
			realThrowTime--;
		}
	}

	public virtual void DoThrow() {
		if(state == State.IDLE) {
			this.transform.parent = null;

			boomerangChild = this.transform.Find("BoomerangChild");
			boomerangChild.GetComponent<TriggerListener>().AddEventListener(this.gameObject);

			arcSizeIncrementer = arcSize;

			targetPosition = target.position;
			originPosition = origin.position;

			realThrowTime = Vector3.Distance(targetPosition, originPosition) / throwSpeed;

			flyUpTime = realThrowTime / 2f;

			arcSizeIncrementerDecrementer = arcSizeIncrementer / flyUpTime;

			boomerangChild.localPosition = Vector3.zero;

			state = State.FLYING;
		}

		SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.gameObject);
	}

	private void BoomerangToTarget(Vector3 targetToFlyTo) {

		if(Vector3.Distance(this.transform.position, targetToFlyTo) <= closeDistance) {
			if(state == State.FLYING) {
				
				realThrowTime = Vector3.Distance(targetPosition, originPosition) /  throwSpeed;
				
				flyUpTime = realThrowTime / 2f;
				
				arcSizeIncrementer = arcSize;
				arcSizeIncrementerDecrementer = arcSizeIncrementer / flyUpTime;
				
				state = State.FLYINGBACK;
			} else {
				state = State.IDLE;
				OnDone();
			}
			return;
		}

		Vector2 boomerangDirection = targetToFlyTo - this.transform.position;
		boomerangDirection.Normalize();

		this.transform.right = boomerangDirection;

		if(realThrowTime > flyUpTime) {
			boomerangChild.localPosition = TransformUtils.ModifyY(boomerangChild, boomerangChild.localPosition.y + arcSizeIncrementer, true);
			arcSizeIncrementer -= arcSizeIncrementerDecrementer;
		} else {
			boomerangChild.localPosition = TransformUtils.ModifyY(boomerangChild, boomerangChild.localPosition.y - arcSizeIncrementer, true);
			arcSizeIncrementer += arcSizeIncrementerDecrementer;
		}
	}

	protected virtual void OnDone() {	
		DispatchMessage("OnBoomerangDone", null);

		if(destroyOnDone) {
			Destroy (this.gameObject);
		}
	}
}
