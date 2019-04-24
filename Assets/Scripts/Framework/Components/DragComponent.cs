using UnityEngine;
using System.Collections;

public class DragComponent : DispatchBehaviour {

	public bool canBeDragged = true;
	public bool usesTopDownView = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
	
	}
	
	public virtual Renderer GetRenderer() {
		return this.GetComponent<Renderer>();
	}
	
	public virtual void OnTouched(RaycastHit hitSummary) {

	}

	public virtual void OnDrag(DragSummary dragSummary) {

		if(this.canBeDragged) {
			if(usesTopDownView) {
				this.transform.position = new Vector3(dragSummary.position.x, this.transform.position.y, dragSummary.position.z);
			} else { 
				this.transform.position = new Vector3(dragSummary.position.x, dragSummary.position.y, this.transform.position.z);
			}
		}
	}

	public virtual void OnDraggingStopped() {

	}

	public override void Enable() {
		base.Enable();
		GetRenderer().enabled = true;
		canBeDragged = true;
		GetComponent<Collider>().enabled =true;
	}

	public override void Disable() {
		base.Enable();
		GetRenderer().enabled = false;
		canBeDragged = false;
		GetComponent<Collider>().enabled = false;
	}
}
