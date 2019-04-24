using UnityEngine;
using System.Collections;

public class JackRollAction : VillagerSpecialAction {

    public float rollSpeed = .3f;
    public Transform leftPosition, rightPosition;
    private bool isRollingToRight = true;

    public override void DoAction(Villager villager) {
        base.DoAction(villager);

        villager.DisableInteraction(SceneUtils.FindObject<Player>());

        this.GetComponent<Collider>().enabled = false;
        
        string rollAnimationPrefix = isRollingToRight ? "Right" : "Left";
        villager.GetAnimationManager().PlayAnimationByName(rollAnimationPrefix + "-Rolling");

        iTween.MoveTo(this.gameObject, 
                        new ITweenBuilder()
                            .SetPosition(isRollingToRight ? rightPosition.position : leftPosition.position)
                            .SetSpeed(rollSpeed)
                            .SetEaseType(iTween.EaseType.linear)
                            .SetOnComplete("OnDoneRolling")
                            .SetOnCompleteTarget(this.gameObject).Build());
    }

    public void OnDoneRolling() {
        isRollingToRight = !isRollingToRight;
        this.GetComponent<Collider>().enabled = true;
    }
}
