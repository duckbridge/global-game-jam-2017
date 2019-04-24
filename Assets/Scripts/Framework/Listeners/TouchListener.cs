using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchListener : DispatchBehaviour {

    public List<Camera> usedCameras = new List<Camera>();
    public float minimumDragDistance = 1f;
    private Vector3 touchPoint;
    
    protected bool isTouched = false;
    public bool canBePaused = false;

    private static GameObject selectedObject;

    public void Start() {}

    public void Update() {
        if(!isTouched) {
            if (Input.GetMouseButtonDown(0)) {
                isTouched = true;
                selectedObject = null;
                RaycastHit hitSummary = RaycastObjectsInCameras(Input.mousePosition);
                this.touchPoint = Input.mousePosition;

                if (hitSummary.collider != null) {
                    selectedObject = hitSummary.collider.gameObject;
                    selectedObject.SendMessage("OnTouched", hitSummary, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        
        if(Input.GetMouseButtonUp(0)) {
            this.isTouched = false;
            
            if(selectedObject != null) {
                selectedObject.SendMessage("OnDraggingStopped", null, SendMessageOptions.DontRequireReceiver);
                DispatchMessage("OnDraggingStopped", null);
            }

            selectedObject = null;
        }

        if(isTouched && selectedObject != null) {
           OnTouchHeld();
        }
    }

    private void OnTouchHeld() {
        foreach(Camera usedCamera in usedCameras) {
			Vector3 currentTouchPosition = usedCamera.ScreenToWorldPoint(Input.mousePosition);
			Vector3 dragAmount = (currentTouchPosition - touchPoint);

            DragSummary dragSummary = new DragSummary();
            dragSummary.position = usedCamera.ScreenToWorldPoint(currentTouchPosition);
            dragSummary.cameraSource = usedCamera;
			dragSummary.direction = currentTouchPosition - selectedObject.transform.position;

            dragSummary.amount = dragAmount;

       	 	selectedObject.SendMessage("OnDrag", dragSummary, SendMessageOptions.DontRequireReceiver);

            if(Mathf.Abs(dragAmount.x) > minimumDragDistance) {
				selectedObject.SendMessage("OnHorizontalDrag", dragSummary, SendMessageOptions.DontRequireReceiver);
                DispatchMessage("OnHorizontalDrag", dragSummary);
            }

            if(Mathf.Abs(dragAmount.y) > minimumDragDistance) {
				selectedObject.SendMessage("OnVerticalDrag", dragSummary, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private RaycastHit RaycastObjectsInCameras(Vector2 screenPos) {
        RaycastHit hitInfo = new RaycastHit();

        foreach(Camera usedCamera in usedCameras) {
            hitInfo = RaycastObject(usedCamera, screenPos);
            if(hitInfo.collider != null) {
                return hitInfo;
            }
        }

        return hitInfo;
    }

    private RaycastHit RaycastObject(Camera usedCamera, Vector2 screenPos) {
        RaycastHit info;
        Physics.Raycast(usedCamera.ScreenPointToRay(screenPos), out info, 200);

        return info;
    }

    public static GameObject GetSelectedObject() {
        return selectedObject;
    }

    public override void OnPauseGame() {
        if(canBePaused) base.OnPauseGame();
    }

    public override void OnResumeGame() {
        if(canBePaused) base.OnResumeGame();
    }
}
