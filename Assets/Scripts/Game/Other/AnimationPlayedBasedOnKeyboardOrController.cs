using UnityEngine;
using System.Collections;

public class AnimationPlayedBasedOnKeyboardOrController : ObjectThatRepondsToKeyboardOrController {

    public Animation2D animationOnControllerIn, animationOnControllerOut;

	protected override void OnXboxControllerPluggedIn() {
        base.OnXboxControllerPluggedIn();

        animationOnControllerIn.Play(true);
        animationOnControllerOut.StopAndHide();
    }

    protected override void OnXboxControllerUnPlugged() {
        base.OnXboxControllerUnPlugged();

        animationOnControllerOut.Play(true);
        animationOnControllerIn.StopAndHide();
    }
}
