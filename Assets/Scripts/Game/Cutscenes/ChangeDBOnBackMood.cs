using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class ChangeDBOnBackMood : CutSceneComponent {

        public WeaponOnBack.DBMoods moodToSwitchTo;

        public override void OnActivated () {
            WeaponOnBack weaponOnBack = SceneUtils.FindObject<WeaponOnBack>();

            if(weaponOnBack) {

                weaponOnBack.SetMood(moodToSwitchTo);
            
            }

            DeActivate();
        }
    }
}
