using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationsInOrderOnBeatObject : BeatObject {

	public Animation2D[] animations;

	private Animation2D currentAnimation;
	private int currentIndex = 0;

	public override void OnBeatEvent () {

		if (currentAnimation) {
			currentAnimation.StopAndHide ();
		}

		++currentIndex;

		if (currentIndex >= animations.Length) {
			currentIndex = 0;
		}

		currentAnimation = animations [currentIndex];
		currentAnimation.Play (true);
	}
}
