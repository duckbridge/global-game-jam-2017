using UnityEngine;
using System.Collections;

public class CassetteShopItem : ShopItem {

    public TileType tileType;

    public override void OnInteract (Player player) {
        if(canInteract) {
            
            if(player.GetCandyContainer().candyAmount >= price) {
                canInteract = false;
            
                DispatchMessage("OnCassetteBought", null);
                Invoke("DispatchItemBoughtDelayed", .5f);
                
                player.GetCandyContainer().DecrementCandyAmount(price);

            } else {
                DispatchMessage("OnNotEnoughMoneyToBuyItem", this);
            }

            HideInput(player);
        }
    }

    private void DispatchItemBoughtDelayed() {
        DispatchMessage("OnItemBought", this);
    }
}
