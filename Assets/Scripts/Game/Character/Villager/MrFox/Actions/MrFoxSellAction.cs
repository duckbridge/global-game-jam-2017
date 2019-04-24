using UnityEngine;
using System.Collections;

public class MrFoxSellAction : MrFoxAction {

    public MrFoxActionType mrFoxActionTypeOnDone;
    public TextBoxManager tbManagerBeforeBuy, tbManagerOnNotEnoughMoney, tbManagerOnBought, tbManagerNotBought;

    private TextBoxManager currentTextBoxManager;

    private Player player;

    protected override void OnStarted() {

        player = SceneUtils.FindObject<Player>();

        mrFox.GetComponent<CharacterToTargetTurner>().SetTarget(player.transform);
        mrFox.GetAnimationControl().PlayAnimationByName("Talking", true);

        tbManagerBeforeBuy.AddEventListener(this.gameObject);
        tbManagerOnNotEnoughMoney.AddEventListener(this.gameObject);
        tbManagerOnBought.AddEventListener(this.gameObject);
        tbManagerNotBought.AddEventListener(this.gameObject);

        ShowTextBoxManager(tbManagerBeforeBuy);
    }

    private void ShowTextBoxManager(TextBoxManager textboxManager) {
        textboxManager.ResetShowAndActivate();
        this.currentTextBoxManager = textboxManager;

    }

    public void OnFirstItemChosen() {
        if(player.GetCandyContainer().candyAmount >= mrFox.GetCurrentShopItem().price) {

            player.GetCandyContainer().DecrementCandyAmount(mrFox.GetCurrentShopItem().price);
            player.GetComponent<PowerUpComponent>().OnPickedUp(mrFox.GetCurrentShopItem(), mrFox.GetCurrentShopItem().destroyAfterBought);
    
            ShowTextBoxManager(tbManagerOnBought);
    
        } else {
            ShowTextBoxManager(tbManagerOnNotEnoughMoney);
        }
    }

    public void OnNoItemChosen() {
        ShowTextBoxManager(tbManagerNotBought);
    }

	public void OnTextBoxDoneAndHidden() {
         if(currentTextBoxManager != tbManagerBeforeBuy) {
            player.GetComponent<PlayerInputComponent>().enabled = true;
			FinishAction(mrFoxActionTypeOnDone);
        }
    }
}