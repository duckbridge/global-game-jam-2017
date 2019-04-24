using UnityEngine;
using System.Collections;

public class VillagerThatMovesAround : VillagerThatDoesSpecialActionAfterTalking {

	public float moveSpeed = .5f; 
	public Transform[] movePositions;
	private int currentIndexToMoveTo = 0;

	public override void Start () {
		base.Start ();
		if (GetComponent<VillagerSpecialAction> ()) {
			GetComponent<VillagerSpecialAction> ().AddEventListener (this.gameObject);
		}
		MoveToTarget ();
	}

	public override void OnTextBoxDoneAndHidden() {
		if (GetComponent<VillagerSpecialAction> ()) {
			GetComponent<VillagerSpecialAction> ().DoAction (this);
		} else {
			player.OnTalkingDone();
			player.GetComponent<PlayerInputComponent>().enabled = true;
			OnVillageActionDone ();
		}
	}

	public override void OnInteract (Player player) {
		iTween.StopByName (this.gameObject, "Moving");
		base.OnInteract (player);
	}

	public void OnVillageActionDone() {
		player.OnTalkingDone();
		player.GetComponent<PlayerInputComponent>().enabled = true;

		MoveToTarget ();
	}

	private void MoveToTarget() {

		animationManager.PlayAnimationByName ("Walking", true);

		iTween.MoveTo (this.gameObject, new ITweenBuilder ()
			.SetName("Moving")
			.SetPosition (movePositions [currentIndexToMoveTo].position)
			.SetSpeed (moveSpeed)
			.SetEaseType (iTween.EaseType.linear)
			.SetOnComplete ("ChooseNewTarget")
			.SetOnCompleteTarget (this.gameObject)
			.Build ());
	}

	private void ChooseNewTarget() {
		++currentIndexToMoveTo;
		if (currentIndexToMoveTo >= movePositions.Length) {
			currentIndexToMoveTo = 0;
		}
		MoveToTarget ();
	}
}
