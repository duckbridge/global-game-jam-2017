using UnityEngine;
using System.Collections;

public class TriggerListenerThatGrows : TriggerListener {

	public void DoGrow(float newSize) {
		float newLocalPostition = newSize * .5f;

		this.transform.localScale = new Vector3(newSize, 1f, this.transform.localScale.z);
		this.transform.localPosition = new Vector3(newLocalPostition, this.transform.localPosition.y, this.transform.localPosition.z);

	}
}
