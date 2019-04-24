using UnityEngine;
using System.Collections;

public class VilagerRobotHide : VillagerSpecialAction {

    public override void DoAction(Villager villager) {
        base.DoAction(villager);
        
        this.GetComponent<Collider>().enabled = false;
        villager.DisableInteraction(SceneUtils.FindObject<Player>());

        villager.GetAnimationManager().PlayAnimationByName("Hiding", true);
    }
}
