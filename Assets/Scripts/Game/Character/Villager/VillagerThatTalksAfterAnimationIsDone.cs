using UnityEngine;
using System.Collections;

public class VillagerThatTalksAfterAnimationIsDone : Villager {

	public string animationNameToPlay = "Climbing";
	private bool hasPlayedAnimation = false;
	private bool isBusy = false;

	public override void OnInteract (Player player) {

		if(canInteract) {
			if(!isBusy) {
				if(!hasPlayedAnimation) {
					isBusy = true;
					GetAnimationManager().AddEventListenerTo(animationNameToPlay, this.gameObject);
					GetAnimationManager().PlayAnimationByName(animationNameToPlay);
				    
				} else {
					base.OnInteract (player);
				}
			}
		}
	}

	public virtual void OnAnimationDone(Animation2D animation2D) {
		hasPlayedAnimation = true;
		isBusy = false;
	}
}
