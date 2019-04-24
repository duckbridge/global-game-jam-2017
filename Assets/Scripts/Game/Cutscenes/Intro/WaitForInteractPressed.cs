using UnityEngine;
using System.Collections;
using InControl;

namespace Cutscenes {
	public class WaitForInteractPressed : CutSceneComponent {
	
		public SoundObject soundObjectToPlay;
		protected PlayerInputActions playerInputActions;

		public override void OnActivated () {
			playerInputActions = PlayerInputHelper.LoadData();
		}

		public override void Update () {
			if(isActivated) {
				if(playerInputActions != null && playerInputActions.interact.IsPressed) {

					if(soundObjectToPlay) {
						SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, soundObjectToPlay.gameObject);

						soundObjectToPlay.Play();
					}

					DeActivate();
				}
			}
		}
	}
}