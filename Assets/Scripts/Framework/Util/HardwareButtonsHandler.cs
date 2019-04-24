using UnityEngine;
using System.Collections;

public class HardwareButtonsHandler : DispatchBehaviour {

	private Loader loader;

	// Use this for initialization
	void Start () {
		loader = this.GetComponent<Loader>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){ 
			DispatchMessage("OnGameExitRequest", null);
		}
	}
}
