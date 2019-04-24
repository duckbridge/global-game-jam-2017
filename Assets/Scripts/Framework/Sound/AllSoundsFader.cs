using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllSoundsFader : MonoBehaviour {

	public float fadeAmount = -.1f;
	public float fadeTimeout = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void FadeSound() {
		List<SoundObject> allSounds = SceneUtils.FindObjects<SoundObject> ();
		foreach (SoundObject soundObject in allSounds) {
			float soundVolume = soundObject.GetVolume ();
			soundObject.SetVolume (soundVolume + fadeAmount);
		}

		bool allSoundsFaded = allSounds.TrueForAll (soundObject => soundObject.GetVolume () < .01f);
		if (!allSoundsFaded) {
			Invoke ("FadeSound", fadeTimeout);
		}
	}

	public void StopFade() {
		CancelInvoke("FadeSound");
	}
}
