using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunPositions : MonoBehaviour {

	public List<RunPosition> runPositions;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GrowCircleTo(int amount) {
		this.transform.localScale = new Vector3(amount, 1f, amount);
	}
	
	public int GetNextRunPositionIndex(int currentIndex) {
		if(currentIndex == -1) {
			return 0;
		} else {
			int newIndex = ++currentIndex;

			if(newIndex >= runPositions.Count) {
				newIndex = 0;
			}

			return newIndex;
		}
	}
}
