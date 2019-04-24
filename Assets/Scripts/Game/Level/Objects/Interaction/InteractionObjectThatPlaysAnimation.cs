using UnityEngine;
using System.Collections;

public class InteractionObjectThatPlaysAnimation : InteractionObject {

	public Animation2D oldAnimationToStop;
	public Animation2D animationToPlay;

	public float animationPlayTimeout = 0f;
	private bool canPlay = true;

	public override void OnInteract (Player player) {
		if(canInteract) {
			base.OnInteract (player);

			if(canPlay) {

				if(oldAnimationToStop) {
					oldAnimationToStop.StopAndHide();
				}

				GetComponent<Collider>().enabled = false;

				canPlay = false;

				animationToPlay.AddEventListener(this.gameObject);
				animationToPlay.Play(true);
		
				if(animationPlayTimeout > 0) {
					animationToPlay.RemoveEventListener(this.gameObject);
					Invoke ("ResetCanPlayAnimation", animationPlayTimeout);
				}
			}
		}
	}

	private void ResetCanPlayAnimation() {
		canPlay = true;
		GetComponent<Collider>().enabled = true;

		if(oldAnimationToStop) {
			oldAnimationToStop.Play();
		}
	}

	public void OnAnimationDone(Animation2D animation2D) {
		if(animationPlayTimeout == 0) {
			ResetCanPlayAnimation();
		}
	}
}
