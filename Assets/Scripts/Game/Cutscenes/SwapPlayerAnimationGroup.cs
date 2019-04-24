using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SwapPlayerAnimationGroup : CutSceneComponent {

		public AnimationGroup animationGroupToSwapTo;

		public override void OnActivated () {

			SceneUtils.FindObject<Player>().GetAnimationControl().SwapAnimationGroup(animationGroupToSwapTo);
			DeActivate();
		}
	}
}
