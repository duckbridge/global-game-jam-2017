using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class UnEquipDB : CutSceneComponent {

		public override void OnActivated () {

			Player player = SceneUtils.FindObject<Player>();

            player.UnEquipDB();
            SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);

			DeActivate();
		}
	}
}