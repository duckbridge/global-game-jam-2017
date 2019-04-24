using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager2D : MonoBehaviour {

	private List<Animation2D> animations;
	protected Dictionary<string, Animation2D> animationsByName = new Dictionary<string, Animation2D>();

	private Animation2D currentAnimation;
	private bool isInitialized = false;
	private bool canSwitchAnimations = true;

	public void Start () {
		Initialize();
	}

	public void ResetAndInitialize() {
		isInitialized = false;
		animationsByName = new Dictionary<string, Animation2D>();
		Initialize();
	}

	public void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			animations = new List<Animation2D>(this.GetComponentsInChildren<Animation2D>());
			
			foreach(Animation2D animation2D in animations) {
				animation2D.Initialize();

				if(!animationsByName.ContainsKey(animation2D.name)) {
					animationsByName.Add(animation2D.name, animation2D);

					if(!animation2D.playOnStartup) {
						animation2D.Stop();
						animation2D.Hide();
					} else {
						currentAnimation = animation2D;
					}
				}
			}
		}
	}

	public void ResumeAnimationByNameAndResetIfDone(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation && canSwitchAnimations) {
			if(foundAnimation != currentAnimation) {
				if(!foundAnimation.IsPlaying()) {
					PlayAnimationByName(animationName, false);
				}
			} else if(!currentAnimation.IsPlaying()) {
				PlayAnimationByName(animationName, true);
			}
		}
	}

	public void ResumeAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		if(foundAnimation && foundAnimation != currentAnimation && !foundAnimation.IsPlaying() && canSwitchAnimations) {
			PlayAnimationByName(animationName, false);
		}
	}

	public void ResumeAnimationSynced(string animationName,  bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(currentAnimation != foundAnimation) {

				int savedFrame = 0;

				if(currentAnimation) {
					if(currentAnimation.GetComponent<IdleAnimation>()) {
						currentAnimation.GetComponent<IdleAnimation>().CancelIdleAnimation();
					}

					savedFrame = currentAnimation.GetPreviousFrame();
					currentAnimation.Stop();
					currentAnimation.Hide ();
				}

				foundAnimation.SetCurrentFrame(savedFrame);
				foundAnimation.Show ();
				foundAnimation.Play(false, false, useTimeOut);

			} 
			currentAnimation = foundAnimation;
		}
	}

	public void StopHideAllAnimations() {
		foreach(Animation2D animation2D in animations) {
			StopHideAnimationByName(animation2D.name);
		}
	}

	public void StopAllAnimations() {
		foreach(Animation2D animation2D in animations) {
			StopAnimationByName(animation2D.name);
		}
	}

	public void StopAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		if(foundAnimation) {
			foundAnimation.Stop();
		}
	}

	public void StopHideAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		if(foundAnimation) {
			foundAnimation.Stop();
			foundAnimation.Hide ();
		}
	}

	public void PlayAnimationByNameReversed(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
        if(!canSwitchAnimations) { 
            return;
        }

		if(foundAnimation) {
			
			if(currentAnimation) {
				
				if(currentAnimation.GetComponent<IdleAnimation>()) {
					currentAnimation.GetComponent<IdleAnimation>().CancelIdleAnimation();
				}

				if(currentAnimation.GetComponent<Blink2D>()) {
					currentAnimation.GetComponent<Blink2D>().StopBlinking();
				}

				currentAnimation.Stop();
				currentAnimation.Hide ();
			}
			
			foundAnimation.Show ();
			foundAnimation.Play(false, true, false);
			
			currentAnimation = foundAnimation;
		}
	}
	
	public virtual void PlayAnimationByName(string animationName, bool reset = false, bool useTimeOut = false, bool force = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		if(!force && !canSwitchAnimations) { 
			return;
		}

		if(foundAnimation) {

			if(currentAnimation) {

				if(currentAnimation.GetComponent<IdleAnimation>()) {
					currentAnimation.GetComponent<IdleAnimation>().CancelIdleAnimation();
				}

				if(currentAnimation.GetComponent<Blink2D>()) {
					currentAnimation.GetComponent<Blink2D>().StopBlinking();
				}

				currentAnimation.Stop();
				currentAnimation.Hide ();
			}

			foundAnimation.Show ();
			foundAnimation.Play(reset, false, useTimeOut);

			currentAnimation = foundAnimation;
		}
	}

	public void SetLastFrameForAnimation(string animationName, bool pauseAnimation = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(pauseAnimation) {
				foundAnimation.Pause ();
			}
			foundAnimation.SetCurrentFrame(foundAnimation.frames.Length - 1);
		}
	}

	public void SetFrameForAnimation(string animationName, int frame, bool pauseAnimation = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(pauseAnimation) {
				foundAnimation.Pause ();
			}
			foundAnimation.SetCurrentFrame(frame);
			foundAnimation.Show ();
		}
	}

	public void StopHideIndependentAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.Hide();
			foundAnimation.Stop();
		}
	}

	public void PlayIndependentAnimationByName(string animationName, bool reset = false, bool reverse = false, bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.Show ();
			foundAnimation.Play(reset, reverse, useTimeOut);
		}
	}

	public void ResumeIndependentAnimationByName(string animationName, bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
		   if(!foundAnimation.IsPlaying()) {
				foundAnimation.Show ();
				foundAnimation.Play(false, false, useTimeOut);
			} else if(useTimeOut) {

				foundAnimation.CancelInvoke("OnAnimationDone");
				foundAnimation.Invoke ("OnAnimationDone", foundAnimation.animationPlayTimeout);
			
			}
		}
	}

	public void SetColorForAnimation(string animationName, Color newColor) {
		Animation2D foundAnimation = GetAnimationByName(animationName);
		if(foundAnimation) {
			foundAnimation.SetColor(newColor);
		}
	}

	public void AddEventListenerTo(string animationName, GameObject listener) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.AddEventListener(listener);
		}
	}
	
	public void RemoveEventListenerFrom(string animationName, GameObject listener) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.RemoveEventListener(listener);
		}
	}

	public Animation2D GetAnimationByName(string animationName) {
		Animation2D foundAnimation = null;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		return foundAnimation;
	}

	public List<Animation2D> GetAnimations() {
		return this.animations;
	}

	public Animation2D GetCurrentAnimation() {
		return currentAnimation;
	}

	public void EnableSwitchAnimations() {
		canSwitchAnimations = true;
	}

	public void DisableSwitchAnimations() {
		canSwitchAnimations = false;
	}

	public bool IsInitialized() {
		return isInitialized;
	}
}
