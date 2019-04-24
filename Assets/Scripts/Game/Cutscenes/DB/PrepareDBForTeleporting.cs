using UnityEngine;
using System.Collections;

namespace Cutscenes {
	public class PrepareDBForTeleporting : CutSceneComponent {

        private BoomboxCompanion boomboxCompanion;

		public override void OnActivated () {

            boomboxCompanion = SceneUtils.FindObject<Player>().GetBoomboxCompanion();
            boomboxCompanion.GetComponent<Collider>().enabled = true;
            
            boomboxCompanion.SetTextBoxBasedOnContext();

            DeActivate();
        }
	}
}
