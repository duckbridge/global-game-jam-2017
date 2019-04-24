using UnityEngine;
using System.Collections;

public class DetroitShopKeeperThatSellsCassette : DetroitShopKeeper {

    public Vector3 cassetteThrowForce = new Vector3(-165f, 0f, 165f);
    public CassettePickup cassettePickup;
    public CassetteShopItem cassetteToSell;


    protected override void GrabRandomItems() { 
		animationManager.SetFrameForAnimation ("Move", 0, true);
        if(cassetteToSell == null) {
            base.GrabRandomItems();
        } else {
            shopItem1 = cassetteToSell;
            shopItem1.AddEventListener(this.gameObject);
    
            allShopItems.Remove(shopItem1);
            allShopItems.Add(shopItem1);
    
            shopItem1.gameObject.SetActive(true);
            shopItem1.EnableInteraction(player);

            shopItem1.transform.parent = firstItemPosition;
        
            shopItem1.transform.localPosition = Vector3.zero;
    
            shopKeeperState = ShopKeeperState.Selling;
        }
    }

    public void OnCassetteBought() {
        animationManager.PlayAnimationByName("ThrowItem", true);
    }

    public override void OnItemBought(ShopItem shopItem) {
        
        if(shopItem.GetComponent<CassetteShopItem>() && cassettePickup) {

            allShopItems.RemoveAt(0);
            cassetteToSell = null;
            
            cassettePickup.gameObject.SetActive(true);
            cassettePickup.transform.parent = this.transform.parent;
            cassettePickup.GetComponent<Rigidbody>().velocity = cassetteThrowForce;
           
        }
  
        base.OnItemBought(shopItem);
    }

    public override void OnInteract(Player player) {

		if (!cassetteToSell) {
			shopKeeperState = ShopKeeperState.AfterSell;
		} else if (canInteract) {
			PlayerSaveComponent playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent> ();
			if (playerSaveComponent.GetUnlockedTileTypeTracks ().Contains (cassetteToSell.tileType)) {
				allShopItems.RemoveAt (0);
				cassetteToSell = null;
				textManager = onSoldOutTextBox;
				shopKeeperState = ShopKeeperState.AfterSell;
			}
		}

        base.OnInteract(player);
    }
}
