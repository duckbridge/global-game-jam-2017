using UnityEngine;
using System.Collections;

public class StartJumpMinigame : CutSceneComponent {

	public JumpRopeMinigame jumpRopeMinigame;

	public override void OnActivated () {
		DeActivate();
	}
}
