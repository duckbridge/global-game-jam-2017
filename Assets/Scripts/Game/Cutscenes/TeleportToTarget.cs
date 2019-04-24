using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TeleportToTarget : CutSceneComponent {

		public GameObject gameObjectToMove;
		public Transform teleportTarget;

		public override void OnActivated () {
			gameObjectToMove.transform.position = new Vector3(teleportTarget.position.x, gameObjectToMove.transform.position.y, teleportTarget.position.z);

			DeActivate();
		}
	}
}
