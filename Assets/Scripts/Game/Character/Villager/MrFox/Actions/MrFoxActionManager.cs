using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MrFoxActionManager : MonoBehaviour {

	public MrFoxActionType startActionType;
	private List<MrFoxAction> mrFoxActions;

	public MrFoxAction currentAction;

	private bool isInitialized = false;

	void Start () {}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			mrFoxActions = new List<MrFoxAction>(this.transform.Find ("Actions").GetComponents<MrFoxAction>());
			
			foreach(MrFoxAction foxAction in mrFoxActions) {
				foxAction.SetBodyControl(GetComponent<BodyControl>());
                foxAction.SetMrFox(GetComponent<MrFox>());
			}

			SwitchState(startActionType);
		}
	}

	void FixedUpdate() {

	}

    public void SetRunTarget(MrFoxActionType actionType, GameObject runTarget) { 
         MrFoxAction newFoxAction = GetActionByType(actionType);
        
        if(newFoxAction) {
            newFoxAction.SetRunTarget(runTarget);
        }
    }

    public void SwitchStateAndSetRunTarget(MrFoxActionType actionType, GameObject runTarget) {
        MrFoxAction newFoxAction = GetActionByType(actionType);
        
        if(newFoxAction) {
            newFoxAction.SetRunTarget(runTarget);
        }

        if(actionType != MrFoxActionType.NONE) {
            
            DoSwitch(newFoxAction);
        }
    }

	public void SwitchStateForced(MrFoxActionType actionType) {
		MrFoxAction newFoxAction = GetActionByType(actionType);
		
		if(actionType != MrFoxActionType.NONE) {
			
			DoSwitch(newFoxAction);
		}
	}

	public void SwitchState(MrFoxActionType actionType) {

		MrFoxAction newFoxAction = GetActionByType(actionType);

		if(actionType != MrFoxActionType.NONE) {

			if(currentAction && !CanSwitchToState(newFoxAction)) {
				return;
			}

			DoSwitch(newFoxAction);
		}
	}

	private void DoSwitch(MrFoxAction newAction) {
		if(currentAction) {
			
			currentAction.RemoveEventListener(this.gameObject);
			currentAction.Disable ();
		}
		
		currentAction = newAction;
		currentAction.AddEventListener(this.gameObject);
		
		currentAction.StartAction();
	}

	private bool CanSwitchToState(MrFoxAction newAction) {
		bool canSwitch = true;

		if(!currentAction.canBeInterrupted && currentAction.IsActive()) {
			Logger.Log ("wanting to switch state, but state " + currentAction + " cannot be interrupted and is not done yet");
			canSwitch = false;
		} 
		
		if(newAction.priority < currentAction.priority && currentAction.IsActive()) {
			Logger.Log ("wanting to switch state, but " + newAction +"'s priority is lower than " + currentAction +"'s priority");
			canSwitch = false;
		}

		return canSwitch;
	}

	public void StopCurrentAction() {
		currentAction.FinishAction(MrFoxActionType.NONE);
	}

	public void OnTriggerEnter(Collider coll) {
		if(currentAction) {
			currentAction.OnTriggered(coll);
		}
	}

	private MrFoxAction GetActionByType(MrFoxActionType actionType) {
		if(mrFoxActions == null) { Start (); }

		return mrFoxActions.Find (action => action.mrFoxActionType == actionType);
	}
}
