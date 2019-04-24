using UnityEngine;
using System.Collections;

public class BeatInputHelperObject : BeatObject {
	
	private bool isEnabled = false;

	public void Awake() {
	}

	public override void Activate () {}

	public override void Deactivate () {}

	public void DoEnable() {
		if(!isEnabled) {
			this.isEnabled = true;
			if(!beatListener) {
				FindBeatListener();
			}
			
			beatListener.AddEventListener(this.gameObject);
		}
	}

	public void DoDisable() {
		if(isEnabled) {
			this.isEnabled = false;

			if(!beatListener) {
				FindBeatListener();
			}
			
			beatListener.RemoveEventListener(this.gameObject);
		}
	}

	public override void OnBeatEvent () {
		if(isEnabled) {
			string[] deviceNameAndInputName = PlayerInputHelper.DecideDeviceNameAndInputNameForInteract();

			if(deviceNameAndInputName[0] == "Keyboard") {
				this.transform.Find("Buttons/Keyboard").GetComponent<Animation2D>().Play(true);
			} else {
				this.transform.Find("Buttons/Controller").GetComponent<Animation2D>().Play(true);
			}
		}
	}
}
