using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class FadeOutSprite : CutSceneComponent {

		public Color colorToFadeInto = Color.clear;
		public Fading2D fadingSprite;
		public float fadeOutTime = 60f;

		public override void OnActivated () {
			fadingSprite.GetComponent<SpriteRenderer>().enabled = true;
			fadingSprite.FadeInto(colorToFadeInto, fadeOutTime, FadeType.DEFAULT);
			fadingSprite.AddEventListener(this.gameObject);
		}

		public void OnFadingDone(FadeType fadeType) {
			fadingSprite.RemoveEventListener(this.gameObject);

			if(isEnabled) {
				DeActivate();
			}
		}
	}
}