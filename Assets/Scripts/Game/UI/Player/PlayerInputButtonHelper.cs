using UnityEngine;
using System.Collections;

public class PlayerInputButtonHelper : MonoBehaviour {

	private string buttonSpriteName = "";
	private string currentButtonSpriteName = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show(string deviceName, string rawButtonName) {

		if(deviceName == "") {
			Logger.Log ("no button found... or device found...");
			return;
		}

		this.transform.Find("Interact").gameObject.SetActive(true);

		if(currentButtonSpriteName != "") {
			this.transform.Find("Interact/Buttons/" + currentButtonSpriteName).GetComponent<Animation2D>().Hide();
		}

		string buttonNameBasedOnDevice = DecideButtonNameBasedOnDevice(deviceName);
		this.transform.Find("Interact/Buttons/" + buttonNameBasedOnDevice.ToString()).GetComponent<Animation2D>().Play (true);

		currentButtonSpriteName = buttonNameBasedOnDevice;
	}

	public void Hide() {
		if(currentButtonSpriteName != "") {
			this.transform.Find("Interact/Buttons/" + currentButtonSpriteName.ToString()).GetComponent<Animation2D>().Hide();
		}
	}

	private string DecideButtonNameBasedOnDevice(string deviceName) {
		string buttonSpriteName = "";

		if(deviceName == "Keyboard") {
			buttonSpriteName = "Keyboard";
		} else {
			buttonSpriteName = "Controller";
		}

		return buttonSpriteName;
	}
}
