using UnityEngine;
using System.Collections;

public class InteractionObjectListener : InteractionObject {
    
    public override void OnInteract(Player player) {
        base.OnInteract(player);
        DispatchMessage("OnInteractListener", player);
    }
}
