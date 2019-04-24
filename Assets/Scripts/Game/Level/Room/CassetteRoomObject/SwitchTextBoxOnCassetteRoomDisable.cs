using UnityEngine;
using System.Collections;

public class SwitchTextBoxOnCassetteRoomDisable : ActionOnCassetteRoomDisable {
    public string newTextBoxName = "";
    public VillagerTextBoxSwitcher textboxSwitcher;

    public override void DoAction(bool disablingAtStart) {
        base.DoAction(disablingAtStart);
        textboxSwitcher.SwitchTextBoxManager(newTextBoxName);
    }
}
