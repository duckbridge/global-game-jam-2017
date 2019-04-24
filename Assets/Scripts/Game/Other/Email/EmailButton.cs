using UnityEngine;
using System.Collections;

public class EmailButton : MenuButtonWithColors {

    public int emailID;
    public string subject, emailText;

    public override void OnPressed() {
        base.OnPressed();
        DispatchMessage("ShowEmailOfButton", this);
    }
}
