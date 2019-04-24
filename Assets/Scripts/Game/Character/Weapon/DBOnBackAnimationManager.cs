using UnityEngine;
using System.Collections;

public class DBOnBackAnimationManager : AnimationManager2D {

	public Animation2D talkAnimation;
	public WeaponOnBack weaponOnBack;

	public override void PlayAnimationByName(string animationName, bool reset = false, bool useTimeOut = false, bool forceAnimation = false) {
		if(animationName.ToLower() == talkAnimation.name.ToLower()) {
			if(weaponOnBack.GetDirection() == Direction.UP) {	
				base.PlayAnimationByName(animationName, reset, useTimeOut, forceAnimation);
			}	
		} else {	
			base.PlayAnimationByName(animationName, reset, useTimeOut, forceAnimation);
		}
	}
}
