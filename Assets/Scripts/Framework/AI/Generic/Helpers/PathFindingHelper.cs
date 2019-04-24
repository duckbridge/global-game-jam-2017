using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFindingHelper : MonoBehaviour {

	private List<Node> closedList;
	private List<Node> openList;

	// Use this for initialization
	void Awake () {
		openList = new List<Node>();
		closedList = new List<Node>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FindPath(List<Node> nodes) {
		
	}

	public void FindGreedyPath(List<Node> nodes) {
		
	}
}
