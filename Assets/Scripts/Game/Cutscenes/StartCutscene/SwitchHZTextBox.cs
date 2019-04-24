using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class SwitchHZTextBox : SwitchVillagerTextBox {

         public override void OnActivated () {

            villagerTextBoxSwitcher = SceneUtils.FindObject<FirstHerz>().GetComponent<VillagerTextBoxSwitcher>();
            base.OnActivated();
        }
    }
}
