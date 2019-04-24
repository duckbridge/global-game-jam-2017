using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TeleportToCassettePickupPosition : CutSceneComponent {

		public GameObject gameObjectToMove;

		public override void OnActivated () {
			Player player = SceneUtils.FindObject<Player> ();
			Transform teleportTarget = player.transform.Find ("CassettePickupPosition");
			gameObjectToMove.transform.position = new Vector3(teleportTarget.position.x, gameObjectToMove.transform.position.y, teleportTarget.position.z);

			DeActivate();
		}
	}
}
