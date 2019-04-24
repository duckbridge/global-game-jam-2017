using UnityEngine;
using System.Collections;

public class UIElement : DispatchBehaviour {
	
	public Transform hideTransform;
	public Transform showTransform;

	public bool isShown = true;
	public float hideShowTime;

	void Start () {}

	// Update is called once per frame
	void Update () {
	
	}

	public virtual void Hide() {
		if(isShown && this.hideTransform) {
			iTween.StopByName(this.gameObject, "Showing");

			iTween.MoveTo(this.gameObject, new ITweenBuilder().SetName("Hiding").SetPosition(hideTransform.position).SetTime(hideShowTime).Build());
			this.isEnabled = false;
			isShown = false;
		}
	}

    public virtual void HideInstant() {
        iTween.StopByName(this.gameObject, "Showing");
        iTween.StopByName(this.gameObject, "Hiding");

        if(hideTransform) {
            this.transform.position = hideTransform.position;
        }

        this.isEnabled = false;
        isShown = false;
    }

	public virtual void Show(bool force = false) {
		if((!isShown || force) && this.showTransform) {
			isShown = true;

			iTween.StopByName(this.gameObject, "Hiding");
			iTween.MoveTo(this.gameObject, new ITweenBuilder().SetName("Showing").SetPosition(showTransform.position).SetTime(hideShowTime).Build());

			this.isEnabled = true;
		}
	}

}
