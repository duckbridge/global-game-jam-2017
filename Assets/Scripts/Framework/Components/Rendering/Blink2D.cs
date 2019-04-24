using UnityEngine;
using System.Collections;

public class Blink2D : MonoBehaviour {

	public float blinkTime = .1f;
	public SpriteRenderer sprite;

	public Color blinkColor = Color.red;
	private Color originalColor;

	public bool usesColor = true;
	private int blinkTimesLeft = 1;

	// Use this for initialization
	void Awake () {
		if(!sprite) {
			sprite = this.GetComponent<SpriteRenderer>();
		}
		originalColor = sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetOriginalColor(Color originalColor) {
		this.originalColor = originalColor;
	}

	public void BlinkWithTimeout(int times, float newBlinkTime) {
		blinkTime = newBlinkTime;
		Blink (times);
	}

	public void Blink(int times = 1) {
		blinkTimesLeft = times;

		CancelInvoke("DoBlink");
		CancelInvoke("Show");

		DoBlink ();
	}

	public void DoBlink() {

		if(!sprite) {
			sprite = this.GetComponent<SpriteRenderer>();
		}

		if(sprite.GetComponent<Renderer>().enabled) {
			if(!usesColor) {
				sprite.GetComponent<Renderer>().enabled = false;
			} else {
				sprite.color = blinkColor;
			}
		}

		Invoke("Show", blinkTime);
	}

	public void StopBlinking() {
	
		CancelInvoke("DoBlink");
		CancelInvoke("Show");

		blinkTimesLeft = 0;

		Show ();
	}

	private void Show() {

		if(sprite.GetComponent<Renderer>().enabled) {
			if(!usesColor) {
				sprite.GetComponent<Renderer>().enabled = true;
			} else {
				sprite.color = originalColor;
			}
		}

		if(blinkTimesLeft == -1) {
			Invoke ("DoBlink", blinkTime);
		}
		
		if(blinkTimesLeft > 1) {
			Invoke ("DoBlink", blinkTime);
			blinkTimesLeft--;
		}
	}
}
