using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {

	private Blink2D blink2D;
	private SpriteRenderer overlay, outline;

	public void Awake() {
		blink2D = GetComponent<Blink2D>();
		overlay = this.transform.Find("Overlay").GetComponent<SpriteRenderer>();
		outline = this.transform.Find("Empty").GetComponent<SpriteRenderer>();
	}

	public void ShowOverlay(bool doBlink) {
		overlay.enabled = true;

		if(doBlink) {
			Blink(0, .2f);
		}
	}

	public void Blink(int amountOfTimes, float blinkTimeout) {
		blink2D.BlinkWithTimeout(amountOfTimes, blinkTimeout);
	}

	public void HideOverlay() {
		overlay.enabled = false;
	}

	public void HideAll() {
		overlay.enabled = false;
		outline.enabled = false;
	}

	public void SetColor(Color newColor) {
		Awake ();

		blink2D.SetOriginalColor(newColor);
		overlay.color = newColor;
		outline.color = newColor;
	}
}
