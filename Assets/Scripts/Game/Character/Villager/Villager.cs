using UnityEngine;
using System.Collections;

public class Villager : InteractionObject {

    public bool playIdleAnimationAfterTalking = true;
	public TextBoxManager textManager;

	protected Player player;

	protected AnimationManager2D animationManager;

	public virtual void Start () {
		animationManager = GetComponentInChildren<AnimationManager2D>();
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			player.GetComponent<PlayerInputComponent>().enabled = false;
            player.GetComponent<BodyControl>().StopMoving();

			this.player = player;
			base.OnInteract (player);

            player.OnTalking();

			textManager.AddEventListener(this.gameObject);
			textManager.ResetShowAndActivate();
		}
	}

	public virtual void OnTextBoxDoneAndHidden() {
        player.OnTalkingDone();

		player.GetComponent<PlayerInputComponent>().enabled = true;
        if(playIdleAnimationAfterTalking) {
            if(animationManager) {
                animationManager.PlayAnimationByName("Idle", true);
            }
        }
	}

	public AnimationManager2D GetAnimationManager() {
		if(!animationManager) {
			animationManager = GetComponentInChildren<AnimationManager2D>();
		}

		return animationManager;
	}
}
