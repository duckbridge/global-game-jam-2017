using UnityEngine;
using System.Collections;

public class VillagerRobotSuicideAction : VillagerSpecialAction {

	public float moveSpeed = .3f;
	public Transform moveTarget;

	public Animation2D doorCloseAndSawAnimation;
	public Animation2D doorOpenAnimation;

    public override void DoAction(Villager villager) {
		this.GetComponent<Collider>().enabled = false;
		villager.DisableInteraction(SceneUtils.FindObject<Player>());

		villager.GetAnimationManager().PlayAnimationByName("Walking", true);

		doorOpenAnimation.Play(true);

		iTween.MoveTo(this.gameObject,
		              new ITweenBuilder().SetPosition(moveTarget.position).SetSpeed(moveSpeed)
		              .SetOnCompleteTarget(this.gameObject).SetOnComplete("OnDoneWithAction").SetEaseType(iTween.EaseType.linear).Build());
	}

	private void OnDoneWithAction() {
		doorCloseAndSawAnimation.Play();
		this.gameObject.SetActive(false);
	}
}
