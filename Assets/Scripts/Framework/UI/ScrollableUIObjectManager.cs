using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollableUIObjectManager : DispatchBehaviour {

	private bool isDragging = false;
	private Transform objectTransform;
	public Collider objectDisplayBounds;
	
	// Use this for initialization
	void Start () {
	}
	
	public void SetObjectDisplayBounds(Collider objectDisplayBounds) {
		this.objectDisplayBounds = objectDisplayBounds;
	}

	// Update is called once per frame
	void Update () {
	}

	public void Initialize() {
		this.objectTransform = this.transform.Find("Objects");
	}

	public override void Disable() {
		base.Disable();
		foreach(DragComponent dragComponent 
			in this.objectTransform.GetComponentsInChildren<DragComponent>()) {
			dragComponent.Disable();
		}
	}

	public override void Enable() {
		base.Enable();
		foreach(DragComponent dragComponent 
			in this.objectTransform.GetComponentsInChildren<DragComponent>()) {
			dragComponent.Enable();
		}
	}

	public void StopDragging() {
		this.isDragging = false;
	}

	public void MoveHorizontal(Vector2 amount) {
		Move(new Vector3(amount.x, 0));
	}

	public void MoveVertical(Vector2 amount) {
		Move(new Vector3(0, amount.y));
	}

	private void Move(Vector2 amount) {
		if(isEnabled) {
			this.isDragging = true;
			this.objectTransform.position += new Vector3(amount.x, amount.y, 0);
		}
	}
}
