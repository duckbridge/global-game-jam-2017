using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleAnimation : Animation2D {

	public List<IdleAnimationFrames> idleAnimationFrames;
	public Sprite[] standStillFrames;
	
	protected IdleAnimationFrames idleAnimationToPlay;
	private bool isPlayingIdleAnimation = false;
	
	public override void OnPlay() {
		
		int idleAnimationChosen = Random.Range (0, idleAnimationFrames.Count);
		idleAnimationToPlay = idleAnimationFrames[idleAnimationChosen];
		
		frames = standStillFrames;
		SetCurrentFrame(0);
		DispatchMessage("OnResetIdleAnimationState", null);
		
		CancelIdleAnimation();
		Invoke ("StartIdleAnimation", idleAnimationToPlay.animationStartTimeout);
		
	}
	
	public void CancelIdleAnimation() {
		CancelInvoke("StartIdleAnimation");
	}
	
	protected virtual void StartIdleAnimation() {
		frames = idleAnimationToPlay.GetFramesAndSaveCopyOfIt();
		Loop = idleAnimationToPlay.doLoop;
		SetFPS(idleAnimationToPlay.FPS);
		
		SetCurrentFrame(0);
		
		DispatchMessage("OnPlayingIdle", null);

		
		Resume ();
	}
}
