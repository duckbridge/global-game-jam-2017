using UnityEngine;
using System.Collections;

public class VillageRunPosition : MonoBehaviour {

	public VillageRunPosition[] neighbours;

	public void Start() {
	}
	
	public void Update() {
	}

	public VillageRunPosition GetRandomNeighbour() {
		VillageRunPosition neighbourToReturn = null;

		if(neighbours.Length > 0) {
			int randomIndex = Random.Range (0, neighbours.Length);
			neighbourToReturn = neighbours[randomIndex];
		}

		return neighbourToReturn;
	}
}
