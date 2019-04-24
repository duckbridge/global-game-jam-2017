using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	public float rotationSpeed = 10f;
	public bool isRotating = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isRotating) {
			this.transform.localEulerAngles += new Vector3(0f, 0f, rotationSpeed);
		}
	}

	public void StartRotating() {
		isRotating = true;
	}
}
