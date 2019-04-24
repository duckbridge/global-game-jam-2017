using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class AddRoomEventListenerToLevelBuilder : CutSceneComponent {

		public Room roomToAddEventListenerTo;

		public override void OnActivated () {
			roomToAddEventListenerTo.AddEventListener(SceneUtils.FindObject<LevelBuilder>().gameObject);
			DeActivate();
		}
	}
}
