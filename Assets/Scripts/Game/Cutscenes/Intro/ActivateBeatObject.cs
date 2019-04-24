using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class ActivateBeatObject : CutSceneComponent {
		
		public BeatObject beatObjectToActivate;
		
		public override void OnActivated () {

			beatObjectToActivate.Initialize();
			beatObjectToActivate.Activate();
			DeActivate();
		}
	}
}

