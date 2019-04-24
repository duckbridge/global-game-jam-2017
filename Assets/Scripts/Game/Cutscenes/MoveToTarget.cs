using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class MoveToTarget : CutSceneComponent {

		public GameObject gameObjectToMove;

		public float moveSpeed = 5f;
		public Transform moveTarget;

		public AnimationManager2D animationManager;

		public string moveAnimationName;
		public string idleAnimationName;

		public override void OnActivated () {

			Vector3 movePosition = new Vector3(moveTarget.position.x, gameObjectToMove.transform.position.y, moveTarget.position.z);

			iTween.MoveTo(gameObjectToMove, 
			              new ITweenBuilder()
			              .SetPosition(movePosition)
			              .SetSpeed(moveSpeed)
			              .SetEaseType(iTween.EaseType.linear)
			              .SetOnCompleteTarget(this.gameObject)
			              .SetOnComplete("OnDoneMoving")
			              .Build());

			animationManager.PlayAnimationByName(moveAnimationName);
		}

		private void OnDoneMoving() {
			if(idleAnimationName.Length > 0) {
				animationManager.PlayAnimationByName(idleAnimationName);
			}

			DeActivate();
		}
	}
}
