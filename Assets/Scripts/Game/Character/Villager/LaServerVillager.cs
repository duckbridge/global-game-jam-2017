using UnityEngine;
using System.Collections;

public class LaServerVillager : VillagerThatDoesSpecialActionAfterTalking {
    
    public TextBoxManager textboxOnPasswordCorrect;

    public override void OnInteract (Player player) {
    
        if(SceneUtils.FindObject<PlayerSaveComponent>().HasLaCorrectPasswordInserted()) {
            textManager = textboxOnPasswordCorrect;
            GetAnimationManager().PlayAnimationByName("CorrectPassword", true);
        }
        
        base.OnInteract(player);
    }
}
