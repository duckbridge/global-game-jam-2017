using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {
	private BodyControl bodyControl;
	private AnimationManager2D animationManager;
	public enum CharacterState { Idle, IdleThrown, Moving, ChargingWeapon, ThrownWeapon, ThrownWeaponOnFloor, RetrievingWeapon, RetrievingWeaponToSide, DEAD, DRUMMING, DANCING, ROLLING, JUMPROPE, FISHING }
	public CharacterState characterState = CharacterState.Idle;

	private bool canChangeDirection = true;

	private bool isInitialized = false;
	private bool isTalking = false;

	// Use this for initialization
	public void Start () {
		Initialize();

	}

    public void Initialize() {
        if(!isInitialized) {
            isInitialized = true;
            
		    bodyControl = GetComponent<BodyControl>();
    
			animationManager = this.GetComponentInChildren<AnimationManager2D> ();
        
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
		
	private void ResetMovementSpeed() {
        bodyControl.ResetMoveSpeed();
    }

	public void FixedUpdate() {

		if(characterState == CharacterState.Idle) {

			if (isTalking) {
				animationManager.ResumeAnimationByName ("Talking");
			} else {
				if (IsWalking ()) { //still temporary
					animationManager.ResumeAnimationByName ("Walking");

				} else {
					animationManager.ResumeAnimationByName ("Idle");
				}
			}
		}
	}

	public void StopIdleAnimation() {
		foreach(IdleAnimation idleAnimation in GetComponentsInChildren<IdleAnimation>()) {
			idleAnimation.CancelIdleAnimation();
		}
	}

	public void OnDie() {
		characterState = CharacterState.DEAD;
	}

	public void SwitchState(CharacterState newState) {
		this.characterState = newState;
	}

	public void ToggleChangeDirection(bool canChangeDirection) {
		this.canChangeDirection = canChangeDirection;
	}

	public bool IsWalking() {
		return this.GetComponent<Rigidbody>().velocity != Vector3.zero;
	}

	public bool IsRolling() {
		return this.characterState == CharacterState.ROLLING;
	}

	public bool CanRoll() {
		bool canRoll = true;

		if(characterState == CharacterState.DEAD || characterState == CharacterState.DANCING || characterState == CharacterState.JUMPROPE || characterState == CharacterState.FISHING) {
			canRoll = false;
		}

		return canRoll;
	}

	public bool CanDance() {
		return characterState == CharacterState.Idle && !IsRolling();
	}

	public void PlayTalkingAnimation() {
		isTalking = true;
		animationManager.PlayAnimationByName("Talking", true);
		Invoke ("StopTalking", .5f);
	}

	private void StopTalking() {
		isTalking = false;
	}
}
