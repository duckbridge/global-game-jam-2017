using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PlayMusicByTileType : CutSceneComponent {
		
		public TileType tileType;
		
		public override void OnActivated () {
			SoundUtils.SetSoundVolumeToSavedValue(SoundType.BG);
			SceneUtils.FindObject<MusicManager>().PlayMusicByTileType(tileType);

			DeActivate();
		}
	}
}
