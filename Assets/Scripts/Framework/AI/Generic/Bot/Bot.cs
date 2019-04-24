using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bot : DispatchBehaviour {

	protected Vector3 spawnPosition;
	protected Vector3 targetPosition;

	protected List<GoalState> goalStates;
	
	protected GoalState currentGoalState;
	protected WorldEventHandler worldEventHandler;
	public GoalState startingGoal;

	public void Start () {
		
		this.goalStates = new List<GoalState>(this.transform.Find("Goals").GetComponentsInChildren<GoalState>());
		worldEventHandler = this.GetComponentInChildren<WorldEventHandler>();
		worldEventHandler.AddEventListener(this.gameObject);
	}
	
	// Update is called once per frame
	public void Update () {

	}

	public GoalState GetStartGoal() {
		return this.startingGoal;
	}

	public GoalState GetCurrentGoal() {
		return this.currentGoalState;
	}

	public List<GoalState> GetGoals() {
		return this.goalStates;
	}

	protected AIAction GetCurrentAction() {
		if(this.currentGoalState != null) {
			AIAction currentAction = this.currentGoalState.GetCurrentAction();
			if(currentAction!= null) {
				return currentAction;
			}
		}
		return null;
	}

	public void SetGoalStateAndExecute(GoalState newGoalState) {
		this.currentGoalState = newGoalState;
		this.currentGoalState.ExecuteAction();
	}

	public void OnWorldStateChanged(List<WorldState> newWorldState) {
		AIAction currentAction = GetCurrentAction();
		if(currentAction) {
			currentAction.OnWorldStateChanged(newWorldState);
		}
	}
}