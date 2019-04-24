using UnityEngine;
using System.Collections;

public class EnableOnDrumReceived : MonoBehaviour {

	public GameObject gameObjectToEnable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrumReceived() {
		gameObjectToEnable.SetActive (true);
	}
}
