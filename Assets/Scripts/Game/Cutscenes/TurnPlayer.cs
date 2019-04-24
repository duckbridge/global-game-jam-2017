using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class TurnPlayer : CutSceneComponent {

		public Direction directionToTurnTo = Direction.DOWN;

		public override void OnActivated () {
			Player player = SceneUtils.FindObject<Player>();

            player.GetCharacterControl().Initialize();

			DeActivate();

		}
	}
}