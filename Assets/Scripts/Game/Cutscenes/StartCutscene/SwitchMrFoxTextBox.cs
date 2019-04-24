using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class SwitchMrFoxTextBox : SwitchVillagerTextBox {

        public override void OnActivated () {
            villagerTextBoxSwitcher =  SceneUtils.FindObject<FirstMrFox>().GetComponent<VillagerTextBoxSwitcher>();
            base.OnActivated();
        }
    }
}
