using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animation2DThatPlaysSoundEffects : Animation2D {

	public List<SoundObject> soundEffects;
	public List<int> framesToPlaySoundEffectOn;

	public override void OnFrameEntered (int enteredFrame) {
		if(framesToPlaySoundEffectOn.Contains(enteredFrame)) {
			int soundEffectIndex = framesToPlaySoundEffectOn.IndexOf(enteredFrame);
			soundEffects[soundEffectIndex].Play(true);
		}
	}
}
