using UnityEngine;
using System.Collections;

public class Beatnote : DispatchBehaviour {

	private float rotationSpeed = 5f;

	private float minimumRotationForBeat = 260f;
	private float maximumRotationForBeat = 280f;

	private float originalRotation;
	private bool isRotating = false;
	private Animation2D ballAnimation2D;

	void Awake() {
		ballAnimation2D = GetComponentInChildren<Animation2D>();
		originalRotation = this.transform.localEulerAngles.z;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void StartRotating() {
		isRotating = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if(isRotating) {
			this.transform.localEulerAngles += new Vector3(0f, 0f, rotationSpeed);

			if(this.transform.localEulerAngles.z > minimumRotationForBeat && this.transform.localEulerAngles.z < maximumRotationForBeat) {
				ballAnimation2D.SetCurrentFrame(1);
			} else {
				ballAnimation2D.SetCurrentFrame(0);
			}

			if(this.transform.localEulerAngles.z > maximumRotationForBeat) {
				//isRotating = false;
				//ResetBall();
				//DispatchMessage("OnBallDone", this);
			}
		}
	}

	public void ResetBall() {
		this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, originalRotation);
	}

	public bool CanDoSoundBlast() {
		if(this.transform.localEulerAngles.z > minimumRotationForBeat && this.transform.localEulerAngles.z < maximumRotationForBeat) {
			return true;
		}

		return false;
	}

	public void StopRotating() {
		isRotating = false;
		this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, originalRotation);
	}

	public void Initialize(float newRotationSpeed, float minimumRotationForBeat, float maximumRotationForBeat) {
		this.rotationSpeed = newRotationSpeed;
		this.minimumRotationForBeat = minimumRotationForBeat;
		this.maximumRotationForBeat = maximumRotationForBeat;
	}
}
