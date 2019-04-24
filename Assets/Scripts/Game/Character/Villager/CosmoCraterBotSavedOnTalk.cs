using UnityEngine;
using System.Collections;

public class CosmoCraterBotSavedOnTalk : VillagerThatTalksAfterAnimationIsDone {

    public override void OnAnimationDone(Animation2D animation2D) {
        base.OnAnimationDone(animation2D);

        PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        playerSaveComponent.OnTakedToBotInCosmoCrater();
    }
}
