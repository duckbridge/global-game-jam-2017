using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PullDBToForeground : CutSceneComponent {

		public float newY;

		public override void OnActivated () {

			BoomboxCompanion boomboxCompanion = SceneUtils.FindObject<BoomboxCompanion> ();
			boomboxCompanion.transform.position = new Vector3 (boomboxCompanion.transform.position.x, newY, boomboxCompanion.transform.position.z);

			DeActivate ();
		}
	}
}