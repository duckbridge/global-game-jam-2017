using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class MoveDBToTarget : CutSceneComponent {

		public float moveSpeed = 5f;
		public Transform moveTarget;

		public string moveAnimationName;
		public string idleAnimationName;
       
        private BoomboxCompanion boomboxCompanion;

		public override void OnActivated () {

            boomboxCompanion = SceneUtils.FindObject<Player>().GetBoomboxCompanion();
           	Vector3 movePosition = new Vector3(moveTarget.position.x, boomboxCompanion.transform.position.y, moveTarget.position.z);

			iTween.MoveTo(boomboxCompanion.gameObject, 
			              new ITweenBuilder()
			              .SetPosition(movePosition)
			              .SetSpeed(moveSpeed)
			              .SetEaseType(iTween.EaseType.linear)
			              .SetOnCompleteTarget(this.gameObject)
			              .SetOnComplete("OnDoneMoving")
			              .Build());

            boomboxCompanion.GetAnimationManager().Initialize();
			boomboxCompanion.GetAnimationManager().PlayAnimationByName(moveAnimationName, true);
		}

		private void OnDoneMoving() {
			if(idleAnimationName.Length > 0) {
				boomboxCompanion.GetAnimationManager().PlayAnimationByName(idleAnimationName, true);
			}

			DeActivate();
		}
	}
}
