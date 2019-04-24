using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawComponent : DragComponent {

	protected List<Line> lines = new List<Line>();
	protected LineDrawer lineDrawer;
	protected bool isBusy = false;
	protected LineFollowComponent lineFollowComponent;

	// Use this for initialization
	public virtual void Start () {
		lineDrawer = SceneUtils.FindObject<LineDrawer>();
		if(!lineDrawer) {
			Debug.Log("[WARN] lineDrawComponent needs LineDrawer!");
		}
		
		lineFollowComponent = this.GetComponent<LineFollowComponent>();
		lineFollowComponent.AddEventListener(this.gameObject);
		this.AddEventListener(lineFollowComponent.gameObject);

		this.canBeDragged = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ClearDrawing() {
		lines.Clear();
		lineDrawer.ClearAll();
	}

	public override void OnTouched(RaycastHit hitSummary) {
		if(isBusy) {
			lineFollowComponent.StopFollowing();
		}
		lineDrawer.StartDrawingFrom(this.gameObject);
		isBusy = true;
	}

	public override void OnDrag(DragSummary dragSummary) {
		if(usesTopDownView) {
			lineDrawer.AddLine(new Vector3(dragSummary.position.x, this.transform.position.y, dragSummary.position.z));
		} else { 
			lineDrawer.AddLine(new Vector3(dragSummary.position.x, dragSummary.position.y, this.transform.position.z));
		}
	}

	public override void OnDraggingStopped() {
		lines = lineDrawer.GetFullLine();
		lineDrawer.StopDrawing();
		DispatchMessage("OnLineDrawn", lines);
	} 

	public void OnStartFollowing() {
		DispatchMessage("StartFollowing", lines); //leave for now
	}
	
	public void OnStoppedFollowing() {
		lines.Clear();
		isBusy = false;
	}

	public void OnLinePassed() {
		lineDrawer.OnLinePassed();
	}

	public List<Line> GetFullLine() {
		return this.lines;
	}
}
