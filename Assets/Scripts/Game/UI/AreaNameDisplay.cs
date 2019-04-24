using UnityEngine;
using System.Collections;

public class AreaNameDisplay : MonoBehaviour {

	private string currentAreaCode;

	public void Start() {
	}

	public void ShowNameForTime(string areaCode, float time) {

		CancelInvoke("HideName");

		if(currentAreaCode != null && currentAreaCode.Length > 0) {
			HideName();
		}

		if(this.transform.Find(areaCode)) {
			currentAreaCode = areaCode;
			this.transform.Find(areaCode).GetComponent<Animation2D>().Play(true);
		}

		Invoke("HideName", time);

	}

	private void HideName() {
		this.transform.Find(currentAreaCode).GetComponent<Animation2D>().StopAndHide();
	}
}
