using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalActionManager : MonoBehaviour {

	public AnimalActionType startAnimalActionType;
	private List<AnimalAction> animalActions;

	public AnimalAction currentAnimalAction;

	private bool isInitialized = false;

	void Start () {}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			animalActions = new List<AnimalAction>(this.transform.Find ("Actions").GetComponents<AnimalAction>());
			
			foreach(AnimalAction animalAction in animalActions) {
				animalAction.SetBodyControl(GetComponent<BodyControl>());
				animalAction.SetAnimal(GetComponent<AnimalCompanion>());
			}

			SwitchState(startAnimalActionType);
		}
	}

	void FixedUpdate() {

	}

	public void SwitchStateAndSetRunTarget(AnimalActionType newAnimalActionType, GameObject newTarget, bool forced = false) {
		AnimalAction newAnimalAction = GetAnimalActionByType(newAnimalActionType);

		if(currentAnimalAction) {

			if(forced) {
			
				newAnimalAction.SetRunTarget(newTarget);
				SwitchStateForced(newAnimalActionType);
			
			} else if(CanSwitchToState(newAnimalAction)) {

				newAnimalAction.SetRunTarget(newTarget);
				SwitchState(newAnimalActionType);
			
			}

		}
	}

	public void SwitchStateForced(AnimalActionType newAnimalActionType) {
		AnimalAction newAnimalAction = GetAnimalActionByType(newAnimalActionType);
		
		if(newAnimalActionType != AnimalActionType.NONE) {
			
			DoSwitch(newAnimalAction);
		}
	}

	public void SwitchState(AnimalActionType newAnimalActionType) {

		AnimalAction newAnimalAction = GetAnimalActionByType(newAnimalActionType);

		if(newAnimalActionType != AnimalActionType.NONE) {

			if(currentAnimalAction && !CanSwitchToState(newAnimalAction)) {
				return;
			}

			DoSwitch(newAnimalAction);
		}
	}

	private void DoSwitch(AnimalAction newAnimalAction) {
		if(currentAnimalAction) {
			
			currentAnimalAction.RemoveEventListener(this.gameObject);
			currentAnimalAction.Disable ();
		}
		
		currentAnimalAction = newAnimalAction;
		currentAnimalAction.AddEventListener(this.gameObject);
		
		currentAnimalAction.StartAction();
	}

	private bool CanSwitchToState(AnimalAction newAnimalAction) {
		bool canSwitch = true;

		if(!currentAnimalAction.canBeInterrupted && currentAnimalAction.IsActive()) {
			Logger.Log ("wanting to switch state, but state " + currentAnimalAction + " cannot be interrupted and is not done yet");
			canSwitch = false;
		} 
		
		if(newAnimalAction.priority < currentAnimalAction.priority && currentAnimalAction.IsActive()) {
			Logger.Log ("wanting to switch state, but " + newAnimalAction +"'s priority is lower than " + currentAnimalAction +"'s priority");
			canSwitch = false;
		}

		return canSwitch;
	}

	public void StopCurrentAction() {
		currentAnimalAction.FinishAction(AnimalActionType.NONE);
	}

	public void OnTriggerEnter(Collider coll) {
		if(currentAnimalAction) {
			currentAnimalAction.OnTriggered(coll);
		}
	}

	private AnimalAction GetAnimalActionByType(AnimalActionType animalctionType) {
		if(animalActions == null) { Start (); }

		return animalActions.Find (animalAction => animalAction.animalActionType == animalctionType);
	}
}
