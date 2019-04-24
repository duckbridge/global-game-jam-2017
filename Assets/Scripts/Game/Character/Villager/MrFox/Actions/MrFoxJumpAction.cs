using UnityEngine;
using System.Collections;

public class MrFoxJumpAction : MrFoxAction {

    public MrFoxActionType mrFoxActionTypeOnDone;

    public Transform jumpTarget;
    public bool jumpsUp = true;

    public float moveSpeed = .2f;

    protected override void OnStarted() {
        mrFox.GetAnimationManager().PlayAnimationByName("Jumping", true);
        mrFox.AddEventListener(this.gameObject);


        Vector3 movePosition = new Vector3(jumpTarget.position.x, mrFox.transform.position.y, jumpTarget.position.z);

        iTween.MoveTo(mrFox.gameObject, 
                      new ITweenBuilder()
                      .SetPosition(movePosition)
                      .SetSpeed(moveSpeed)
                      .SetEaseType(iTween.EaseType.easeOutExpo)
                      .SetOnCompleteTarget(this.gameObject)
                      .SetOnComplete("OnDoneMoving")
                      .Build());
    }

    public void OnDoneMoving() {
        FinishAction(mrFoxActionTypeOnDone);
    }
}