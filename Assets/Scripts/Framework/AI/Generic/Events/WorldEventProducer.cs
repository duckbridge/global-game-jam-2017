using UnityEngine;
using System.Collections;

public class WorldEventProducer : DispatchBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnWorldStateChanged(WorldState newWorldState) {
		Debug.Log("[WARN] temporary disabled");
		//DispatchMessage("OnWorldStateChanged", newWorldState);
	}

	public void OnAddWorldState(WorldState newWorldState) {
		DispatchMessage("OnAddWorldState", newWorldState);
	}

	public void OnRemoveWorldState(WorldState newWorldState) {
		DispatchMessage("OnRemoveWorldState", newWorldState);
	}
}
