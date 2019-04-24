using UnityEngine;
using System.Collections;

public class MrFoxTalkAction : MrFoxAction {

    public MrFoxActionType mrFoxActionTypeOnDone;
    public TextBoxManager textBoxManagerToUse;

    protected override void OnStarted() {
        textBoxManagerToUse.AddEventListener(this.gameObject);
        textBoxManagerToUse.ResetShowAndActivate();
    }

    public void OnTextDone() {
        FinishAction(mrFoxActionTypeOnDone);
    }
}