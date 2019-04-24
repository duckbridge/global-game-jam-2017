using UnityEngine;
using System.Collections;

public class OnDieEffect : DispatchBehaviour {

	public SoundObject onDieEffectSound;
	public float fadeTime = 60f;

	public bool fadeOutSound = true;
	public bool fadeIn = false;
	public bool play2DAnimation = true;

	public Fading2D onDieFader;
	public Animation2D onDieAnimation;
	public AllSoundsFader allSoundsFader;

	private GameObject listeningGameObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowAnimationEffect(GameObject listeningGameObject) {
		this.listeningGameObject = listeningGameObject;
		this.AddEventListener (listeningGameObject);

		onDieAnimation.AddEventListener (this.gameObject);
		onDieAnimation.Play (true);

	}

	public void HideAnimationEffect() {
		onDieAnimation.Play (true, true);
	}

	public void OnAnimationDone(Animation2D animation2D) {
		DispatchMessage ("OnDieAnimationDone", null);
		onDieAnimation.RemoveEventListener (this.gameObject);
		this.RemoveEventListener (listeningGameObject);

	}

	public void StartEffect() {
		if (fadeIn) {
			onDieFader.GetComponent<SpriteRenderer>().color = Color.clear;
			onDieFader.GetComponent<SpriteRenderer>().enabled = true;

			onDieFader.FadeInto(Color.white, fadeTime);
		}

		if (play2DAnimation) {
			onDieAnimation.RemoveEventListener (this.gameObject);
			onDieAnimation.Play (true);
		}

		onDieEffectSound.Play ();

		if (fadeOutSound) {
			allSoundsFader.FadeSound ();
		}
	}
}
