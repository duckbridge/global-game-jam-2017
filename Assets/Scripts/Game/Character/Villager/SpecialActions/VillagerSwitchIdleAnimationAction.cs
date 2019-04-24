using UnityEngine;
using System.Collections;

public class VillagerSwitchIdleAnimationAction : VillagerSpecialAction {

	public Animation2D newIdleAnimation;
	public SoundObject soundObjectOnSwitch;

	private bool hasSwitchedAnimation = false;

	public override void DoAction (Villager villager) {

		if (!hasSwitchedAnimation) {
			hasSwitchedAnimation = true;

			if (soundObjectOnSwitch) {
				soundObjectOnSwitch.Play (true);
			}

			Animation2D idleAnimation = villager.GetAnimationManager ().GetAnimationByName ("Idle");
			villager.GetAnimationManager ().StopAnimationByName ("Idle");
			idleAnimation.frames = newIdleAnimation.frames;
			villager.GetAnimationManager ().SetFrameForAnimation ("Idle", 0, true);
			idleAnimation.GetComponent<AnimationOnBeatObject> ().Activate ();
		}
	}
}
