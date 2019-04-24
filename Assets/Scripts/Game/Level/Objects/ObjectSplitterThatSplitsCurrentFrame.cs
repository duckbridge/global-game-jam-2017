using UnityEngine;
using System.Collections;

public class ObjectSplitterThatSplitsCurrentFrame : ObjectSplitter {

	public AnimationManager2D attachedAnimationManager;

	public override void DoSplit (Transform objectThatHits, Direction directionItHitsIn) {

		spriteToSplitOnHit = attachedAnimationManager.GetCurrentAnimation().outputRenderer;

		base.DoSplit (objectThatHits, directionItHitsIn);
	}
}
