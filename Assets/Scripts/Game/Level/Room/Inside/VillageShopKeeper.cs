using UnityEngine;
using System.Collections;

public class VillageShopKeeper : Villager {

    public float leftLimit, rightLimit;
    public float moveToPlayerSpeed = .2f;

    protected enum ShopKeeperState { Moving, Selling, AfterSell }
    protected ShopKeeperState shopKeeperState = ShopKeeperState.Moving;

    protected bool isActivated = false;

    public virtual void Activate() {
        player = SceneUtils.FindObject<Player>();
        isActivated = true;
        shopKeeperState = ShopKeeperState.Moving;
    }

    public void DeActivate() {
        isActivated = false;
        shopKeeperState = ShopKeeperState.Moving;
    }

    public void FixedUpdate() {

        if(isActivated) {

            if(Mathf.Abs(this.transform.position.x - player.transform.position.x) > .5f) {

                float directionX = player.transform.position.x - this.transform.position.x;
                
                if(directionX > 0) {
                    directionX = 1f;
                } else if(directionX < 0) {
                    directionX = -1f;
                }
                    
                if(this.transform.localPosition.x + (moveToPlayerSpeed * directionX) >= leftLimit &&
                    this.transform.localPosition.x + (moveToPlayerSpeed * directionX) <= rightLimit) {
                    
                    this.transform.localPosition += new Vector3(moveToPlayerSpeed * directionX, 0f, 0f);
                }
            }
        }
    }
}
