using UnityEngine;
using System.Collections;

public class DraggableObject : Wall {

	public float dragTime = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDragged() {
		Invoke("StopDragging", dragTime);
	}

	private void StopDragging() {
		this.transform.parent = null;
	}
}
