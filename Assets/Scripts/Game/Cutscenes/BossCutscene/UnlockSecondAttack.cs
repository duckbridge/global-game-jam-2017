using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class UnlockSecondAttack : CutSceneComponent {

        public override void OnActivated () {

			SceneUtils.FindObject<PowerUpComponent> ().UnlockSecondAttack ();
			SceneUtils.FindObject<PlayerSaveComponent>().SaveData(SpawnType.TELEPORTED, true);

            DeActivate();
        }
    }
}
