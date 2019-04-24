using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldEventManager : DispatchBehaviour {

	private List<WorldEventProducer> worldEventProducers;
	public static List<WorldState> worldStates;
	public WorldState startWorldState;

	// Use this for initialization
	void Awake () {
		worldStates = new List<WorldState>();
		worldStates.Add(startWorldState);
		worldEventProducers = SceneUtils.FindObjects<WorldEventProducer>();
		
		worldEventProducers.ForEach(weProducer => weProducer.AddEventListener(this.gameObject));
	}
	
	public void OnAddWorldState(WorldState worldState) {

		if(!worldStates.Contains(worldState))
			worldStates.Add(worldState);
		OnWorldStateChanged();
	}

	public void OnRemoveWorldState(WorldState worldState) {

		if(worldStates.Contains(worldState))
			worldStates.Remove(worldState);
		OnWorldStateChanged();
	}

	public void OnWorldStateChanged() {
		DispatchMessage("OnWorldStateChanged", worldStates);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static bool Contains(WorldState worldState) {
		return worldStates.Contains(worldState);
	}
}
