using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class FadeOutMusic : CutSceneComponent {

        public bool fadeOut = true;
		public float fadeSpeed = .01f;

		public bool deActivateInstantly = false;
		public FadingAudio fadingAudio;

		public override void OnActivated () {

            fadingAudio.RemoveEventListener(this.gameObject);

			if(deActivateInstantly) {
				DoFade();
				DeActivate();
			} else {
				fadingAudio.AddEventListener(this.gameObject);
                DoFade();
			}
		}

        private void DoFade() {
            if(fadeOut) {
                fadingAudio.FadeOut(fadeSpeed);
            } else {
                fadingAudio.FadeIn(fadeSpeed);
            }        
        }

		public void OnFadedOut(FadingAudio fadingAudio) {
            fadingAudio.RemoveEventListener(this.gameObject);

			if(isEnabled) {
				DeActivate();
			}
		}

        public void OnFadedIn(FadingAudio fadingAudio) {
            fadingAudio.RemoveEventListener(this.gameObject);

            if(isEnabled) {
                DeActivate();
            }
        }
	}
}