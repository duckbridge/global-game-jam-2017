using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class DispatchBehaviour : MonoBehaviour {
	
	private List<GameObject> listeninggameObjects = new List<GameObject>();
	protected bool isEnabled = true;

	public void DispatchMessage(string message, object parameter) {
		if(listeninggameObjects != null) {
			for(int i = 0 ; i < listeninggameObjects.Count ; i++) {
				if(listeninggameObjects[i]) {
					listeninggameObjects[i].SendMessage(message, parameter, SendMessageOptions.DontRequireReceiver);
				}
			}
		}	
	}
	
	public void AddEventListener(GameObject go) {
		if(go != null && !listeninggameObjects.Contains(go)) {
			listeninggameObjects.Add(go);
		}
	}

	public void AddTwoWayEventListener(DispatchBehaviour dispatchingBehaviour) {
		this.AddEventListener(dispatchingBehaviour.gameObject);
		dispatchingBehaviour.AddEventListener(this.gameObject);
	}
	
	public void RemoveTwoWayEventListener(DispatchBehaviour dispatchingBehaviour) {
		this.RemoveEventListener(dispatchingBehaviour.gameObject);
		dispatchingBehaviour.RemoveEventListener(this.gameObject);
	}

	public void RemoveEventListener(GameObject go) {
		listeninggameObjects.Remove (go);
	}

	public virtual void OnPauseGame() {
		this.enabled = false;
	}

	public virtual void OnResumeGame() {
		this.enabled = true;
	}

	public virtual void Enable() {
		this.isEnabled = true;
	}

	public virtual void Disable() {
		this.isEnabled = false;
	}

	public List<GameObject> GetListeners() {
		return this.listeninggameObjects;
	}
}
