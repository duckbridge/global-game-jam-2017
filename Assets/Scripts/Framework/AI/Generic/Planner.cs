using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner : DispatchBehaviour {

	public List<AIAction> allBaseActions;
	public GoalState defaultGoalState;

	// Use this for initialization
	void Start () {
	}
	
	public void PlanForBots(List<Bot> bots) {
		bots.ForEach(bot => PlanAndExecute(bot, bot.GetStartGoal()));
	}

	// Update is called once per frame
	void Update () {
	
	}

	public GoalState PlanForBot(Bot bot) {

		foreach(GoalState goal in bot.GetGoals()) {
			if(WorldEventManager.Contains(goal.worldState)) {  //rewrite this part
				Debug.Log("[Planner] Found goal : " + goal + " finding action sequence " + bot.GetCurrentGoal());
				goal.SetActionSequence(FindActionsForGoal(bot, goal));
				return goal;
			}
		}

		//if nothing was found	
		Debug.Log("[Planner] nothing found, falling back on default!");
		defaultGoalState.SetActionSequence(FindActionsForGoal(bot, defaultGoalState));
		
		return defaultGoalState;
	}

	public void PlanAndExecute(Bot bot, InternalState internalState) {
		bot.GetCurrentGoal().FlushCurrentActions();
		GoalState goalWithInternalState = bot.GetGoals().Find(goal => goal.internalState == internalState);

		PlanAndExecute(bot, goalWithInternalState);
	}

	public void PlanAndExecute(Bot bot, GoalState goalState) { 

		Debug.Log("[Planner] Planning for : " + bot + " with goalstate :  " + goalState.internalState);

		if(!goalState) { Debug.Log("ERROR : CANNOT FIND A GOAL FOR THAT INTERNAL STATE!"); }

	 	goalState.SetActionSequence(FindActionsForGoal(bot, goalState));
	 	goalState.Initialize(bot, this);
	 	bot.SetGoalStateAndExecute(goalState);
	}


	public List<AIAction> FindActionsForGoal(Bot bot, GoalState goalState) {

		List<AIAction> actionSequence = FindActionsForWorldState(bot, goalState.internalState);
		actionSequence.Reverse();
		
		Debug.Log("[Planner] Found sequence with " + actionSequence.Count + " actions");
		return actionSequence;
	}

	private List<AIAction> FindActionsForWorldState(Bot bot, InternalState internalState) {
		List<AIAction> allTemporaryActions = new List<AIAction>(allBaseActions);
		List<AIAction> actionSequence = new List<AIAction>();

		FindActionForWorldState(bot, actionSequence, internalState, allTemporaryActions);

		return actionSequence;	
	}

	private void FindActionForWorldState(Bot bot, List<AIAction> actionSequence, InternalState preConditioninternalState, List<AIAction> allTemporaryActions) { // :((((
		
		AIAction highestSatisfactoryAction = null;
		int highestSatisfactionScore = 0;

		for(int i = 0 ; i < allTemporaryActions.Count ; i++) {
			int satisfactionScore = allTemporaryActions[i].ConditionsSatisfied(bot, preConditioninternalState);
			if(satisfactionScore > highestSatisfactionScore) {
				highestSatisfactoryAction = allTemporaryActions[i];
				highestSatisfactionScore = satisfactionScore;
			}
		}

		if(highestSatisfactoryAction != null) {
			Debug.Log("[Planner] found! : " + highestSatisfactoryAction);
			actionSequence.Add(highestSatisfactoryAction);	
			allTemporaryActions.Remove(highestSatisfactoryAction);

			FindActionForWorldState(bot, actionSequence, highestSatisfactoryAction.GetPreCondition(), allTemporaryActions);
		}
	}

	public void OnGoalCompleted(GoalState goalState) {
		goalState.RemoveEventListener(this.gameObject);
		PlanForBot(goalState.GetBot());
	}
}
