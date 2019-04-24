using UnityEngine;
using System.Collections;

public class MrFoxMoveToTarget : MrFoxAction {

    public Transform runTargetOverride;

    public MrFoxActionType mrFoxActionTypeOnDone;

    public float moveSpeed = .2f;

	protected override void OnStarted() {
		mrFox.AddEventListener(this.gameObject);

        Vector3 movePosition = Vector3.zero;

        if(runTargetOverride != null) {
            movePosition = new Vector3(runTargetOverride.transform.position.x, mrFox.transform.position.y, runTargetOverride.transform.position.z);
            mrFox.GetComponent<CharacterToTargetTurner>().SetTarget(runTargetOverride);
        } else {
            movePosition = new Vector3(runTarget.transform.position.x, mrFox.transform.position.y, runTarget.transform.position.z);
            mrFox.GetComponent<CharacterToTargetTurner>().SetTarget(runTarget.transform);
        }

        mrFox.GetComponent<CharacterToTargetTurner>().OnUpdate();

        mrFox.GetAnimationControl().PlayAnimationByName("Running", true);
    
        iTween.MoveTo(mrFox.gameObject, 
                      new ITweenBuilder()
                      .SetPosition(movePosition)
                      .SetSpeed(moveSpeed)
                      .SetEaseType(iTween.EaseType.linear)
                      .SetOnCompleteTarget(this.gameObject)
                      .SetOnComplete("OnDoneMoving")
                      .Build());
	}

    public void OnDoneMoving() {
        FinishAction(mrFoxActionTypeOnDone);
    }
}
