using UnityEngine;
using System.Collections;

public class PathFindNode : MonoBehaviour {

	public PathFindNode[] neighbours;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string ToString ()
	{
		return "PathFindNode:" + this.name + " || " + this.transform.position;
	}
}
