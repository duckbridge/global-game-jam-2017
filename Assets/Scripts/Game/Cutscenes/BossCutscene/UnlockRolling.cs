using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class UnlockRolling : CutSceneComponent {

        public override void OnActivated () {

            SceneUtils.FindObject<PowerUpComponent>().UnlockRolling();
			SceneUtils.FindObject<PlayerSaveComponent>().SaveData(SpawnType.TELEPORTED, true);

            DeActivate();
        }
    }
}
