using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Animation2DThatPlaysSoundEffect : Animation2D {

	public SoundObject soundEffect;
	public List<int> framesToPlaySoundEffectOn;


	public override void OnFrameEntered (int enteredFrame) {
		if(framesToPlaySoundEffectOn.Contains(enteredFrame)) {
			soundEffect.Play(true);
		}
	}
}
