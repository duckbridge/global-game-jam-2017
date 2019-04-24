using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePosition : MonoBehaviour {

	public int position = 0;
	private bool isOccupied = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetOccupied() {
		this.isOccupied = true;
	}

	public void SetUnOccupied() {
		this.isOccupied = false;
	}

	public bool IsOccupied() {
		return isOccupied;
	}

	public virtual void OnArrived(AnimalWithInputPattern animal) {}
}
