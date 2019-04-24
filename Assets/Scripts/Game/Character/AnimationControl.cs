using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {

	private string currentAnimation = "Idle";
	private BodyControl bodyControl;

	private bool isInitialized = false;
	private AnimationManager2D animationManager;
	private bool canSwitchAnimations = true;

	private bool isEnabled = true;
	private AnimationGroup currentAnimationGroup = AnimationGroup.Naked;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FixedUpdate() {

	}

	public void PlayAnimationByName(string animationName, bool reset = false, bool useTimeout = false) {
		if(isEnabled) {
			string prefix = GetAnimationPrefix();

			currentAnimation = animationName;

			if(!animationManager) {
				animationManager = GetComponent<AnimationManager2D>();
			}

			animationManager.PlayAnimationByName(prefix + animationName, reset, useTimeout);
		}
	}

	public void ResumeAnimationSyncedByName(string animationName) {
		if(isEnabled) {

			if(!animationManager) {
				animationManager = GetComponent<AnimationManager2D>();
			}

			string prefix = GetAnimationPrefix();
			
			currentAnimation = animationName;
			
			animationManager.ResumeAnimationSynced(prefix + animationName, true);
		}
	}

	public void ResumeAnimationByName(string animationName) {
		if(isEnabled) {
			string prefix = GetAnimationPrefix();

			currentAnimation = animationName;
			animationManager.ResumeAnimationByName(prefix + animationName);
		}
	}

	public void AddEventListenerTo(string animationName, GameObject listeningGO) {
		string prefix = GetAnimationPrefix();
		animationManager.AddEventListenerTo(prefix + animationName, listeningGO);
	}

	public virtual void Initialize(BodyControl bodyControl) {
		if(!isInitialized) {
			this.animationManager = GetComponent<AnimationManager2D>();
			this.bodyControl = bodyControl;
		}

		isInitialized = true;
	}

	public string GetAnimationPrefix() {
		string prefix = "Front-";

		if(bodyControl) {
			if(bodyControl.GetCurrentDirection() == Direction.UP) {
				prefix = "Back-";
			}
			
			if(bodyControl.GetCurrentDirection() == Direction.LEFT) {
				prefix = "Left-";
			}
			
			if(bodyControl.GetCurrentDirection() == Direction.RIGHT) {
				prefix = "Right-";
			}
		}
		return prefix;

	}

	public void Enable() {
		isEnabled = true;
	}

	public void Disable() {
		isEnabled = false;
	}

	public AnimationGroup GetCurrentAnimationGroup() {
		return currentAnimationGroup;
	}

	public void SetCurrentAnimation(string currentAnimation) {
		this.currentAnimation = currentAnimation;
	}

	public void SwapAnimationGroup(AnimationGroup newAnimationGroup) {

		if(newAnimationGroup != currentAnimationGroup) {
			this.transform.Find(newAnimationGroup.ToString()).gameObject.SetActive(true);
			this.transform.Find(currentAnimationGroup.ToString()).gameObject.SetActive(false);

			if(!animationManager) {
				animationManager = GetComponent<AnimationManager2D>();
			}

			animationManager.ResetAndInitialize();
			ResumeAnimationByName(currentAnimation);

			currentAnimationGroup = newAnimationGroup;
		}
	}
}
