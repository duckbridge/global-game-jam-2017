using UnityEngine;
using System.Collections;

namespace Cutscens {
public class ActivateBossEnemy : CutSceneComponent {

		public override void OnActivated () {
			SceneUtils.FindObject<BossEnemy>().OnActivate();
			DeActivate();
		}
	}
}
