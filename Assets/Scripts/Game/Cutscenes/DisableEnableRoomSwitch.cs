using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class DisableEnableRoomSwitch : CutSceneComponent {

		public bool disable = true;

		public override void OnActivated () {
			if(disable) {
				SceneUtils.FindObject<CameraBorderManager>().DisableSwitchingOfRooms();
			} else {
				SceneUtils.FindObject<CameraBorderManager>().EnableSwitchingOfRooms();
			}

			DeActivate();
		}
	}
}