using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetroitShopKeeper : VillageShopKeeper {

	public TextBoxManager onNotEnoughMoneyTextBox, onBoughtTextBox, onSoldOutTextBox;

	public TextBoxManagerWithOptionMenu buyItemFromShopMenu;
	public List<ShopItem> allShopItems;

    protected Transform firstItemPosition, secondItemPosition;

	protected ShopItem shopItem1, shopItem2;
	private SpriteRenderer cancelButton;

	public override void Start() {
        base.Start();

		buyItemFromShopMenu.AddEventListener(this.gameObject);

		onNotEnoughMoneyTextBox.AddEventListener(this.gameObject);
		onBoughtTextBox.AddEventListener(this.gameObject);
        onSoldOutTextBox.AddEventListener(this.gameObject);

		firstItemPosition = this.transform.Find("FirstItemPosition");
		secondItemPosition = this.transform.Find("SecondItemPosition");
		cancelButton = this.transform.Find("CancelButton").GetComponent<SpriteRenderer>();
	}

	public override void OnTextBoxDoneAndHidden () {}

	public void OnTextDone() {
		if(shopKeeperState == ShopKeeperState.Moving) {
            if(allShopItems.Count > 0) {
        
    			GrabRandomItems();
    			cancelButton.enabled = true;

                buyItemFromShopMenu.Initialize();
    			buyItemFromShopMenu.ResetShowAndActivate();

            } else {
                shopKeeperState = ShopKeeperState.AfterSell;
                onSoldOutTextBox.ResetShowAndActivate();
            }
		} else if(shopKeeperState == ShopKeeperState.AfterSell) {
			shopKeeperState = ShopKeeperState.Moving;
            player.OnTalkingDone();
			player.GetComponent<PlayerInputComponent>().enabled = true;
			animationManager.PlayAnimationByName ("Move", true);
		}
	}

	public virtual void OnFirstItemChosen() {
        
		OnDoneWithSelling();
		shopItem1.OnInteract(player);
	}

	public void OnNotEnoughMoneyToBuyItem(ShopItem shopItem) {
		shopKeeperState = ShopKeeperState.AfterSell;
		onNotEnoughMoneyTextBox.ResetShowAndActivate();
		animationManager.PlayAnimationByName ("Move", true);
	}

	public virtual void OnItemBought(ShopItem shopItem) {
		shopKeeperState = ShopKeeperState.AfterSell;
		onBoughtTextBox.ResetShowAndActivate();
	}

    public void OnItemChosen(int itemId) {
        switch(itemId) {
            case 1:
                OnFirstItemChosen();
            break;
            
            case 2:
                OnSecondItemChosen();
            break;

            case 3:
                OnNoItemChosen();
            break;
        }
    }

	public void OnSecondItemChosen() {
		OnDoneWithSelling();
		shopItem2.OnInteract(player);
	}

	public void OnNoItemChosen() {
		OnDoneWithSelling();
        player.OnTalkingDone();
		player.GetComponent<PlayerInputComponent>().enabled = true;
	}

	protected virtual void GrabRandomItems() {      

        ShowItem(ref shopItem1, firstItemPosition);
        ShowItem(ref shopItem2, secondItemPosition);
		
        allShopItems.Add(shopItem1);
        allShopItems.Add(shopItem2);

		shopKeeperState = ShopKeeperState.Selling;

	}
        
    protected void ShowItem(ref ShopItem shopItem, Transform newItemParent) {
        shopItem = allShopItems[Random.Range (0, allShopItems.Count)];
        shopItem.AddEventListener(this.gameObject);
        
        shopItem.gameObject.SetActive(true);
        shopItem.transform.parent = newItemParent;
        shopItem.transform.localPosition = Vector3.zero;
    
        allShopItems.Remove(shopItem);

    }

	private void OnDoneWithSelling() {

		shopKeeperState = ShopKeeperState.Moving;

		cancelButton.enabled = false;
		shopItem1.gameObject.SetActive(false);
        
        if(shopItem2) {
		    shopItem2.gameObject.SetActive(false);
        }
		animationManager.PlayAnimationByName ("Move", true);
	}
}
