using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGameObjectActivator : MonoBehaviour {

	public List<GameObject> gameObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateRandom() {
		int randomNumber = Random.Range (0, gameObjects.Count);
		gameObjects [randomNumber].SetActive (true);
	}
}
