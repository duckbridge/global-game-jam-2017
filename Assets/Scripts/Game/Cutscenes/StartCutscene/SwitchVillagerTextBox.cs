using UnityEngine;
using System.Collections;

namespace Cutscenes {
    public class SwitchVillagerTextBox : CutSceneComponent {

        public string tbManagerName;
        public VillagerTextBoxSwitcher villagerTextBoxSwitcher;

        public override void OnActivated () {

            villagerTextBoxSwitcher.SwitchTextBoxManager(tbManagerName);

            DeActivate();
        }
    }
}
