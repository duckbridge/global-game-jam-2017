using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoomboxActionManager : MonoBehaviour {

	public BoomboxActionType startAnimalActionType;
	private List<BoomboxAction> boomboxActions;

	public BoomboxAction currentBoomboxAction;

	private bool isInitialized = false;

	void Start () {}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			boomboxActions = new List<BoomboxAction>(this.transform.Find ("Actions").GetComponents<BoomboxAction>());
			
			foreach(BoomboxAction boomboxAction in boomboxActions) {
				boomboxAction.SetBoombox(GetComponent<BoomboxCompanion>());
			}
		}
	}

	public void DisableAllActions() {
		if(boomboxActions != null) {
			foreach(BoomboxAction boomboxAction in boomboxActions) {
				boomboxAction.Disable();
			}
		}
	}

	void FixedUpdate() {

	}

	public void SwitchStateAndSetRunTarget(BoomboxActionType newBoomBoxType, GameObject newTarget, bool forced = false) {
		BoomboxAction newBoomboxAction = GetBoomboxActionTypeBy(newBoomBoxType);

		if(currentBoomboxAction) {

			if(forced) {
			
				newBoomboxAction.SetRunTarget(newTarget);
				SwitchStateForced(newBoomBoxType);
			
			} else if(CanSwitchToState(newBoomboxAction)) {

				newBoomboxAction.SetRunTarget(newTarget);
				SwitchState(newBoomBoxType);
			
			}

		} else {

			if(newBoomboxAction) {
				newBoomboxAction.SetRunTarget(newTarget);
				SwitchState(newBoomBoxType);
			}
		}
	}

	public void SwitchStateForced(BoomboxActionType newAnimalActionType) {
		BoomboxAction newBoomboxAction = GetBoomboxActionTypeBy(newAnimalActionType);
		
		if(newAnimalActionType != BoomboxActionType.NONE) {
			
			DoSwitch(newBoomboxAction);
		}
	}

	public void SwitchState(BoomboxActionType newAnimalActionType) {

		BoomboxAction newBoomboxAction = GetBoomboxActionTypeBy(newAnimalActionType);

		if(newAnimalActionType != BoomboxActionType.NONE) {

			if(currentBoomboxAction && !CanSwitchToState(newBoomboxAction)) {
				return;
			}

			DoSwitch(newBoomboxAction);
		}
	}

	private void DoSwitch(BoomboxAction newAnimalAction) {
		if(currentBoomboxAction) {
			
			currentBoomboxAction.RemoveEventListener(this.gameObject);
			currentBoomboxAction.Disable ();
		}
		
		currentBoomboxAction = newAnimalAction;
		currentBoomboxAction.AddEventListener(this.gameObject);
		
		currentBoomboxAction.StartAction();
	}

	private bool CanSwitchToState(BoomboxAction newAnimalAction) {
		bool canSwitch = true;

		if(!currentBoomboxAction.canBeInterrupted && currentBoomboxAction.IsActive()) {
			Logger.Log ("wanting to switch state, but state " + currentBoomboxAction + " cannot be interrupted and is not done yet");
			canSwitch = false;
		} 
		
		if(newAnimalAction.priority < currentBoomboxAction.priority && currentBoomboxAction.IsActive()) {
			Logger.Log ("wanting to switch state, but " + newAnimalAction +"'s priority is lower than " + currentBoomboxAction +"'s priority");
			canSwitch = false;
		}

		return canSwitch;
	}

	public void StopCurrentAction() {
		if(currentBoomboxAction) {
			currentBoomboxAction.FinishAction(BoomboxActionType.NONE);
		}
	}

	public void OnTriggerEnter(Collider coll) {
		if(currentBoomboxAction) {
			currentBoomboxAction.OnTriggered(coll);
		}
	}

	public BoomboxAction GetBoomboxActionTypeBy(BoomboxActionType boomboxActionType) {
		if(boomboxActions == null) { Initialize(); }

		return boomboxActions.Find (boomboxAction => boomboxAction.boomboxActionType == boomboxActionType);
	}
}
