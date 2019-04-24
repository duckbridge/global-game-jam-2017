using UnityEngine;
using System.Collections;

public class LaHacker : VillagerThatDoesSpecialActionAfterTalking {

    public TextBoxManager regularTextBox, textBoxOnPasswordInserted, textboxOnCassetteThrown;

    public VillagerSpecialAction regularVillageSpecialAction;
    public LAVillagerThrowCassette throwCassetteAction;

    private VillagerSpecialAction villageSpecialActionToUse;

    private bool hasInsertedCorrectPassword = false;
    private PlayerSaveComponent playerSaveComponent;

    public override void Start() {
        base.Start();

        Invoke("LoadCorrectPasswordInsertedDelayed", 1f);
    }

    private void LoadCorrectPasswordInsertedDelayed() {
        playerSaveComponent = SceneUtils.FindObject<PlayerSaveComponent>();
        hasInsertedCorrectPassword = playerSaveComponent.HasLaCorrectPasswordInserted();
       
        SetCorrectTextBoxAndSpecialAction();
    }

    public override void OnInteract(Player player) {

        LoadCorrectPasswordInsertedDelayed();

        base.OnInteract(player);
    }

	public override void OnTextBoxDoneAndHidden() {
        player.OnTalkingDone();
        player.GetComponent<PlayerInputComponent>().enabled = true;

        if(villageSpecialActionToUse != null) {
            villageSpecialActionToUse.DoAction(this);
        }
    }

    private void SetCorrectTextBoxAndSpecialAction() {

        if(playerSaveComponent.GetUnlockedTileTypeTracks().Contains(throwCassetteAction.cassettePickup.tileType)) {
           villageSpecialActionToUse = null;
           textManager = textboxOnCassetteThrown;
        } else {

            if(hasInsertedCorrectPassword) {
                villageSpecialActionToUse = throwCassetteAction;
                textManager = textBoxOnPasswordInserted;
            } else {
               villageSpecialActionToUse = regularVillageSpecialAction;
               textManager = regularTextBox;
            }
        }
    }
}
