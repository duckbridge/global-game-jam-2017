using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ToggleInTown : CutSceneComponent {

		public bool isInTown = false;

		public override void OnActivated () {
			SceneUtils.FindObject<Player>().SetInTown(isInTown);
			DeActivate ();
		}
	}
}
