using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GoalState : DispatchBehaviour {

	public WorldState worldState;
	public InternalState internalState;

	protected List<AIAction> actionSequence;
	protected Bot bot;
	
	public virtual void Update () {}

	public void OnActionCompleted(AIAction action) {
		RemoveCurrentAction(action);
		ExecuteAction();
	}

	public void FlushCurrentActions() {
		for(int i = 0 ; i < actionSequence.Count ; i++) {
			RemoveCurrentAction(actionSequence[i]);
			--i;
		}
	}

	private void RemoveCurrentAction() {
		if(actionSequence.Count > 0) {
			RemoveCurrentAction(actionSequence[0]);
		}
	}

	private void RemoveCurrentAction(AIAction action) {
		action.RemoveEventListener(this.gameObject);
		actionSequence.RemoveAt(0);

		Destroy(this.transform.Find("CurrentAction").GetComponentInChildren<AIAction>().gameObject);
	}

	public void ExecuteAction() {
		if(actionSequence.Count > 0) {
			
			if(bot == null) { //quick ugy fix
				return;
			}

			AIAction addedAction = (AIAction) GameObject.Instantiate(actionSequence[0], bot.transform.position, Quaternion.identity);
			addedAction.transform.parent = this.transform.Find("CurrentAction");
			addedAction.transform.localPosition = Vector3.zero;
			addedAction.ActivateActionForBot(bot);
			addedAction.AddEventListener(this.gameObject);

		} else { 
			DispatchMessage("OnGoalCompleted", this); 
		}
	}

	public AIAction GetCurrentAction() {
		AIAction currentAction = this.transform.Find("CurrentAction").GetComponentInChildren<AIAction>();
		return currentAction;
	}

	public void Initialize(Bot bot, Planner planner) {
		this.bot = bot;
		this.AddEventListener(planner.gameObject);
	}

	public void SetActionSequence(List<AIAction> actionSequence) {
		this.actionSequence = actionSequence;
	}

	public Bot GetBot() {
		return this.bot;
	}
}
