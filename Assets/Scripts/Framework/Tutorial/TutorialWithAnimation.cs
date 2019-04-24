using UnityEngine;
using System.Collections;

public class TutorialWithAnimation : MonoBehaviour {

	public SoundObject spraySound;
	public Animation2D animationToPlay, extraAnimationToPlay;

	public GameObject contentToShow;

	private float hideTimeout = 0f;
	private bool willHideAfterShowing = false;

	private bool isHidden = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show() {
		isHidden = false;

		animationToPlay.AddEventListener(this.gameObject);
		animationToPlay.Play(true);
		
		if(extraAnimationToPlay) {
			extraAnimationToPlay.Play(true);
		}

		if(spraySound) {
			SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, this.gameObject);
			spraySound.Play();
		}
	}

	public void ShowAndHideAfterTime(float time) {
		Show ();
		willHideAfterShowing = true;
		hideTimeout = time;
	}

	public void Hide() {
		contentToShow.active = false;

		animationToPlay.Hide ();
		if(extraAnimationToPlay) {
			extraAnimationToPlay.Hide ();
		}

		willHideAfterShowing = false;
		hideTimeout = 0f;

		isHidden = true;
	}

	public void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == animationToPlay.name) {
			contentToShow.active = true;

			if(willHideAfterShowing) {
				Invoke ("Hide", hideTimeout);
			}
		}
	}

	public bool IsHidden() {
		return isHidden;
	}
}
