using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class GivePlayerWatchPad : CutSceneComponent {

		public float watchPadMoveSpeed;
		public GameObject watch, pad;
		public SoundObject onReceivedWatchPadSound;
		public Transform watchPadMovePosition;

		public override void OnActivated () {
			watch.SetActive(true);
			pad.SetActive(true);

			iTween.MoveTo(watch, 
			              new ITweenBuilder()
			              .SetPosition(watchPadMovePosition.position)
			              .SetSpeed(watchPadMoveSpeed)
			              .SetEaseType(iTween.EaseType.linear)
			              .SetOnComplete("OnDoneMoving")
			              .SetOnCompleteTarget(this.gameObject)
			              .Build());

			iTween.MoveTo(pad, 
			              new ITweenBuilder()
			              .SetPosition(watchPadMovePosition.position)
			              .SetSpeed(watchPadMoveSpeed)
			              .SetEaseType(iTween.EaseType.linear)
			              .Build());
		}

		private void OnDoneMoving() {
			watch.SetActive(false);
			pad.SetActive(false);

			DeActivate();
		}
	}
}