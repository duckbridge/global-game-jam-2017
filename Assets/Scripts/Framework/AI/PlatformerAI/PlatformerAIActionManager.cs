using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformerAIActionManager : MonoBehaviour {

	public PlatformerAIAction currentAction;

	protected List<PlatformerAIAction> bossActions;

	private string currentActionTypeName;
	protected bool isInSecondStage = false;

	public virtual void Awake() {
		bossActions = new List<PlatformerAIAction>(GetComponentsInChildren<PlatformerAIAction>());
	}

	public void StartFirstAction() {
		SwitchToNewAction(currentAction.GetActionName());
	}

	public void StopCurrentAction() {
		currentAction.FinishAction();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnActionDone(string newActionTypeName) {
		SwitchToNewAction(newActionTypeName);
	}
	
	public string GetCurrentActionTypeName() {
		return currentActionTypeName;
	}
	
	private void SwitchToNewAction(string newActionTypeName) {

		if(currentAction) {
			currentAction.FinishAction();
		}

		currentAction = FindActionByType(newActionTypeName);
		currentActionTypeName = newActionTypeName.ToString();

		Logger.Log ("switching to " + currentAction);
		currentAction.StartAction();
	}
	
	public PlatformerAIAction FindActionByType(string actionTypeName) {
		PlatformerAIAction typeToReturn = null;

		Logger.Log (actionTypeName);

		typeToReturn = bossActions.Find(bossAction => bossAction.GetActionName() == actionTypeName);
		
		return typeToReturn;
	}

	public List<PlatformerAIAction> GetActions() {
		return bossActions;
	}

	public void OnTriggerEnter(Collider coll) {
		if(currentAction) {
			currentAction.OnTriggered(coll);
		}
	}

	public void OnCollisionEnter(Collision coll) {
		if(currentAction) {
			currentAction.OnCollided(coll);
		}
	}


	public virtual void OnSecondStageEntered() {
		if(!isInSecondStage) {
			isInSecondStage = true;
			foreach(PlatformerAIAction bossAction in bossActions) {
				bossAction.OnSecondStageEntered();
			}
		}
	}
}
