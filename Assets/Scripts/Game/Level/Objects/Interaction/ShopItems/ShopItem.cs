using UnityEngine;
using System.Collections;

public class ShopItem : InteractionObject {

	public bool destroyAfterBought = true;
	public enum ShopItemType { Heart, ExtraHeart, Cassette }
	public ShopItemType shopItemType;

	public int price = 5;

	// Use this for initialization
	public override void Start () {
		this.transform.Find("Price").GetComponent<TextMesh>().text = price + "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnInteract (Player player) {
		if(canInteract) {
			base.OnInteract (player);

			if(player.GetCandyContainer().candyAmount >= price) {

				DispatchMessage("OnItemBought", this);

				player.GetCandyContainer().DecrementCandyAmount(price);
				player.GetComponent<PowerUpComponent>().OnPickedUp(this, destroyAfterBought);

			} else {
				DispatchMessage("OnNotEnoughMoneyToBuyItem", this);
			}
		}
	}
}
