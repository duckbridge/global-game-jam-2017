using UnityEngine;
using System.Collections;

public class ScrollableUI : UIElement {

	private ScrollableUIObjectManager scrollableUIObjectManager;
	public float scrollSpeed =  .1f;
	private bool canBeScrolled = false;

	public enum ScrollType { HORIZONTAL, VERTICAL };
	public ScrollType scrollType = ScrollType.HORIZONTAL;
	
	// Use this for initialization
	void Start () {
		this.scrollableUIObjectManager = this.GetComponentInChildren<ScrollableUIObjectManager>();
		this.scrollableUIObjectManager.SetObjectDisplayBounds(this.GetComponent<Collider>());
		this.scrollableUIObjectManager.Initialize();
	}

	public void OnObjectClicked(RaycastHit hitSummary) {
		canBeScrolled = true;
	}

	public void OnDraggingStopped() {
		this.scrollableUIObjectManager.StopDragging();
		canBeScrolled = false;
	}

	public void OnHorizontalDrag(DragSummary dragSummary) {
		if(dragSummary.cameraSource.GetComponent<CameraTypes>().cameraType == CameraTypes.CameraType.UICAMERA) {
			if(scrollType == ScrollType.HORIZONTAL && canBeScrolled)
				this.scrollableUIObjectManager.MoveHorizontal(dragSummary.amount * scrollSpeed);
		}
	}

	public void OnVerticalDrag(DragSummary dragSummary) {
		if(dragSummary.cameraSource.GetComponent<CameraTypes>().cameraType == CameraTypes.CameraType.UICAMERA) {
			if(scrollType == ScrollType.VERTICAL && canBeScrolled)
				this.scrollableUIObjectManager.MoveVertical(dragSummary.amount * scrollSpeed);
		}
	}

}
