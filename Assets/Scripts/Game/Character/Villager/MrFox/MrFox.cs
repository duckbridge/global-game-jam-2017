using UnityEngine;
using System.Collections;

public class MrFox : DispatchBehaviour {
    
    public MrFoxItem[] mrFoxShopItems;

    protected MrFoxActionManager mrFoxActionManager;

    protected AnimationControl animationControl;
    protected AnimationManager2D animationManager;

    protected MrFoxItem currentShopItem;

    void Start() {
        for(int i = 0 ; i < mrFoxShopItems.Length ; i++) {
            if(mrFoxShopItems[i] != null) {
                mrFoxShopItems[i].AddEventListener(this.gameObject);
            }
        }

        mrFoxActionManager = GetComponent<MrFoxActionManager>();

        animationManager = this.transform.Find("Body/Animations").GetComponent<AnimationManager2D>();
        animationControl = this.transform.Find("Body/Animations").GetComponent<AnimationControl>();

        animationControl.Initialize(GetComponent<BodyControl>());
        
        animationManager.Initialize();
        
        mrFoxActionManager.Initialize();

    }

    public void SwitchAction(MrFoxActionType newActionType) {
        mrFoxActionManager.SwitchState(newActionType);
    }

    public AnimationControl GetAnimationControl() {
        return animationControl;
    }

    public AnimationManager2D GetAnimationManager() {
        return animationManager;
    }

    public void OnShopItemRequested(MrFoxItem mrFoxShopItem) {
        this.currentShopItem = mrFoxShopItem;

        if(mrFoxActionManager.currentAction != null && mrFoxActionManager.currentAction.mrFoxActionType == MrFoxActionType.IDLE) {
            mrFoxActionManager.SetRunTarget(MrFoxActionType.RUN_TO_PLAYER, mrFoxShopItem.runTarget);
            mrFoxActionManager.SwitchState(MrFoxActionType.JUMPDOWN);

        } else {
            mrFoxActionManager.SwitchStateAndSetRunTarget(MrFoxActionType.RUN_TO_PLAYER, mrFoxShopItem.runTarget);
        }   
    }

    public MrFoxItem GetCurrentShopItem() {
        return this.currentShopItem;
    }
}
