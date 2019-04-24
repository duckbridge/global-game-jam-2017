using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloseDoorOnCassetteRoomDisable : ActionOnCassetteRoomDisable {

	public Collider collider;
	public Animation2D animation2D;

    public override void DoAction (bool disablingAtStart) {
		base.DoAction (disablingAtStart);

		collider.enabled = false;
        
        if(disablingAtStart) {
            Animation2DThatPlaysSoundEffects soundEffectAnimation = animation2D.GetComponent<Animation2DThatPlaysSoundEffects>();
            if(soundEffectAnimation) {
                soundEffectAnimation.framesToPlaySoundEffectOn = new List<int>();
            }
        }

		animation2D.Play(true);
	}
}
