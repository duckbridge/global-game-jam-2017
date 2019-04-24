using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ScreenShotComponent))]
public class ScreenShotAndEmailTrigger : EmailTrigger {

    void Start() {}

    protected override void SubmitEmail(Player player) {
        GetComponent<ScreenShotComponent>().SaveScreenShot();
        base.SubmitEmail(player);
    }
}
