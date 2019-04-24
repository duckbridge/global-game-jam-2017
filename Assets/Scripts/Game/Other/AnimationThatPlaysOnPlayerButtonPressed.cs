using UnityEngine;
using System.Collections;
using InControl;

public class AnimationThatPlaysOnPlayerButtonPressed : ObjectThatActivatesOnRoomEnter {

	public bool activateOnStart = false;

	public enum ActionsToReactTo { 
		MOVELEFT,
		MOVERIGHT,
		MOVEUP,
		MOVEDOWN,
		ROLL,
		INTERACT,
		SECONDATTACK,
		THIRDATTACK,
		BACK,
		DANCE
	}

	public ActionsToReactTo actionToReactTo;
	private PlayerInputActions playerInputActions;
	private Animation2D animation2D;

	// Use this for initialization
	void Awake() {
		playerInputActions = PlayerInputHelper.LoadData();
		animation2D = GetComponent<Animation2D> ();

		if (activateOnStart) {
			this.isActivated = activateOnStart;
		}
	}
	
	// Update is called once per frame
	protected override void OnUpdate ()  {
		
		if (playerInputActions.moveHorizontally.Value > 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVERIGHT, true);
		} else if (playerInputActions.moveHorizontally.Value <= 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVERIGHT, false);
		}

		if (playerInputActions.moveHorizontally.Value < 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVELEFT, true);
		} else if (playerInputActions.moveHorizontally.Value >= 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVELEFT, false);	
		}

		if (playerInputActions.moveVertically.Value > 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVEUP, true);		
		} else if (playerInputActions.moveVertically.Value <= 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVEUP, false);	
		}

		if (playerInputActions.moveVertically.Value < 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVEDOWN, true);
		} else if (playerInputActions.moveVertically.Value >= 0) {
			AnimateIfReactsTo (ActionsToReactTo.MOVEDOWN, false);
		}
			
		if (playerInputActions.roll.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.ROLL, true);
		} else if (playerInputActions.roll.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.ROLL, false);
		}

		if (playerInputActions.interact.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.INTERACT, true);
		} else if (playerInputActions.interact.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.INTERACT, false);
		}

		if (playerInputActions.secondAttack.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.SECONDATTACK, true);
		} else if (playerInputActions.secondAttack.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.SECONDATTACK, false);
		}

		if (playerInputActions.thirdAttack.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.THIRDATTACK, true);
		} else if (playerInputActions.thirdAttack.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.THIRDATTACK, false);
		}

		if (playerInputActions.back.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.BACK, true);
		} else if (playerInputActions.back.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.BACK, false);
		}

		if (playerInputActions.dance.WasPressed) {
			AnimateIfReactsTo (ActionsToReactTo.DANCE, true);
		} else if (playerInputActions.dance.WasReleased) {
			AnimateIfReactsTo (ActionsToReactTo.DANCE, false);
		}
	}

	private void AnimateIfReactsTo(ActionsToReactTo actionRequired, bool isPressed) {
		if (actionRequired == actionToReactTo) {
			if (isPressed) {
				animation2D.SetCurrentFrame (1);
			} else {
				animation2D.SetCurrentFrame (0);
			}
		}
	}

	public override void DeActivate () {
		base.DeActivate ();
		GetComponent<Animation2D>().SetCurrentFrame(0);
	}
}