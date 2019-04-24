using UnityEngine;
using System.Collections;

public class PlayerManagerTestComponent : MonoBehaviour {

	public PlayerCharacterName characterToSwapTo;

	// Use this for initialization
	void Start () {
		Invoke ("SwitchToFitch", 5f);
	}

	private void SwitchToFitch() {
		GetComponent<PlayerManager>().SwapPlayer(characterToSwapTo);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
