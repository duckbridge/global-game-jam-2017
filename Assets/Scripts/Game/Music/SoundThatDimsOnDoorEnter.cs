using UnityEngine;
using System.Collections;

public class SoundThatDimsOnDoorEnter : SoundObject {

	public float newVolumeLevel = .5f;
	private float soundLevelBeforeDim;

	public void DimSound() {
		soundLevelBeforeDim = GetVolume();
		SetVolume(newVolumeLevel * SoundUtils.GetVolume(SoundType.BG));
	}

	public void ResetSound() {
		SetVolume(soundLevelBeforeDim);
	}
}
