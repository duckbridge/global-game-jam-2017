using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SaveKiddo : CutSceneComponent {

		public string kiddoName = "Jack";

		public override void OnActivated () {

			SceneUtils.FindObject<PlayerSaveComponent>().AddSavedKiddo(kiddoName);
			SceneUtils.FindObject<PlayerSaveComponent>().SaveData (SpawnType.TELEPORTED);

			DeActivate();
		}
	}
}
