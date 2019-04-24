using UnityEngine;
using System.Collections;

public class EnemyBreakComponent : MonoBehaviour {

	public GameObject[] brokenParts;
	private int index = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BreakAll() {
		for(int i = 0 ; i < brokenParts.Length ; i++) {
			Break();
		}
	}

	public void Break() {
		if(index < brokenParts.Length) {
			brokenParts[index].transform.parent = null;
			brokenParts[index].SetActive(true);
			index ++;
		}
	}
}
