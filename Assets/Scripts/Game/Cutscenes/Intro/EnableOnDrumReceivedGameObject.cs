using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class EnableOnDrumReceivedGameObject : CutSceneComponent {

		public override void OnActivated () {
			SceneUtils.FindObject<EnableOnDrumReceived> ().OnDrumReceived ();
			DeActivate();
		}	
	}
}
