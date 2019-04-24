using UnityEngine;
using System.Collections;

public class FreeDetroitCandyDrop : CandyDrop {

    public override void Awake()
    {
        base.Awake();
    
        if(!SceneUtils.FindObject<PlayerSaveComponent>().HasFreeBatteryInDetroit()) {
            this.gameObject.SetActive(false);
        }
        
    }

    protected override void OnPickedupByPlayer(Player player) {
        SceneUtils.FindObject<PlayerSaveComponent>().OnFreeBatteryInDetroitPickedUp();
        base.OnPickedupByPlayer(player);
    }
    
}
