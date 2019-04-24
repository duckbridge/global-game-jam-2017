using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class InitializeBeatListener : CutSceneComponent {

		public MusicHutbeatListener beatListener;

		public override void OnActivated () {
			beatListener.Initialize(beatListener.musicToListenTo.GetSound().clip.name, beatListener.onBeatRange);
			DeActivate();
		}
	}
}
