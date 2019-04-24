using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomboxNavigateToPlayer : BoomboxAction {
	
	public float closeToTargetDistance = 1f;
	
	private bool isMoving = false;
	private Player player;

	private List<PathFindNode> pathFindNodes;
	private PathFindNode[] path;

	private Transform nextTarget;
	private int currentIndex = 0;

	private int switchActionThreshold = 99;
	private BoomboxActionType actionTypeToSwitchTo;
	private GameObject gameObjectRequiredForAction;
	private Room sourceRoom;

	protected override void OnUpdate () {
		if (MathUtils.GetDistance2D (nextTarget.position, boomboxCompanion.transform.position) > closeToTargetDistance) {
			if (!isMoving) {
				boomboxCompanion.GetAnimationManager ().PlayAnimationByName ("Walking");
			}

			isMoving = true;

			boomboxCompanion.GetComponent<BodyControl> ().MoveKinematic (nextTarget.transform.position);
	
		} else {
			currentIndex++;
			if (currentIndex < path.Length) {
				nextTarget = path [currentIndex].transform;
				SwitchToOtherActionIfExistsAndThresholdIsMet ();
			} else {
				nextTarget = player.transform;
				SwitchToOtherActionIfExistsAndThresholdIsMet ();
				if (MathUtils.GetDistance2D (nextTarget.position, boomboxCompanion.transform.position) < closeToTargetDistance) {
					if (!player.GetComponent<CharacterControl> ().IsRolling ()) {
						currentIndex = 0;
						FinishAction (BoomboxActionType.EQUIP);
					}
				}
			}
		}
	}

	public void SetPathNodes(PathFindNode[] pathFindNodes) {
		this.pathFindNodes = new List<PathFindNode>(pathFindNodes);
	}

	protected override void OnFinished () {
		ResetProperties ();
	}

	private void ResetProperties() {
		currentIndex = 0;

		actionTypeToSwitchTo = BoomboxActionType.NONE;
		switchActionThreshold = 99;
		gameObjectRequiredForAction = null;

		this.sourceRoom = null;
	}

	protected override void OnStarted () {
		ResetProperties ();

		boomboxCompanion.GetComponent<Collider>().enabled = false;
		boomboxCompanion.GetAnimationManager().PlayAnimationByName("Walking");

		if (!player) {
			player = SceneUtils.FindObject<Player> ();
		}

		path = boomboxCompanion
			.GetComponent<PathFinder>()
			.FindBestPathToTargetFrom(pathFindNodes, boomboxCompanion.transform.position, player.transform.position)
			.ToArray();

		if (path.Length > 1) {
			nextTarget = path [currentIndex].transform;
		} else {
			nextTarget = player.transform;
		}
	}

	public int GetCurrentPathIndex () {
		return this.currentIndex;
	}

	private void SwitchToOtherActionIfExistsAndThresholdIsMet() {
		if (actionTypeToSwitchTo != BoomboxActionType.NONE && 
			(nextTarget == player.transform || currentIndex >= switchActionThreshold) && 
			gameObjectRequiredForAction != null) {

			boomboxCompanion.GetComponent<BoomboxActionManager> ().SwitchStateAndSetRunTarget (actionTypeToSwitchTo, gameObjectRequiredForAction);
		}
	}

	public void SetThresHoldAndActionToSwitchTo (int switchActionThreshold, BoomboxActionType actionTypeToSwitchTo, GameObject gameObjectRequiredForAction) {
		this.switchActionThreshold = switchActionThreshold;
		this.actionTypeToSwitchTo = actionTypeToSwitchTo;
		this.gameObjectRequiredForAction = gameObjectRequiredForAction;

		SwitchToOtherActionIfExistsAndThresholdIsMet ();
	}

	public bool CanSwitchToOtherAction(Room room) {
		bool canSwitchToOtherAction = true;

		if (sourceRoom != null && room != sourceRoom) {
			canSwitchToOtherAction = false;
		}

		return canSwitchToOtherAction;
	}

	public Room GetSourceRoom() {
		return this.sourceRoom;
	}

	public void SetSourceRoom(Room sourceRoom) {
		this.sourceRoom = sourceRoom;
	}
}