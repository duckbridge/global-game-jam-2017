using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PlayerThrowCassette : CutSceneComponent {

		public float flySpeed;

		public GameObject fakeCassette;
		public Transform cassetteFlyTarget;

		public override void OnActivated () {
			SceneUtils.FindObject<PlayerInputComponent>().enabled = true;
			SceneUtils.FindObject<PlayerInputComponent>().AddEventListener(this.gameObject);
		
		}

		public void OnNextTrackPressed() {
			SceneUtils.FindObject<PlayerInputComponent>().RemoveEventListener(this.gameObject);
			iTween.MoveTo(fakeCassette, new ITweenBuilder().SetPosition(cassetteFlyTarget.position).SetSpeed(flySpeed).SetOnCompleteTarget(this.gameObject).SetOnComplete("OnThrownCassette").Build());
		}

		private void OnThrownCassette() {
			fakeCassette.SetActive(false);
			DeActivate();
		}
	}
}
