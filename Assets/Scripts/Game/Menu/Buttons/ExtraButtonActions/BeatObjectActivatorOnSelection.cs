using UnityEngine;
using System.Collections;

public class BeatObjectActivatorOnSelection : ExtraButtonAction {
   
    public override void DoActionOnSelection() {
        base.DoActionOnSelection();
        GetComponent<BeatObject>().Activate();
    }

    public override void DoActionOnDeSelection() {
        base.DoActionOnDeSelection();
        GetComponent<BeatObject>().Deactivate();
    }

}
