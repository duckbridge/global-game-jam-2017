using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleTimeScaleIndependentAnimation2D : TimeScaleIndependentUpdate {
	
	public bool isPlayingReverse = false;

	public bool Loop;
	public Sprite[] frames;
	public bool playOnStartup = false;

	public SpriteRenderer outputRenderer;
	public bool hideWhenDone = false;

	protected bool stopped = false;
	protected bool paused = false;

	protected int currentFrame;

	// Use this for initialization
	public void Awake () {
		Initialize();
			
		currentFrame = 0;
		if(playOnStartup)
			Play(true);
        else 
            Stop();

		OnAwake();
	}

	
	public void Initialize() {
		if(!outputRenderer)
			outputRenderer = this.GetComponent<SpriteRenderer>();
	}


	public void Start() {
		OnStart();
	}

	public void SetOutputRenderer(SpriteRenderer outputRenderer) {
		this.outputRenderer = outputRenderer;
	}
	
	public void SetColor(Color newColor) {
		outputRenderer.color = newColor;
	}

	public Color GetColor() {
		return outputRenderer.color;
	}

	public override void OnUpdate () {
		if(IsPlaying()) {

			elapsedTime += deltaTime;

			if(elapsedTime > timeRequiredForUpdateTick) {
				elapsedTime = 0f;
				Animate();
			}
		}
	}

	public virtual void Animate() {
		CancelInvoke("Animate");
		if(!isPlayingReverse) {
			if(currentFrame >= frames.Length) {
				if(!Loop) {
					stopped = true;
					OnAnimationDone();
				} else {
					currentFrame = 0;
					DispatchMessage("OnLoopingAnimationDone", this);
				}
			}
		} else {
			if(currentFrame <= -1) {
				if(!Loop) {
					stopped = true;
					
					OnReverseAnimationDone();
				} else {
					currentFrame = frames.Length - 1;
				}
			}
		}
		
		if(outputRenderer.enabled && !stopped) {
			if(!isPlayingReverse)
				OnFrameExited(currentFrame - 1);
			else
				OnFrameExited(currentFrame);
			
			outputRenderer.sprite = frames[currentFrame];
			
			if(!isPlayingReverse)
				OnFrameEntered(currentFrame);
			else
				OnFrameEntered(currentFrame - 1);
		}
		
		if(!stopped) {
			if(!isPlayingReverse) {
				currentFrame++;
			} else {
				currentFrame--;
			}
		}
	}

	public void Play(bool reset = false, bool reverse = false) {
		this.isPlayingReverse = reverse;
		PlayWithReset(reset);
	}
	
	protected virtual void OnAnimationDone() {
		if(hideWhenDone) {
			Hide ();
		}

		DispatchMessage("OnAnimationDone", this);
	}

	protected virtual void OnReverseAnimationDone() {
		if(hideWhenDone) {
			Hide ();
		}
		
		DispatchMessage("OnReverseAnimationDone", this);
	}
	
	private void PlayWithReset(bool reset) {
		if(reset) {
			if(!isPlayingReverse) {
				currentFrame = 0;
			} else {
				currentFrame = frames.Length - 1;
			}
		}
		
		paused = false;
		stopped = false;
		outputRenderer.enabled = true;

		if(frames.Length > 1)
			Animate();
		else if(frames.Length > 0) {
			if(!isPlayingReverse)
				outputRenderer.sprite = frames[0];
			else 
				outputRenderer.sprite = frames[frames.Length - 1];
		}
		
		OnPlay();
	}

	public virtual void OnPlay(){}
	public virtual void OnAnimationStopped(){}

	protected virtual void OnStart(){}
	protected virtual void OnAwake() {}

	public virtual void OnFrameEntered(int enteredFrame) {}
	public virtual void OnFrameExited(int exitedFrame) {}

	public void Pause() {
		stopped = true;
		paused = true;
	}

	public void Resume() {
		stopped = false;
		paused = false;

		Animate ();
	}

	public void Hide() {
		this.outputRenderer.enabled = false;
	}

	public void Show() {
		this.outputRenderer.enabled = true;
	}

	public void Stop() {
		Pause();
		currentFrame = 0;

		OnAnimationStopped();
	}

	public void SetCurrentFrame(int newFrame) {
		if(newFrame > -1 && newFrame < frames.Length) {
			currentFrame = newFrame;
			outputRenderer.sprite = frames[currentFrame];
		}
	}

	public int GetPreviousFrame() {
		return currentFrame - 1;
	}

	public int GetCurrentFrame() {
		return currentFrame;
	}

	public bool IsPlaying() {
		return !this.stopped;
	}

	public override void OnPauseGame() {}

	public override void OnResumeGame() {}
}