using UnityEngine;
using System.Collections;

public class CollectionItemDisplay : TimeScaleIndependentUpdate {

    public float hideTimeout = 2f;

	protected bool isShown = false;

	private float waitTime = 20f;
    private bool isWaitingForHide = false;

    public virtual void Show(CollectionItemButton collectionItemButton) {
		isShown = true;
		waitTime = hideTimeout * 60;
       	isWaitingForHide = true;
    }

    public override void OnUpdate() {
        if(isWaitingForHide) {
            --waitTime;
            
            if(waitTime <= 0) {
                isWaitingForHide = false;
                waitTime = hideTimeout * 60;
				Hide ();
            }
        }
    }

	public void Hide() {
		isShown = false;
		this.gameObject.SetActive (false);
	}

	public bool IsShown() {
		return isShown;
	}
}
