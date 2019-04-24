using UnityEngine;
using System.Collections;
using InControl;

public class ObjectThatRepondsToKeyboardOrController : MonoBehaviour {

    private bool isUsingController = false;

	// Use this for initialization
	void Start () {
        if(ControllerHelper.IsXboxControllerPluggedIn()) {	
            OnXboxControllerPluggedIn();
        } else {
            OnXboxControllerUnPlugged();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {     

        if(isUsingController) {
          if(!ControllerHelper.IsXboxControllerPluggedIn()) {  
                OnXboxControllerUnPlugged();
            }  
        } else {
            if(ControllerHelper.IsXboxControllerPluggedIn()) {  
                OnXboxControllerPluggedIn();
            }
        }
    }

    protected virtual void OnXboxControllerPluggedIn() {
        isUsingController = true;
    }

    protected virtual void OnXboxControllerUnPlugged() {
        isUsingController = false;
    }
}
