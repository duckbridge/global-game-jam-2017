using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animation2D))]
public class AnimationOnBeatObject : BeatObject {

	public bool playFullAnimation = false;
	public bool randomizeAnimationFrames = false;

	private Animation2D animation2D;
	private bool isPaused = false;

	public void Awake() {
		animation2D = GetComponent<Animation2D>();
		animation2D.Stop ();

		if(randomizeAnimationFrames) {
			RandomizeFrames();
		}
	}

	public override void OnBeatEvent () {

		if (isPaused) {
			return;
		}

		if(animation2D.GetCurrentFrame() + 1 >= animation2D.frames.Length) {
			if(randomizeAnimationFrames) {
				RandomizeFrames();
			}
		}

		if (!playFullAnimation) {
			animation2D.ShowNextFrame ();
		} else {
			animation2D.Play (true);
		}
	}

	private void RandomizeFrames() {
		List<Sprite> randomFrames = RandomHelper.ShuffleRandomly(new List<Sprite>(animation2D.frames));
		animation2D.frames = randomFrames.ToArray();
	}

	public void Pause() {
		isPaused = true;
	}

	public void Resume() {
		isPaused = false;
	}
}
