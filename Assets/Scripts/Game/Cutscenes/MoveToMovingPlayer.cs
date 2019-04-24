using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class MoveToMovingPlayer : CutSceneComponent {

		public bool deactivateOnDone = true;
		public float closeToPlayerDistance = .5f;
		public GameObject gameObjectToMove;

		public float moveSpeed = .2f;

		public AnimationManager2D animationManager;
		public string moveAnimationName;
		private Player player;

		public void FixedUpdate() {
			if(isActivated) {

				Vector3 movePosition = player.transform.position;

				if(gameObjectToMove && Vector2.Distance(new Vector2(movePosition.x, movePosition.z), 
				                    new Vector2(gameObjectToMove.transform.position.x, gameObjectToMove.transform.position.z)) > closeToPlayerDistance) {

					Vector3 moveDirection = 
					   MathUtils.CalculateDirection(movePosition, gameObjectToMove.transform.position);

					gameObjectToMove.transform.position += new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);

				} else {
					if (deactivateOnDone) {
						DeActivate ();	
					}
				}
			}
		}

		public override void OnActivated () {
			player = SceneUtils.FindObject<Player>();
			animationManager.PlayAnimationByName(moveAnimationName);
		}

		private void OnDoneMoving() {
			if (deactivateOnDone) {
				DeActivate ();	
			}
		}
	}
}
