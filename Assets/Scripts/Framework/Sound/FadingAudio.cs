using UnityEngine;
using System.Collections;

public class FadingAudio : SoundObject {
	
	private AudioSource audio;

	private bool isFadingOut = false;
	private bool isFadingIn = false;
	private float fadeSpeed = 0f;

	private float maxVolume = 1f;

	// Use this for initialization
	public override void Awake () {
		base.Awake ();
		audio = this.GetComponent<AudioSource>();

		if(soundType == SoundType.FX) {
			maxVolume = 1f;
		} else {
			maxVolume = 1f;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isFadingOut) {
			audio.volume -= fadeSpeed;
			if(audio.volume <= 0f) {
				audio.volume = 0f;
				isFadingOut = false;

				DispatchMessage("OnFadedOut", this);

				Stop();
			}
		}

		if(isFadingIn) {
			audio.volume += fadeSpeed;

			if(audio.volume >= maxVolume) {
				audio.volume = maxVolume;
				isFadingIn = false;
				DispatchMessage("OnFadedIn", this);
			}
		}
	}

	public void PlayAtTime(float time) {
		audio.time = time;
		audio.Play();
	}

	public void FadeOut(float speed) {
		if(!isMuted) {
			isFadingOut = true;
			fadeSpeed = speed;
		}
	}

	public void FadeIn(float speed) {
		if(!isMuted) {
			audio.volume = 0f;
			isFadingIn = true;
			fadeSpeed = speed;
		}
	}

	public float GetCurrentPlayTime() {
		return audio.time;
	}

	public void Stop() {
		audio.Stop();
	}

	public void Play() {
		if(audio == null) {
			audio = this.GetComponent<AudioSource>();
		}

		audio.Play();
	}
}
