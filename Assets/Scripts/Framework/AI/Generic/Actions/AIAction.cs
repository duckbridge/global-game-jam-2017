using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AIAction : DispatchBehaviour {

	public InternalState precondition;
	public InternalState effect;

	protected Bot bot;
	protected bool isActivated = false;
	protected bool isInitialized = false;
	protected int conditionsSatisfied = 0;
	public virtual void Start() {}

	public virtual void Update() {
		CheckPreCondition();
	}

	public abstract void OnWorldStateChanged(List<WorldState> newWorldState);
	
	protected abstract void CheckPreCondition();
	protected abstract void ActivateAction();
	protected abstract void OnPreFailed();
	
	public virtual int ConditionsSatisfied(Bot bot, InternalState preConditionInternalState) {
		conditionsSatisfied = 0; //FUCK :D! BY THE GODS
		if(GetEffect() == preConditionInternalState) {
			++conditionsSatisfied;
		}  else {
			--conditionsSatisfied;
		}
		return conditionsSatisfied;
	}

	public void OnComplete() {
		DispatchMessage("OnActionCompleted", this);
	}

	public void ActivateActionForBot(Bot bot){
		this.bot = bot;
		ActivateAction();
	}

	public InternalState GetEffect() {
		return this.effect;
	}

	public InternalState GetPreCondition() {
		return this.precondition;
	}

	public bool IsActivated() {
		return isActivated;
	}
}
