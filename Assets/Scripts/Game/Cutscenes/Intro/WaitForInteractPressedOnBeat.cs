using UnityEngine;
using System.Collections;

namespace Cutscenes {

	public class WaitForInteractPressedOnBeat : Cutscenes.WaitForInteractPressed {

		public BeatListener beatListener;

		public override void Update () {
			if(isActivated) {
				if(playerInputActions != null && playerInputActions.interact.WasPressed) {

					if(beatListener.CanDoBeat(false)) {
						if(soundObjectToPlay) {
							soundObjectToPlay.Play();
						}
					
						DeActivate();
					}

				}
			}
		}

	}
}