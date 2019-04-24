using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldEventHandler : DispatchBehaviour {

	// Use this for initialization
	void Start () {
		SceneUtils.FindObject<WorldEventManager>().AddEventListener(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UnSubScribe() {
		SceneUtils.FindObject<WorldEventManager>().RemoveEventListener(this.gameObject);
	}

	public void OnWorldStateChanged(List<WorldState> newWorldState) {
		DispatchMessage("OnWorldStateChanged", newWorldState);
	}
}
