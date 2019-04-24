using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationHelper {

	private static Animation currentAnimation;

	public static void PlayAnimation(Animation animation) {
		if(animation && !animation.isPlaying) {
			currentAnimation = animation;
			animation.Play();
		}
	}

	public static void PauseAnimation(Animation animation) {
		//animation.speed = 0f;
	}

	public static void ResumeAnimation(Animation animation) {
		//animation.speed = 1f;
	}

	private static void StopCurrentAnimation() {
		currentAnimation.Stop();
	}
}
