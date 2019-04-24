using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class MoveToPlayer : CutSceneComponent {

		public GameObject gameObjectToMove;

		public float moveSpeed = 5f;

		public AnimationManager2D animationManager;
		public string moveAnimationName;

		public override void OnActivated () {

			Vector3 movePosition = SceneUtils.FindObject<Player>().transform.position;

			movePosition = new Vector3(movePosition.x, gameObjectToMove.transform.position.y, movePosition.z);

			iTween.MoveTo(gameObjectToMove, 
			              new ITweenBuilder()
			              .SetPosition(movePosition)
			              .SetSpeed(moveSpeed)
			              .SetEaseType(iTween.EaseType.linear)
			              .SetOnCompleteTarget(this.gameObject)
			              .SetOnComplete("OnDoneMoving")
			              .Build());

			if(animationManager) {
				animationManager.PlayAnimationByName(moveAnimationName);
			}
		}

		private void OnDoneMoving() {
			DeActivate();
		}
	}
}
