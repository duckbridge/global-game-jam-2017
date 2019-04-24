using UnityEngine;
using System.Collections;

public class MrFoxItem : ShopItem {

    public TextMesh informationOutput;
    public GameObject runTarget;

    public override void Start() {
        informationOutput.text = price + "";
    }

    public override void OnInteract(Player player) {
        HideInput(player);
        
        player.GetComponent<PlayerInputComponent>().enabled = false;
        player.OnTalking();
        
        DispatchMessage("OnShopItemRequested", this);
    }
}
