using UnityEngine;
using System.Collections;

public class UIPositioner : MonoBehaviour {

	public enum Corner { TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT, LEFT, RIGHT }
	public Corner corner;
	public Camera usedCamera;

	// Use this for initialization
	public virtual void Start() {
		SetPosition();
	}

	public void SetPosition() {
		Vector3 cornerPosition = Vector3.zero;
		
		switch(corner) {
			
		case Corner.TOPLEFT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(0, 1, 0));
			break;
			
		case Corner.TOPRIGHT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
			break;
			
		case Corner.BOTTOMLEFT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
			break;
			
		case Corner.BOTTOMRIGHT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));
			break;
			
		case Corner.LEFT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(0, 0, this.transform.position.z));
			break;
			
		case Corner.RIGHT:
			cornerPosition = usedCamera.ViewportToWorldPoint(new Vector3(1, 0, this.transform.position.z));
			break;
		}
		
		if(corner == Corner.LEFT || corner == Corner.RIGHT) {
			this.transform.position = new Vector3(cornerPosition.x, this.transform.position.y, this.transform.position.z);
		} else {
			this.transform.position = new Vector3(cornerPosition.x, cornerPosition.y, this.transform.position.z);
		}
	}
}
