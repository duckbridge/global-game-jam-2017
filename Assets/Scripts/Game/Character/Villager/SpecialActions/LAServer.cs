using UnityEngine;
using System.Collections;

public class LAServer : VillagerSpecialAction {

    public string password;
    private OnScreenKeyboard onScreenKeyboard;

    private Player player;
    private Villager villager;

    public void Start() {
         onScreenKeyboard = SceneUtils.FindObject<OnScreenKeyboardActivator>().onScreenKeyboard;
    }

    public override void DoAction(Villager villager) {

         this.player = SceneUtils.FindObject<Player>();
         this.villager = villager;

        if(!SceneUtils.FindObject<PlayerSaveComponent>().HasLaCorrectPasswordInserted()) {

            base.DoAction(villager);
            
            SoundUtils.SetSoundVolumeToSavedValueForGameObject(SoundType.FX, onScreenKeyboard.gameObject);
            onScreenKeyboard.gameObject.SetActive(true);
            onScreenKeyboard.ResetText();
            
            onScreenKeyboard.AddEventListener(this.gameObject);
            player.GetComponent<PlayerInputComponent>().enabled = false;

        } else {

            player.GetComponent<PlayerInputComponent>().enabled = true;
            player.OnTalkingDone();

        }
    }

    public void OnSubmitPressed(string outputText) {
        if(outputText.Equals(password)) {

            Logger.Log("CORRECT PASSWORD!");

            SceneUtils.FindObject<PauseScreenManager>().SwitchPauseScreen(PauseScreenTypes.Regular);

            onScreenKeyboard.gameObject.SetActive(false);
            villager.GetAnimationManager().PlayAnimationByName("CorrectPassword", true);
            SceneUtils.FindObject<PlayerSaveComponent>().OnLaCorrectPasswordInserted();
            player.GetComponent<PlayerInputComponent>().enabled = true;

        } else {
            onScreenKeyboard.gameObject.SetActive(false);
            villager.GetAnimationManager().PlayAnimationByName("InCorrectPassword", true);
            player.GetComponent<PlayerInputComponent>().enabled = true;

        }
    }
}
