using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class SetAllSoundVolumes : CutSceneComponent {
		
		public override void OnActivated () {
            SceneUtils.FindObject<SettingsSaveComponent>().LoadSettingsData();
			SoundUtils.SetSoundVolumeToSavedValue();
			DeActivate();
		}
	}
}